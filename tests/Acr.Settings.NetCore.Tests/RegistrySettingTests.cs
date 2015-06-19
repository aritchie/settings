using System;
using Acr.Settings.Tests;
using Microsoft.Win32;
using NUnit.Framework;


namespace Acr.Settings.NetCore.Tests {

    [TestFixture]
    public class RegistrySettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new RegistrySettingsImpl("acrsettings", false);
        }


        [TestFixtureTearDown]
        public void OnTestsDone() {
            Registry.CurrentUser.DeleteSubKey("acrsettings", false);
        }
    }
}
