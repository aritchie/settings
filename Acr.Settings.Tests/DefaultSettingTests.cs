using System;

namespace Acr.Settings.Tests
{

    public class DefaultSettingTests : AbstractSettingTests
    {
        public DefaultSettingTests()
        {
            this.Settings = CrossSettings.Current;
        }
    }
}
