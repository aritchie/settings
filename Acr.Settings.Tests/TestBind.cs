using System;

namespace Acr.Settings.Tests
{
    public class TestBind : AbstractSettingObject
    {
        string stringProperty;
        public string StringProperty
        {
            get => this.stringProperty;
            set => this.SetProperty(ref this.stringProperty, value);
        }


        Guid? nullProperty;
        public Guid? NullableProperty
        {
            get => this.nullProperty;
            set => this.SetProperty(ref this.nullProperty, value);
        }


        int? ignoredProperty;
        [Ignore]
        public int? IgnoredProperty
        {
            get => this.ignoredProperty;
            set => this.SetProperty(ref this.ignoredProperty, value);
        }
    }
}
