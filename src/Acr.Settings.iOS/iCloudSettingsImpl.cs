using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;


namespace Acr.Settings {

    public class iCloudSettingsImpl : AbstractSettings {

        protected NSUbiquitousKeyValueStore Store => NSUbiquitousKeyValueStore.DefaultStore;


        public override bool Contains(string key) {
            return (this.Store.ValueForKey(new NSString(key)) != null);
        }


        protected override void NativeClear() {
            var prefs = NSUbiquitousKeyValueStore.DefaultStore;
            var values = this.NativeValues();
            foreach (var item in values)
                if (this.ShouldClear(item.Key))
                    prefs.Remove(item.Key);

            prefs.Synchronize();
        }


        protected override void NativeSet(Type type, string key, object value) {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode) {

                case TypeCode.Boolean:
                    this.Store.SetBool(key, (bool)value);
                    break;

                case TypeCode.Double:
                    this.Store.SetDouble(key, (double)value);
                    break;

                case TypeCode.Int64:
                    this.Store.SetLong(key, (long)value);
                    break;

                case TypeCode.String:
                    this.Store.SetString(key, (string)value);
                    break;

                default:
                    var @string = this.Serialize(type, value);
                    this.Store.SetString(key, @string);
                    break;
            }

            this.Store.Synchronize();
        }


        protected override object NativeGet(Type type, string key) {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode) {

                case TypeCode.Boolean:
                    return this.Store.GetBool(key);

                case TypeCode.Double:
                    return this.Store.GetDouble(key);

                case TypeCode.Int64:
                    return this.Store.GetLong(key);

                case TypeCode.String:
                    return this.Store.GetString(key);

                default:
                    var @string = this.Store.GetString(key);
                    return this.Deserialize(type, @string);
            }
        }


        protected override void NativeRemove(string key) {
            this.Store.Remove(key);
            this.Store.Synchronize();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this
                .Store
                .ToDictionary()
                .ToDictionary(
                    x => x.Key.ToString(),
                    x => x.Value.ToString()
                );
        }
    }
}