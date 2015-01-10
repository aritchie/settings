using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private static readonly ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context.ApplicationContext);


        protected override IDictionary<string, string> GetNativeSettings() {
            return prefs.All.ToDictionary(y => y.Key, y => y.Value.ToString());
        }


        protected override void AddOrUpdateNative(IEnumerable<KeyValuePair<string, string>> saves) {
            using (var editor = prefs.Edit()) {
                foreach (var item in saves)
                    editor.PutString(item.Key, item.Value);

                editor.Commit();
            }
        }


        protected override void RemoveNative(IEnumerable<KeyValuePair<string, string>> deletes) {
            using (var editor = prefs.Edit()) {
                foreach (var item in deletes) 
                    editor.Remove(item.Key);

                editor.Commit();
            }
        }


        protected override void ClearNative() {
            using (var editor = prefs.Edit()) {
                editor.Clear();
                editor.Commit();
            }
        }
    }
}