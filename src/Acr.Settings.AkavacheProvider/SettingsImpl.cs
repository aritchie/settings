using System;
using System.Collections.Generic;
using System.ComponentModel;
using Akavache;
using Newtonsoft.Json;


namespace Acr.Settings.AkavacheProvider
{
    public class SettingsImpl : ISettings
    {
        public SettingsImpl()
        {
            BlobCache.ApplicationName = "";
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
            //BlobCache.LocalMachine
            //BlobCache.UserAccount
            //BlobCache.Secure
        }


        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        public List<string> KeysNotToClear { get; }
        public IReadOnlyDictionary<string, string> List { get; }
        public event EventHandler<SettingChangeEventArgs> Changed;
        public T Get<T>(string key, T defaultValue = default(T))
        {
            throw new NotImplementedException();
        }

        public object GetValue(Type type, string key, object defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public T GetRequired<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool SetDefault<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Bind<T>() where T : INotifyPropertyChanged, new()
        {
            throw new NotImplementedException();
        }

        public void Bind(INotifyPropertyChanged obj)
        {
            throw new NotImplementedException();
        }

        public void UnBind(INotifyPropertyChanged obj)
        {
            throw new NotImplementedException();
        }
    }
}
