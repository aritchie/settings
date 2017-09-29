using System;
using System.Collections.Generic;


namespace Acr.Settings
{
    public class SettingsImpl : AbstractSettings
    {
        public override bool Contains(string key)
        {
            throw new NotImplementedException();
        }


        protected override object NativeGet(Type type, string key)
        {
            throw new NotImplementedException();
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            throw new NotImplementedException();
        }


        protected override void NativeRemove(string[] keys)
        {
            throw new NotImplementedException();
        }


        protected override IDictionary<string, string> NativeValues()
        {
            throw new NotImplementedException();
        }
    }
}
