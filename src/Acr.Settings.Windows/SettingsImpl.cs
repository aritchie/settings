using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private readonly ApplicationDataContainer container;


        public SettingsImpl(bool isRoaming = false) {
            this.IsRoamingProfile = isRoaming;
            this.container = this.IsRoamingProfile
                ? ApplicationData.Current.RoamingSettings
                : ApplicationData.Current.LocalSettings;
        }


        public override bool Contains(string key) {
            return this.container.Values.ContainsKey(key);
        }


        protected override void NativeClear() {
            this.container.Values.Clear();
        }


        protected override string NativeGet(string key) {
            return (string)this.container.Values[key];
        }


        protected override void NativeRemove(string key) {
            this.container.Values.Remove(key);
        }


        protected override void NativeSet(string key, string value) {
            this.container.Values[key] = value;
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.container
                .Values
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }
    }
}
