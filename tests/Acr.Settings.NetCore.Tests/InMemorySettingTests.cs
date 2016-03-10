using System;
using Acr.Settings.Tests;
using NUnit.Framework;


namespace Acr.Settings.NetCore.Tests {

    [TestFixture]
    public class InMemorySettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new InMemorySettingsImpl();
        }
    }
}
