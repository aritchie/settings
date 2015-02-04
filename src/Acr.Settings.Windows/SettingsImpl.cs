using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {

        public override bool Contains(string key) {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }


        protected override void NativeClear() {
            ApplicationData.Current.LocalSettings.Values.Clear();
        }


        protected override string NativeGet(string key) {
            return (string)ApplicationData.Current.LocalSettings.Values[key];
        }


        protected override void NativeRemove(string key) {
            ApplicationData.Current.LocalSettings.Values.Remove(key);
        }


        protected override void NativeSet(string key, string value) {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }


        protected override IDictionary<string, string> NativeValues() {
            return ApplicationData
                .Current
                .LocalSettings
                .Values
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }
    }
}
