using System;
using Acr.Settings.Android.Tests;
using NUnit.Framework;


namespace Acr.Settings.iOS.Tests {

    [TestFixture]
    public class PlatformTests : TestCases {

        protected override ISettings CreateInstance() {
            return new SettingsImpl();
        }
    }
}