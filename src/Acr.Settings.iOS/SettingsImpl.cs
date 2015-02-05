using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private static readonly NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;

        public static readonly string[] ProtectedSettingsKeys = {
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


        public override bool Contains(string key) {
            return (prefs.ValueForKey(new NSString(key)) != null);
        }


        protected override void NativeClear() {
            var values = this.NativeValues();
            foreach (var item in values)
                if (this.CanTouch(item.Key))
                    prefs.RemoveObject(item.Key);

            prefs.Synchronize();
        }


        protected override string NativeGet(string key) {
            return prefs.StringForKey(key);
        }


        protected override void NativeRemove(string key) {
            prefs.RemoveObject(key);
            prefs.Synchronize();
        }


        protected override void NativeSet(string key, string value) {
            prefs.SetString(value, key);
			prefs.Synchronize();
        }


        protected virtual bool CanTouch(string settingsKey) {
            return !ProtectedSettingsKeys.Any(x => x.Equals(settingsKey, StringComparison.CurrentCultureIgnoreCase));
        }


        protected override IDictionary<string, string> NativeValues() {
            return prefs
                .ToDictionary()
                .Where(x => this.CanTouch(x.Key.ToString()))
                .ToDictionary(
                    x => x.Key.ToString(),
                    x => x.Value.ToString()
                );
        }
    }
}