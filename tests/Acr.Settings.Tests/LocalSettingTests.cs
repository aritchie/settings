#if !NETCORE && !MSTESTS
using System;
using NUnit.Framework;

namespace Acr.Settings.Tests {

    [TestFixture]
    public class LocalSettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new SettingsImpl();
        }
    }
}
#endif