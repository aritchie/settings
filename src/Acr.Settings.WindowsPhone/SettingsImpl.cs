using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        readonly ApplicationDataContainer container;


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
            var keys = this.container
                .Values
                .Where(x => this.ShouldClear(x.Key))
                .Select(x => x.Key)
                .ToList();

            foreach (var key in keys)
                this.container.Values.Remove(key);
        }


        protected override object NativeGet(Type type, string key) {
            return this.container.Values[key];
        }


        protected override void NativeSet(Type type, string key, object value) {
            this.container.Values[key] = value;
        }


        protected override void NativeRemove(string key) {
            this.container.Values.Remove(key);
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
