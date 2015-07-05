using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private readonly NSUserDefaults prefs;


        public SettingsImpl(string nameSpace) {
            this.KeysNotToClear = new List<string> {
                "WebKitKerningAndLigaturesEnabledByDefault",
                "AppleLanguages",
                "monodevelop-port",
                "AppleITunesStoreItemKinds",
                "AppleLocale",
                "connection-mode",
                "AppleKeyboards",
                "NSLanguages",
                "UIDisableLegacyTextView",
                "NSInterfaceStyle"
            };

            if (nameSpace == null) {
                this.prefs = NSUserDefaults.StandardUserDefaults;
                this.IsRoamingProfile = false;
            }
            else {
                this.prefs = new NSUserDefaults(nameSpace, NSUserDefaultsType.SuiteName);
                this.IsRoamingProfile = true;
            }
        }


        public override bool Contains(string key) {
            return (this.prefs.ValueForKey(new NSString(key)) != null);
        }


        protected override void NativeClear() {
            var values = this.NativeValues();
            foreach (var item in values)
                if (this.ShouldClear(item.Key))
                    this.prefs.RemoveObject(item.Key);

            this.prefs.Synchronize();
        }


        protected override object NativeGet(Type type, string key) {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode) {

                case TypeCode.Boolean:
                    return this.prefs.BoolForKey(key);

                case TypeCode.Double:
                    return this.prefs.DoubleForKey(key);

                case TypeCode.Int32:
                    // returns nint now, type case to int
                    return (int)this.prefs.IntForKey(key);

                case TypeCode.Single:
#warning Still returns float, not nfloat.  Xamarin likely to break since above was recently changed
                    return this.prefs.FloatForKey(key);

                case TypeCode.String:
                    return this.prefs.StringForKey(key);

                default:
                    //this.prefs.StringArrayForKey("")
                    //this.prefs.ArrayForKey(key)
                    var @string = this.prefs.StringForKey(key);
                    return this.Deserialize(type, @string);
            }
        }


        protected override void NativeSet(Type type, string key, object value) {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode) {

                case TypeCode.Boolean:
                    this.prefs.SetBool((bool)value, key);
                    break;

                case TypeCode.Double:
                    this.prefs.SetDouble((double)value, key);
                    break;

                case TypeCode.Int32:
                    this.prefs.SetInt((int)value, key);
                    break;

                case TypeCode.String:
                    this.prefs.SetString((string)value, key);
                    break;

                default:
                    var @string = this.Serialize(type, value);
                    this.prefs.SetString(@string, key);
                    break;
            }
			this.prefs.Synchronize();
        }


        protected override void NativeRemove(string key) {
            this.prefs.RemoveObject(key);
            this.prefs.Synchronize();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this
                .prefs
                .ToDictionary()
                .ToDictionary(
                    x => x.Key.ToString(),
                    x => x.Value.ToString()
                );
        }
    }
}