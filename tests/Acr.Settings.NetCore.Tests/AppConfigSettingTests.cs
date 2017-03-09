using System;
using Acr.Settings.Tests;
using NUnit.Framework;


namespace Acr.Settings.NetCore.Tests
{

    [TestFixture]
    public class AppConfigSettingTests : AbstractSettingTests
    {

        protected override ISettings Create()
        {
            return new AppConfigSettingsImpl("test.config");
        }
    }
}
