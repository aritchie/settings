using System;
using System.Collections.Generic;
using System.Text;

namespace Acr.Settings.Tests
{
    public class TestBind : AbstractSettingObject
    {
        public string StringProperty { get; set; }
        public Guid? NullableProperty { get; set; }

        [Ignore]
        public int? IgnoredProperty { get; set; }
    }
}
