using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;


namespace Acr.Settings
{

    public class SettingsImpl : AbstractSettings
    {
        readonly string nameSpace;
        readonly object syncLock;
        ISharedPreferences prefs;


        public SettingsImpl(string nameSpace)
        {
            this.syncLock = new object();
            this.nameSpace = nameSpace;
            this.IsRoamingProfile = (nameSpace != null);
        }


        ISharedPreferences CreatePreferences()
        {
            var ctx = Application.Context.ApplicationContext;
            return this.nameSpace == null
                ? PreferenceManager.GetDefaultSharedPreferences(ctx)
                : ctx.GetSharedPreferences(this.nameSpace, FileCreationMode.WorldWriteable);
        }


        void UoW(Action<ISharedPreferences, ISharedPreferencesEditor> doWork)
        {
            lock (this.syncLock)
            {
                this.prefs = this.prefs ?? this.CreatePreferences();
                using (var editor = prefs.Edit())
                {
                    doWork(prefs, editor);
                    editor.Commit();
                }
            }
        }


        public override bool Contains(string key)
        {
            lock (this.syncLock)
            {
                this.prefs = this.prefs ?? this.CreatePreferences();
                return this.prefs.Contains(key);
            }
        }


        protected override object NativeGet(Type type, string key)
        {
            lock (this.syncLock)
            {
                this.prefs = this.prefs ?? this.CreatePreferences();
                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {

                    case TypeCode.Boolean:
                        return this.prefs.GetBoolean(key, false);

                    case TypeCode.Int32:
                        return this.prefs.GetInt(key, 0);

                    case TypeCode.Int64:
                        return this.prefs.GetLong(key, 0);

                    case TypeCode.Single:
                        return this.prefs.GetFloat(key, 0);

                    case TypeCode.String:
                        return this.prefs.GetString(key, String.Empty);

                    default:
                        var @string = this.prefs.GetString(key, String.Empty);
                        return this.Deserialize(type, @string);
                }

            }
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            this.UoW((prefs, x) =>
            {
                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {

                    case TypeCode.Boolean:
                        x.PutBoolean(key, (bool)value);
                        break;

                    case TypeCode.Int32:
                        x.PutInt(key, (int)value);
                        break;

                    case TypeCode.Int64:
                        x.PutLong(key, (long)value);
                        break;

                    case TypeCode.Single:
                        x.PutFloat(key, (float)value);
                        break;

                    case TypeCode.String:
                        x.PutString(key, (string)value);
                        break;

                    default:
                        var @string = this.Serialize(type, value);
                        x.PutString(key, @string);
                        break;
                }
            });
        }


        protected override void NativeRemove(string[] keys)
        {
            this.UoW((prefs, x) =>
            {
                foreach (var key in keys)
                    x.Remove(key);
            });
        }


        protected override IDictionary<string, string> NativeValues()
        {
            lock (this.syncLock)
            {
                this.prefs = this.prefs ?? this.CreatePreferences();
                return this.prefs.All.ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
            }
        }
    }
}