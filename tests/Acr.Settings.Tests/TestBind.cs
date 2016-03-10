using System;
using System.Collections.Generic;
using System.Text;

namespace Acr.Settings.Tests
{
    public class TestBind : AbstractSettingObject
    {
        string stringProperty;

        public string StringProperty
        {
            get { return this.stringProperty; }
            set { this.SetProperty(ref this.stringProperty, value); }
        }


        Guid? nullProperty;
        public Guid? NullableProperty
        {
            get { return this.nullProperty; }
            set { this.SetProperty(ref this.nullProperty, value); }
        }


        int? ignoredProperty;

        [Ignore]
        public int? IgnoredProperty
        {
            get { return this.ignoredProperty; }
            set { this.SetProperty(ref this.ignoredProperty, value); }
        }
    }
}
