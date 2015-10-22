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


        protected override void NativeClear() {
            var keys = this.container
                .Where(x => this.ShouldClear(x.Key))
                .Select(x => x.Key)
                .ToList();

            foreach (var key in keys)
                this.container.Remove(key);
        }


        protected override object NativeGet(Type type, string key) {
            var @string = (string)this.container[key];
            var @object = this.Deserialize(type, @string);
            return @object;
        }


        protected override void NativeSet(Type type, string key, object value) {
            var @string = this.Serialize(type, value);
            this.container[key] = @string;
        }


        protected override void NativeRemove(string key) {
            this.container.Remove(key);
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.container.ToDictionary(
                x => x.Key,
                x => x.Value.ToString()
            );
        }
    }
}
