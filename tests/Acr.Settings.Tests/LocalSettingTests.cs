#if !NETCORE
using System;
using NUnit.Framework;


namespace Acr.Settings.Tests {

#if MSTESTS
        [TestClass]
#else
        [TestFixture]
#endif
    public class LocalSettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new SettingsImpl(null);
        }


#if MSTESTS
        [TestMethod]
#else
        [Test]
#endif
        public void GlobalInstanceTest() {
            Assert.NotNull(Acr.Settings.Settings.Local);
            Assert.IsFalse(Acr.Settings.Settings.Local.IsRoamingProfile, "Romaing profile detected");
        }
    }
}
#endif