using System;
using Acr.Settings.Tests;
using NUnit.Framework;


namespace Acr.Settings.NetCore.Tests {

    [TestFixture]
    public class DatabaseSettingsTest : AbstractSettingTests {

        protected override ISettings Create() {
            return new DatabaseSettingsImpl("test", "settingstest");
        }


        [TestFixtureSetUp]
        public void OnStartup() {
            // TODO: create db and drop at the end
            var impl = (DatabaseSettingsImpl)this.Create();
            try {
                impl.DropTable();
            }
            catch { }
            impl.CreateTable();
        }


        // TODO: additional tests
    }
}
