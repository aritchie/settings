using System;
using NUnit.Framework;
using System.Threading.Tasks;


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
		public void Resync() {
		}


		[Test]
		public async void OnSettingChanged() {
			string key = null;

			this.settings.All.CollectionChanged += (sender, e) => {
//				e.NewItems
			};
			this.settings.Set("OnSettingChanged", "boo");
			await Task.Delay(100);

			var value = this.settings.Get("OnSettingChanged");
			Assert.AreEqual("OnSettingChanged", key, "Event not fired");
			Assert.AreEqual("foo", value, "Values not set");
		}


		[Test]
		public void Object() {
			var inv = new Tuple<int, string>(1, "2");
			this.settings.SetObject("Object", inv);

			var outv = this.settings.GetObject<Tuple<int, string>>("Object");
			Assert.AreEqual(inv.Item1, outv.Item1);
			Assert.AreEqual(inv.Item2, outv.Item2);
		}


		[Test]
		public void SetOverride() {
			this.settings.Set("Test", "1");
			this.settings.Set("Test", "2");
			var r = this.settings.Get("Test");
			Assert.AreEqual("2", r);
		}


		[Test]
		public void GetDefault() {
			var tmp = Guid.NewGuid().ToString();
			var r = this.settings.Get("GetDefault", tmp);
			Assert.AreEqual(r, tmp);
		}
    }
}