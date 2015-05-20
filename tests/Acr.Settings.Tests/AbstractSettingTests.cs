using System;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Acr.Settings.Tests {

    public abstract class AbstractSettingTests {
        public ISettings Settings { get; set; }


        protected abstract ISettings Create();


		[SetUp]
		public void OnSetup() {
			this.Settings = this.Create();
			this.Settings.Clear();
		}


		[Test]
		public async void OnSettingChanged() {
            var tcs = new TaskCompletionSource<SettingChangeEventArgs>();
			this.Settings.Changed += (sender, args) => tcs.TrySetResult(args);

			this.Settings.Set("OnSettingChanged", "boo");
			var eventArgs = await tcs.Task;

			Assert.AreEqual(SettingChangeAction.Add, eventArgs.Action);
			Assert.AreEqual("OnSettingChanged", eventArgs.Key, "Event not fired");
			Assert.AreEqual("boo", eventArgs.Value, "Values not set");
		}


		[Test]
		public void Object() {
			var inv = new Tuple<int, string>(1, "2");
			this.Settings.Set("Object", inv);

			var outv = this.Settings.Get<Tuple<int, string>>("Object");
			Assert.AreEqual(inv.Item1, outv.Item1);
			Assert.AreEqual(inv.Item2, outv.Item2);
		}


        [Test]
        public void IntTest() {
            this.Settings.Set("Test", 99);
            var value = this.Settings.Get<int>("Test");
            Assert.AreEqual(99, value);
        }


        [Test]
        public void IntNullTest() {
            var nvalue = this.Settings.Get<int?>("Blah");
            Assert.IsNull(nvalue, "Int? should be null");

            nvalue = 199;
            this.Settings.Set("Blah", nvalue);

            nvalue = this.Settings.Get<int?>("Blah");
            Assert.AreEqual(199, nvalue.Value);
        }


        [Test]
        public void DateTimeNullTest() {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.IsNull(nvalue, "Should be null");

            this.Settings.Set("DateTimeNullTest", dt);
            nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.AreEqual(dt, nvalue);
        }


		[Test]
		public void SetOverride() {
			this.Settings.Set("Test", "1");
			this.Settings.Set("Test", "2");
			var r = this.Settings.Get<string>("Test");
			Assert.AreEqual("2", r);
		}


        [Test]
        public void ContainsTest() {
            var flag = this.Settings.Contains(Guid.NewGuid().ToString());
            Assert.False(flag, "Contains should have returned false");

            this.Settings.Set("Test", "1");
            flag = this.Settings.Contains("Test");
            Assert.True(flag, "Contains should have returned true");
        }


        [Test]
        public void RemoveTest() {
            this.Settings.Set("Test", "1");
            var flag = this.Settings.Remove("Test");
            Assert.True(flag, "Remove should have returned success");
        }


		[Test]
		public void GuidTest() {
			var guid = this.Settings.Get<Guid>("GuidTest");
			Assert.AreEqual(Guid.Empty, guid);

			guid = new Guid();
			this.Settings.Set("GuidTest", guid);
			var tmp = this.Settings.Get<Guid>("GuidTest");
			Assert.AreEqual(guid, tmp);
		}


		[Test]
		public void GetDefault() {
			var tmp = Guid.NewGuid().ToString();
			var r = this.Settings.Get("GetDefault", tmp);
			Assert.AreEqual(r, tmp);
		}
    }
}