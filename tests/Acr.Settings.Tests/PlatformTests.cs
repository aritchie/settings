using System;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Acr.Settings.Tests {

    [TestFixture]
    public class PlatformTests {
		private ISettings settings;


		[SetUp]
		public void OnSetup() {
			this.settings = new SettingsImpl();
			this.settings.Clear();
		}


		[Test]
		public async void OnSettingChanged() {
            var tcs = new TaskCompletionSource<SettingChangeEventArgs>();
			this.settings.Changed += (sender, args) => tcs.TrySetResult(args);

			this.settings.Set("OnSettingChanged", "boo");
			var eventArgs = await tcs.Task;

			Assert.AreEqual(SettingChangeAction.Add, eventArgs.Action);
			Assert.AreEqual("OnSettingChanged", eventArgs.Key, "Event not fired");
			Assert.AreEqual("boo", eventArgs.Value, "Values not set");
		}


		[Test]
		public void Object() {
			var inv = new Tuple<int, string>(1, "2");
			this.settings.Set("Object", inv);

			var outv = this.settings.Get<Tuple<int, string>>("Object");
			Assert.AreEqual(inv.Item1, outv.Item1);
			Assert.AreEqual(inv.Item2, outv.Item2);
		}


        [Test]
        public void IntTest() {
            this.settings.Set("Test", 99);
            var value = this.settings.Get<int>("Test");
            Assert.AreEqual(99, value);
        }


        [Test]
        public void IntNullTest() {
            var nvalue = this.settings.Get<int?>("Blah");
            Assert.IsNull(nvalue, "Int? should be null");

            nvalue = 199;
            this.settings.Set("Blah", nvalue);

            nvalue = this.settings.Get<int?>("Blah");
            Assert.AreEqual(199, nvalue.Value);
        }


        [Test]
        [Ignore]
        public void DateTimeNullTest() {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.settings.Get<DateTime?>("DateTimeNullTest");
            Assert.IsNull(nvalue, "Should be null");

            this.settings.Set("DateTimeNullTest", dt);
            nvalue = this.settings.Get<DateTime?>("DateTimeNullTest");
            Assert.AreEqual(dt, nvalue);
        }


		[Test]
		public void SetOverride() {
			this.settings.Set("Test", "1");
			this.settings.Set("Test", "2");
			var r = this.settings.Get<string>("Test");
			Assert.AreEqual("2", r);
		}


        [Test]
        public void ContainsTest() {
            var flag = this.settings.Contains(Guid.NewGuid().ToString());
            Assert.False(flag, "Contains should have returned false");

            this.settings.Set("Test", "1");
            flag = this.settings.Contains("Test");
            Assert.True(flag, "Contains should have returned true");
        }


        [Test]
        public void RemoveTest() {
            this.settings.Set("Test", "1");
            var flag = this.settings.Remove("Test");
            Assert.True(flag, "Remove should have returned success");
        }


		[Test]
		public void GetDefault() {
			var tmp = Guid.NewGuid().ToString();
			var r = this.settings.Get("GetDefault", tmp);
			Assert.AreEqual(r, tmp);
		}
    }
}