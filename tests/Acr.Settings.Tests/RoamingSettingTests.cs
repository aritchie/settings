#if !NETCORE && !MSTEST
using System;
using NUnit.Framework;


namespace Acr.Settings.Tests {

    [TestFixture]
    public class RoamingSettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new SettingsImpl("acr.settings.tests");
        }


        [Test]
        public void GlobalInstanceTest() {
            Acr.Settings.Settings.InitRoaming("acr.settings.tests");
            Assert.NotNull(Acr.Settings.Settings.Roaming);
            Assert.IsTrue(Acr.Settings.Settings.Roaming.IsRoamingProfile, "Romaing profile NOT detected");
        }
    }
}
#endif