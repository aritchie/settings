#if !NETCORE
using System;
#if MSTESTS
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using NUnit.Framework;
#endif

namespace Acr.Settings.Tests {

#if MSTESTS
        [TestClass]
#else
        [TestFixture]
#endif
    public class LocalSettingTests : AbstractSettingTests {

        protected override ISettings Create() {
#if WINDOWS_UWP
            return new SettingsImpl(false);
#else
            return new SettingsImpl(null);
#endif
        }


#if MSTESTS
        [TestMethod]
#else
        [Test]
#endif
        public void GlobalInstanceTest() {
            Assert.IsNotNull(Acr.Settings.Settings.Local);
            Assert.IsFalse(Acr.Settings.Settings.Local.IsRoamingProfile, "Romaing profile detected");
        }
    }
}
#endif