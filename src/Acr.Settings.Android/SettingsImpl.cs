using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private static readonly Lazy<ISharedPreferences> prefs = new Lazy<ISharedPreferences>(() => PreferenceManager.GetDefaultSharedPreferences(Application.Context.ApplicationContext));


        public override bool Contains(string key) {
            return prefs.Value.Contains(key);
        }


        protected override void NativeClear() {
            using (var editor = prefs.Value.Edit()) {
                editor.Clear();
                editor.Commit();
            }
        }


        protected override string NativeGet(string key) {
            return prefs.Value.GetString(key, null);
        }


        protected override void NativeRemove(string key) {
            using (var editor = prefs.Value.Edit()) {
                editor.Remove(key);
                editor.Commit();
            }
        }


        protected override void NativeSet(string key, string value) {
            using (var editor = prefs.Value.Edit()) {
                editor.PutString(key, value);
                editor.Commit();
            }
        }


        protected override IDictionary<string, string> NativeValues() {
            return prefs
                .Value
                .All
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }
    }
}