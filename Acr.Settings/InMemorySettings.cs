using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Settings
{
    /// <summary>
    /// This is a simple (non-thread safe) dictionary useful for unit testing.  NOT INTENDED FOR PRODUCTION!
    /// </summary>
    public class InMemorySettings : AbstractSettings
    {
        readonly IDictionary<string, object> settings = new Dictionary<string, object>();


        protected override IDictionary<string, string> NativeValues()
            => this.settings.ToDictionary(
                x => x.Key,
                x => x.Value.ToString()
            );
        public override bool Contains(string key) => this.settings.ContainsKey(key);
        protected override object NativeGet(Type type, string key) => this.settings[key];
        protected override void NativeSet(Type type, string key, object value) => settings[key] = value;


        protected override void NativeRemove(string[] keys)
        {
            foreach (var key in keys)
                this.settings.Remove(key);
        }
    }
}