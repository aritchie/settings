using System;


namespace Acr.Settings {

    public interface ISettings {

        ISettingsDictionary All { get; }
		event EventHandler<SettingChangeEventArgs> Changed;

        string Get(string key, string defaultValue = null);
        void Set(string key, string value);
        void Remove(string key);
        void Clear();
        void Resync();
        bool Contains(string key);
    }
}
