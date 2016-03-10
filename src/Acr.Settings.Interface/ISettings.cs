using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;


namespace Acr.Settings {

    public interface ISettings {

        JsonSerializerSettings JsonSerializerSettings { get; set; }

        bool IsRoamingProfile { get; }

        /// <summary>
        /// Any values in this list will not be removed using clear
        /// </summary>
        List<string> KeysNotToClear { get; }

        /// <summary>
        /// List current values from settings store
        /// </summary>
        IReadOnlyDictionary<string, string> List { get; }

        /// <summary>
        /// Monitor setting events (Add, Update, Remove, Clear)
        /// </summary>
		event EventHandler<SettingChangeEventArgs> Changed;

        /// <summary>
        /// Gets an object from settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T Get<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Loosely typed version of Get
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        object GetValue(Type type, string key, object defaultValue = null);

        /// <summary>
        /// Enforces that the key is set and returns value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetRequired<T>(string key);

        /// <summary>
        /// Send any value/object to the settings store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Loosely typed version of Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(string key, object value);

        /// <summary>
        /// This will only set the value if the setting is not currently set.  Will not fire Changed event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetDefault<T>(string key, T value);

        /// <summary>
        /// Remove the setting by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// Checks if the setting key is set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// Clears all setting values
        /// </summary>
        void Clear();

        /// <summary>
        /// Will create a bound object set FROM the settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix">Set this if you want to send multiple objects of this type to settings - otherwise default of type name is assumed</param>
        void Bind<T>() where T : INotifyPropertyChanged, new();

        /// <summary>
        /// Unbinds an object from monitoring
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prefix"></param>
        void UnBind(INotifyPropertyChanged obj, string prefix = null);
    }
}
