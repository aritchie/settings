using System;
using NUnit.Framework;


namespace Acr.Settings.Android.Tests {

    [TestFixture]
    public class PlatformTests : TestCases {

        protected override ISettings CreateInstance() {
            return new SettingsImpl();
        }


		[Test]
		public override void GetDefault() {
			return base.GetDefault();
		}
    }
}