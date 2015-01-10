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


        protected override IDictionary<string, string> GetNativeSettings() {
            return prefs
                .ToDictionary()
                .ToDictionary(
                    x => x.Key.ToString(), 
                    x => x.Value.ToString()
                )
                .Where(x => this.CanTouch(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);
        }


        protected override void AddOrUpdateNative(IEnumerable<KeyValuePair<string, string>> saves) {
            foreach (var item in saves)
                if (this.CanTouch(item.Key))
                    prefs.SetString(item.Value, item.Key);

            prefs.Synchronize();
        }


        protected override void RemoveNative(IEnumerable<KeyValuePair<string, string>> dels) {
            foreach (var item in dels)
                if (this.CanTouch(item.Key))
                    prefs.RemoveObject(item.Key);

            prefs.Synchronize();
        }


        protected override void ClearNative() {
            foreach (var item in this.All)
                if (this.CanTouch(item.Key))
                    prefs.RemoveObject(item.Key);

            //prefs.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
            prefs.Synchronize();
        }


        protected virtual bool CanTouch(string settingsKey) {
            return !ProtectedSettingsKeys.Any(x => x.Equals(settingsKey, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}