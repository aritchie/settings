using System;
using System.Collections.Generic;


namespace Acr.Settings
{
    public class SettingsImpl : AbstractSettings
    {
        public override bool Contains(string key)
        {
            throw new NotImplementedException();
        }


        protected override object NativeGet(Type type, string key)
        {
            throw new NotImplementedException();
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            throw new NotImplementedException();
        }


        protected override void NativeRemove(string[] keys)
        {
            throw new NotImplementedException();
        }


        protected override IDictionary<string, string> NativeValues()
        {
            throw new NotImplementedException();
        }
    }
}
/*
using System;
using System.Reflection;
using Newtonsoft.Json;


namespace Acr.Configuration
{
    public class SqliteConfiguration : AbstractConfiguration
    {
        readonly LocalCache userCache;
        readonly LocalCache localCache;
        readonly LocalCache secureCache;


        public SqliteConfiguration(string dbName)
        {
            this.userCache = new LocalCache(new ConfigSqliteConnection($"user_{dbName}"));
            this.localCache = new LocalCache(new ConfigSqliteConnection($"local_{dbName}"));
            this.secureCache = new LocalCache(new ConfigSqliteConnection($"secure_{dbName}"));

            this.userCache.Init();
            this.localCache.Init();
            this.secureCache.Init();
        }


        public override T Get<T>(string key, T defaultValue = default(T), Scope? scope = null)
        {
            var value = this.GetCache(scope).Get(key);
            if (value is string s && typeof(T) != typeof(string))
                return JsonConvert.DeserializeObject<T>(s);

            return value == null
                ? defaultValue
                : (T)value;
        }


        protected override void SetValueNative<T>(string key, object value, Scope scope)
        {
            var cache = this.GetCache(scope);

            if (value != null && !this.IsSimpleType(value.GetType()))
            {
                var @string = JsonConvert.SerializeObject(value);
                cache.Set(key, @string);
            }
            else
            {
                cache.Set(key, value);
            }
        }


        public override bool Contains(string key, Scope? scope = null) => this.GetCache(scope).Contains(key);
        protected override bool RemoveNative(string key, Scope scope) => this.GetCache(scope).Remove(key);


        protected override void ClearNative(Scope? scope)
        {
            if (scope != null)
            {
                this.GetCache(scope).Clear();
            }
            else
            {
                this.localCache.Clear();
                this.secureCache.Clear();
                this.userCache.Clear();
            }
        }


        protected virtual LocalCache GetCache(Scope? scope)
        {
            var sc = scope ?? this.DefaultScope;
            switch (sc)
            {
                case Scope.LocalMachine : return this.localCache;
                case Scope.Secure       : return this.secureCache;
                case Scope.UserAccount  : return this.userCache;
                default:
                    throw new ArgumentException("Invalid scope");
            }
        }


        protected virtual bool IsSimpleType(Type type)
        {
            var ti = type.GetTypeInfo();
            return ti.IsPrimitive || ti.IsValueType || type == typeof(string);
        }
    }
}
     */