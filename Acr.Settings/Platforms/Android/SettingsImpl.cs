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
        readonly object syncLock = new object();
        ISharedPreferences prefs;


        public ISharedPreferences Prefs
        {
            get
            {
                var ctx = Application.Context.ApplicationContext;
                this.prefs = this.prefs ?? PreferenceManager.GetDefaultSharedPreferences(ctx);
                return this.prefs;
            }
        }


        void UoW(Action<ISharedPreferencesEditor> doWork)
        {
            lock (this.syncLock)
            {
                using (var editor = this.Prefs.Edit())
                {
                    doWork(editor);
                    editor.Commit();
                }
            }
        }


        public override bool Contains(string key)
        {
            lock (this.syncLock)
                return this.Prefs.Contains(key);
        }


        protected override object NativeGet(Type type, string key)
        {
            lock (this.syncLock)
            {
                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {

                    case TypeCode.Boolean:
                        return this.Prefs.GetBoolean(key, false);

                    case TypeCode.Int32:
                        return this.Prefs.GetInt(key, 0);

                    case TypeCode.Int64:
                        return this.Prefs.GetLong(key, 0);

                    case TypeCode.Single:
                        return this.Prefs.GetFloat(key, 0);

                    case TypeCode.String:
                        return this.Prefs.GetString(key, String.Empty);

                    default:
                        var @string = this.Prefs.GetString(key, String.Empty);
                        return this.Deserialize(type, @string);
                }

            }
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            this.UoW(x =>
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
            this.UoW(x =>
            {
                foreach (var key in keys)
                    x.Remove(key);
            });
        }


        protected override IDictionary<string, string> NativeValues()
        {
            lock (this.syncLock)
            {
                return this.Prefs.All.ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
            }
        }
    }
}