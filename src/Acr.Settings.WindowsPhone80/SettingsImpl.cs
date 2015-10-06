using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Acr.Settings {

    public class SettingsImpl : AbstractSettings {
        private readonly IsolatedStorageSettings isolatedStore;
        private UTF8Encoding encoding;


        public SettingsImpl(bool isRoaming = false) {
            this.IsRoamingProfile = false;

            isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            encoding = new UTF8Encoding();
        }


        public override bool Contains(string key) {
            return this.isolatedStore.Contains(key);
        }


        protected override void NativeClear() {
            var keys = this.isolatedStore
                .Where(x => this.ShouldClear(x.Key))
                .Select(x => x.Key)
                .ToList();

            foreach (var key in keys)
                this.isolatedStore.Remove(key);


            Save();
        }


        protected override object NativeGet(Type type, string key) {
            return this.isolatedStore[key];
        }


        protected override void NativeSet(Type type, string key, object value) {
            //this.isolatedStore[key] = value;
            AddOrUpdateValue(key, value);
        }


        protected override void NativeRemove(string key) {
            this.isolatedStore.Remove(key);
            Save();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.isolatedStore
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }



        private void AddOrUpdateValue(string key, object value) {
            bool valueChanged = false;

            try {
                // If the new value is different, set the new value.
                if (this.isolatedStore[key] != value) {
                    this.isolatedStore[key] = value;
                    valueChanged = true;
                }
            } catch (KeyNotFoundException) {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            } catch (ArgumentException) {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            }

            if (valueChanged) {
                Save();
            }
        }

        private T GetValueOrDefault<T>(string key, T defaultValue) {
            T value;

            try {
                value = (T)this.isolatedStore[key];
            } catch (KeyNotFoundException) {
                value = defaultValue;
            } catch (ArgumentException) {
                value = defaultValue;
            }

            return value;
        }

        private void Save() {
            isolatedStore.Save();
        }


    }
}
