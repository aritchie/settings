using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;


namespace Acr.Settings.Uap.Tests {

    [TestClass]
    public class LocalSettingTests {
        public ISettings Settings { get; set; }


        ISettings Create() {
            return new SettingsImpl();
        }


        [TestInitialize]
        public void OnSetup() {
            this.Settings = this.Create();
            this.Settings.Clear();
        }


        [TestMethod]
		public async void OnSettingChanged() {
            var tcs = new TaskCompletionSource<SettingChangeEventArgs>();
			this.Settings.Changed += (sender, args) => tcs.TrySetResult(args);

			this.Settings.Set("OnSettingChanged", "boo");
			var eventArgs = await tcs.Task;

			Assert.AreEqual(SettingChangeAction.Add, eventArgs.Action);
			Assert.AreEqual("OnSettingChanged", eventArgs.Key, "Event not fired");
			Assert.AreEqual("boo", eventArgs.Value, "Values not set");
		}


		[TestMethod]
		public void Object() {
			var inv = new Tuple<int, string>(1, "2");
			this.Settings.Set("Object", inv);

			var outv = this.Settings.Get<Tuple<int, string>>("Object");
			Assert.AreEqual(inv.Item1, outv.Item1);
			Assert.AreEqual(inv.Item2, outv.Item2);
		}


        [TestMethod]
        public void IntTest() {
            this.Settings.Set("Test", 99);
            var value = this.Settings.Get<int>("Test");
            Assert.AreEqual(99, value);
        }


        [TestMethod]
        public void IntNullTest() {
            var nvalue = this.Settings.Get<int?>("Blah");
            Assert.IsNull(nvalue, "Int? should be null");

            nvalue = 199;
            this.Settings.Set("Blah", nvalue);

            nvalue = this.Settings.Get<int?>("Blah");
            Assert.AreEqual(199, nvalue.Value);
        }


        [TestMethod]
        public void DateTimeNullTest() {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.IsNull(nvalue, "Should be null");

            this.Settings.Set("DateTimeNullTest", dt);
            nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.AreEqual(dt, nvalue);
        }


		[TestMethod]
		public void SetOverride() {
			this.Settings.Set("Test", "1");
			this.Settings.Set("Test", "2");
			var r = this.Settings.Get<string>("Test");
			Assert.AreEqual("2", r);
		}


        [TestMethod]
        public void ContainsTest() {
            var flag = this.Settings.Contains(Guid.NewGuid().ToString());
            Assert.IsFalse(flag, "Contains should have returned false");

            this.Settings.Set("Test", "1");
            flag = this.Settings.Contains("Test");
            Assert.IsTrue(flag, "Contains should have returned true");
        }


        [TestMethod]
        public void RemoveTest() {
            this.Settings.Set("Test", "1");
            var flag = this.Settings.Remove("Test");
            Assert.IsTrue(flag, "Remove should have returned success");
        }


        [TestMethod]
        public void LongTest() {
            long value = 1;
            this.Settings.Set("LongTest", value);
            var value2 = this.Settings.Get<long>("LongTest");
            Assert.AreEqual(value, value2);
        }


		[TestMethod]
		public void GuidTest() {
			var guid = this.Settings.Get<Guid>("GuidTest");
			Assert.AreEqual(Guid.Empty, guid);

			guid = new Guid();
			this.Settings.Set("GuidTest", guid);
			var tmp = this.Settings.Get<Guid>("GuidTest");
			Assert.AreEqual(guid, tmp);
		}


        [TestMethod]
        public void SetNullRemoves() {
            this.Settings.Set("SetNullRemoves", "Blah");
            this.Settings.Set<string>("SetNullRemoves", null);
            var contains = this.Settings.Contains("SetNullRemoves");
            Assert.IsFalse(contains);
        }


        [TestMethod]
        public void SetDefaultTRemoves() {
            long value = 1;
            this.Settings.Set("SetDefaultTRemoves", value);
            this.Settings.Set<long>("SetDefaultTRemoves", default(long));
            var contains = this.Settings.Contains("SetDefaultTRemoves");
            Assert.IsFalse(contains);
        }


		[TestMethod]
		public void GetDefaultParameter() {
			var tmp = Guid.NewGuid().ToString();
			var r = this.Settings.Get("GetDefaultParameter", tmp);
			Assert.AreEqual(r, tmp);
		}


        [TestMethod]
        public void TryDefaults() {
            var flag = this.Settings.SetDefault("TryDefaults", "Initial Value");
            Assert.IsTrue(flag, "Default value could not be set");

            flag = this.Settings.SetDefault("TryDefaults", "Second Value");
            Assert.IsFalse(flag, "Default value was set and should not have been");

            var currentValue = this.Settings.Get<string>("TryDefaults");
            Assert.AreEqual("Initial Value", currentValue);
        }


        [TestMethod]
        public void ClearPreserveList() {
            this.Settings.Set("ClearPreserveTest", "Value");
            this.Settings.KeysNotToClear.Add("ClearPreserveTest");
            this.Settings.Clear();
            var value = this.Settings.Get<string>("ClearPreserveTest");
            Assert.AreEqual("Value", value);
        }
    }
}