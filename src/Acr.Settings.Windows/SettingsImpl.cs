using System;
using Windows.Storage;


namespace Acr.Settings.Windows {
    public class SettingsImpl : ISettings {

        public ISettingsDictionary All {
            get { throw new NotImplementedException(); }
        }


        public string Get(string key, string defaultValue = null) {
            throw new NotImplementedException();
        }


        public void Set(string key, string value) {
            throw new NotImplementedException();
        }


        public void Remove(string key) {
            throw new NotImplementedException();
        }


        public void Clear() {
            throw new NotImplementedException();
        }


        public void Resync() {
            //ApplicationData.Current.LocalSettings
            throw new NotImplementedException();
        }


        public bool Contains(string key) {
            throw new NotImplementedException();
        }
    }
}
