using System;
using Acr.Settings.Tests;
using NUnit.Framework;


namespace Acr.Settings.NetCore.Tests
{

    [TestFixture]
    public class InMemorySettingTests : AbstractSettingTests
    {
        [Test]
        public void InsertNull()
        {
            this.Settings.Set<string>("InsertNull", null);
            var value = this.Settings.Get<string>("InsertNull");
            Assert.IsNull(value);
            //var list = this.Settings.List;
        }


        protected override ISettings Create() => new InMemorySettingsImpl();
    }
}
