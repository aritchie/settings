using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private readonly string nameSpace;

        public SettingsImpl(string nameSpace) {
            this.nameSpace = nameSpace;
            this.IsRoamingProfile = (nameSpace != null);
        }


        private ISharedPreferences GetPreferences() {
            var ctx = Application.Context.ApplicationContext;
            return this.nameSpace == null
                ? PreferenceManager.GetDefaultSharedPreferences(ctx)
                : ctx.GetSharedPreferences(this.nameSpace, FileCreationMode.Append);
        }


        private void UoW(Action<ISharedPreferencesEditor> doWork) {
            using (var prefs = this.GetPreferences()) {
                using (var editor = prefs.Edit()) {
                    doWork(editor);
                    editor.Commit();
                }
            }
        }


        public override bool Contains(string key) {
            using (var prefs = this.GetPreferences())
                return prefs.Contains(key);
        }


        protected override void NativeClear() {
            this.UoW(x => x.Clear());
        }


        protected override string NativeGet(string key) {
            using (var prefs = this.GetPreferences())
                return prefs.GetString(key, null);
        }


        protected override void NativeRemove(string key) {
            this.UoW(x => x.Remove(key));
        }


        protected override void NativeSet(string key, string value) {
            this.UoW(x => x.PutString(key, value));
        }


        protected override IDictionary<string, string> NativeValues() {
            using (var prefs = this.GetPreferences())
                return prefs.All.ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }
    }
}