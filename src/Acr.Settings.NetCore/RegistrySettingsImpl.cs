using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;


namespace Acr.Settings.NetCore {

    public class RegistrySettingsImpl : AbstractSettings {
        private readonly RegistryKey registry;
        public bool IsCurrentUser { get; private set; }
        public string RegistryKeyName { get; private set; }


        public RegistrySettingsImpl(string registryKey, bool userLevel) {
            var r = userLevel
                ? Registry.CurrentUser
                : Registry.LocalMachine;

            this.registry = r.OpenSubKey(registryKey) ?? r.CreateSubKey(registryKey);
            this.IsCurrentUser = userLevel;
            this.RegistryKeyName = registryKey;
        }


        public override bool Contains(string key) {
            return this.registry.GetValue(key, null) != null;
        }


        protected override void NativeClear() {
            var values = this.NativeValues();
            foreach (var item in values)
                if (this.ShouldClear(item.Key))
                    this.registry.DeleteValue(item.Key, false);

            this.registry.Flush();
        }


        protected override object NativeGet(Type type, string key) {
            var value = (string)this.registry.GetValue(key);
            var result = this.Deserialize(type, value);
            return result;
        }


        protected override void NativeSet(Type type, string key, object value) {
            var @string = this.Serialize(type, value);
            this.registry.SetValue(key, @string, RegistryValueKind.String);
            this.registry.Flush();
        }


        protected override void NativeRemove(string key) {
            this.registry.DeleteValue(key, false);
            this.registry.Flush();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.registry
                .GetValueNames()
                .ToDictionary(
                    x => x,
                    x => (string)this.registry.GetValue(x)
                );
        }
    }
}
