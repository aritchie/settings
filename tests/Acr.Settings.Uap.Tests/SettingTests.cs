using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;


namespace Acr.Settings.Tests {

    [TestClass]
    public class SettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new SettingsImpl(false);
        }


        [TestInitialize]
        public override void OnSetup() {
            base.OnSetup();
		}


        [TestMethod]
        public override async Task OnSettingChanged() {
            await base.OnSettingChanged();
		}


        [TestMethod]
		public override void Object() {
            base.Object();
		}


        [TestMethod]
        public override void IntTest() {
            base.IntTest();
        }


        [TestMethod]
        public override void IntNullTest() {
            base.IntNullTest();
        }


        [TestMethod]
        public override void DateTimeNullTest() {
            base.DateTimeNullTest();
        }


        [TestMethod]
		public override void SetOverride() {
            base.SetOverride();
		}


        [TestMethod]
        public override void ContainsTest() {
            base.ContainsTest();
        }


        [TestMethod]
        public override void RemoveTest() {
            base.RemoveTest();
        }


        [TestMethod]
        public override void LongTest() {
            base.LongTest();
        }


        [TestMethod]
		public override void GuidTest() {
            base.GuidTest();
		}


        [TestMethod]
        public override void SetNullRemoves() {
            base.SetNullRemoves();
        }


        [TestMethod]
        public override void SetDefaultRemoves() {
            base.SetDefaultRemoves();
        }


        [TestMethod]
		public override void GetDefaultParameter() {
			base.GetDefaultParameter();
		}


        [TestMethod]
        public override void TryDefaults() {
            base.TryDefaults();
        }


        [TestMethod]
        public override void ClearPreserveList() {
            base.ClearPreserveList();
        }
    }
}