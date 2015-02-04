using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;


namespace Acr.Settings {

    public abstract class AbstractSettings : ISettings {
        public event EventHandler<SettingChangeEventArgs> Changed;


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
            this.OnChanged(new SettingChangeEventArgs(action, key,  value));
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
            if (t.GetGenericTypeDefinition() == typeof(Nullable<>))
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
            if (t.GetGenericTypeDefinition() == typeof(Nullable<>))
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



//        public static bool SetIfNotSet<T>(this ISettings settings, string key, T obj) {
//            if (!settings.Contains(key))
//                settings.Set(key, JsonConvert.SerializeObject(obj));
//        }


//        public static int GetInt(this ISettings settings, string key, int defaultValue = 0) {
//            var value = settings.Get(key);
//            var r = defaultValue;
//            if (!Int32.TryParse(value, out r))
//                return defaultValue;

//            return r;
//        }


//        public static long GetLong(this ISettings settings, string key, long defaultValue = 0) {
//            return Int64.Parse(settings.Get(key, defaultValue.ToString()));
//        }


//        public static void SetDateTime(this ISettings settings, string key, DateTime dateTime) {
//            settings.Set(key, dateTime.ToString());
//        }


//        public static DateTime? GetDateTime(this ISettings settings, string key, DateTime? defaultValue = null) {
//            var s = settings.Get(key);
//            if (s == null)
//                return defaultValue;

//            return DateTime.Parse(s);
//        }


//        public static void SetDateTimeOffset(this ISettings settings, string key, DateTimeOffset dateTime) {
//            settings.Set(key, dateTime.ToString());
//        }


//        public static DateTimeOffset? GetDateTimeOffset(this ISettings settings, string key, DateTimeOffset? defaultValue = null) {
//            var s = settings.Get(key);
//            if (s == null)
//                return defaultValue;

//            return DateTimeOffset.Parse(s);
//        }


//        public static void SetTimeSpan(this ISettings settings, string key, TimeSpan ts) {
//            settings.Set(key, ts.Ticks.ToString());
//        }


//        public static TimeSpan GetTimeSpan(this ISettings settings, string key) {
//            var num = settings.GetLong(key, 0);
//            return TimeSpan.FromTicks(num);
//        }