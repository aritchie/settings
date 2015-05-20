using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Reflection;


namespace Acr.Settings {

    public abstract class AbstractSettings : ISettings {
        public event EventHandler<SettingChangeEventArgs> Changed;


        public bool IsRoamingProfile { get; protected set; }

        public virtual IReadOnlyDictionary<string, string> List { get; protected set; }


        public virtual T Get<T>(string key, T defaultValue = default(T)) {
            if (!this.Contains(key))
                return defaultValue;

            var @string = this.NativeGet(key);
            var obj = this.Deserialize<T>(@string);
            return obj;
        }


        public virtual void Set<T>(string key, T value) {
            var action = this.Contains(key)
                ? SettingChangeAction.Update
                : SettingChangeAction.Add;

            var @string = this.Serialize<T>(value);
            this.NativeSet(key, @string);
            this.OnChanged(new SettingChangeEventArgs(action, key, value));
        }


        public virtual bool SetDefault<T>(string key, T value) {
            if (!this.Contains(key))
                return false;

            var @string = this.Serialize<T>(value);
            this.NativeSet(key, @string);
            return true;
        }


        public virtual bool Remove(string key) {
            if (!this.Contains(key))
                return false;

            this.NativeRemove(key);
            return true;
        }


        public virtual void Clear() {
            this.NativeClear();
            this.OnChanged(new SettingChangeEventArgs(SettingChangeAction.Clear, null, null));
        }


		protected virtual void OnChanged(SettingChangeEventArgs args) {
			if (this.Changed != null)
				this.Changed(this, args);

            var native = this.NativeValues();
            this.List = new ReadOnlyDictionary<string, string>(native);
		}


        protected virtual string Serialize<T>(object value) {
            var t = typeof(T);
			if (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                t = Nullable.GetUnderlyingType(t);

            if (t == typeof(string))
                return (string)value;

            if (this.IsStringifyType(t))
                return value.ToString();

            return JsonConvert.SerializeObject(value);
        }


        protected virtual T Deserialize<T>(string value) {
            object result = null;
            var t = typeof(T);
			if (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                t = Nullable.GetUnderlyingType(t);

            if (t == typeof(string))
                result = value;
            else if (this.IsStringifyType(t))
                result = (T)Convert.ChangeType(value, t);
            else
                result = JsonConvert.DeserializeObject<T>(value);

            return (T)result;
        }


        protected virtual bool IsStringifyType(Type t) {
            return (
                t == typeof(DateTime) ||
                t == typeof(DateTimeOffset) ||
                t == typeof(bool) ||
                t == typeof(short) ||
                t == typeof(int) ||
                t == typeof(long) ||
                t == typeof(double) ||
                t == typeof(float) ||
                t == typeof(decimal)
            );
        }


        public abstract bool Contains(string key);

        protected abstract void NativeClear();
        protected abstract string NativeGet(string key);
        protected abstract void NativeRemove(string key);
        protected abstract void NativeSet(string key, string value);
        protected abstract IDictionary<string, string> NativeValues();
    }
}