using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;


namespace Acr.Settings {

    public abstract class AbstractSettings : ISettings {
        public event EventHandler<SettingChangeEventArgs> Changed;


        public bool IsRoamingProfile { get; protected set; }
        public List<string> KeysNotToClear { get; set; }
        public virtual IReadOnlyDictionary<string, string> List { get; protected set; }
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        protected AbstractSettings() {
            this.KeysNotToClear = new List<string>();
        }


        public virtual T Get<T>(string key, T defaultValue = default(T)) {
            try {
                if (!this.Contains(key))
                    return defaultValue;

                var type = this.UnwrapType(typeof(T));
                var value = this.NativeGet(type, key);
                return (T)value;
            }
            catch (Exception ex) {
                throw new ArgumentException($"Error getting key: {key}", ex);
            }
        }


        public virtual T GetRequired<T>(string key) {
            if (!this.Contains(key))
                throw new ArgumentException($"Settings key '{key}' is not set");

            return this.Get<T>(key);
        }


        public virtual void Set<T>(string key, T value) {
            try {
                var action = this.Contains(key)
                    ? SettingChangeAction.Update
                    : SettingChangeAction.Add;

                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                    this.Remove(key);

                else {
                    var type = this.UnwrapType(typeof(T));
                    this.NativeSet(type, key, value);
                    this.OnChanged(new SettingChangeEventArgs(action, key, value));
                }
            }
            catch (Exception ex) {
                throw new ArgumentException($"Error setting key {key} with value {value}", ex);
            }
        }


        public virtual bool SetDefault<T>(string key, T value) {
            if (this.Contains(key))
                return false;

            var type = this.UnwrapType(typeof(T));
            this.NativeSet(type, key, value);
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
			    this.Changed.Invoke(this, args);

            var native = this.NativeValues();
            this.List = new ReadOnlyDictionary<string, string>(native);
		}


        protected virtual string Serialize(Type type, object value) {
            if (type == typeof(string))
                return (string)value;

            if (this.IsStringifyType(type)) {
                var format = value as IFormattable;
                return format == null
                    ? value.ToString()
                    : format.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
            }

            return JsonConvert.SerializeObject(value, this.JsonSerializerSettings);
        }


        protected virtual object Deserialize(Type type, string value) {
            if (type == typeof(string))
                return value;

            if (this.IsStringifyType(type))
                return Convert.ChangeType(value, type, System.Globalization.CultureInfo.InvariantCulture);

            return JsonConvert.DeserializeObject(value, type, this.JsonSerializerSettings);
        }


        protected virtual bool ShouldClear(string key) {
            return !this.KeysNotToClear.Any(x => x.Equals(key));
        }


        protected virtual Type UnwrapType(Type type) {
			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return type;
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
        protected abstract object NativeGet(Type type, string key);
        protected abstract void NativeSet(Type type, string key, object value);
        protected abstract void NativeRemove(string key);
        protected abstract IDictionary<string, string> NativeValues();
    }
}