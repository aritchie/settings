using System;
using System.Collections.Generic;


namespace Acr.Settings {

    public interface ISettings {

        IReadOnlyDictionary<string, string> List { get; }
		event EventHandler<SettingChangeEventArgs> Changed;

        T Get<T>(string key, T defaultValue = default(T));
        void Set<T>(string key, T value);
        bool Remove(string key);
        bool Contains(string key);
        void Clear();
    }
}
