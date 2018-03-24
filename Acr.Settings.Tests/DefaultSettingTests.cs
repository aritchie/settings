using System;

namespace Acr.Settings.Tests
{

    public class DefaultSettingTests : AbstractSettingTests
    {
        public DefaultSettingTests() : base(Acr.Settings.Settings.Current) { }
    }
}
