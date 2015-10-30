using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;


namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        readonly IsolatedStorageSettings container;


        public SettingsImpl(bool isRoaming = false) {
            this.IsRoamingProfile = isRoaming;
            this.container = IsolatedStorageSettings.ApplicationSettings;
        }


        public override bool Contains(string key) {
            return this.container.Contains(key);
        }


        protected override object NativeGet(Type type, string key) {
            var @string = (string)this.container[key];
            var @object = this.Deserialize(type, @string);
            return @object;
        }


        protected override void NativeSet(Type type, string key, object value) {
            var @string = this.Serialize(type, value);
            this.container[key] = @string;
            this.container.Save();
        }


        protected override void NativeRemove(string[] keys) {
            foreach (var key in keys)
                this.container.Remove(key);

            this.container.Save();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.container.ToDictionary(
                x => x.Key,
                x => x.Value.ToString()
            );
        }
    }
}
