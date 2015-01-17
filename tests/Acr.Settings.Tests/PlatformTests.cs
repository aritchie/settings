using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
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
		public void Resync() {
		}


		[Test]
		public async void OnSettingChanged() {
			SettingChangeEventArgs eventArgs = null;
			this.settings.Changed += (sender, args) => eventArgs = args;

			this.settings.Set("OnSettingChanged", "boo");
			await Task.Delay(100);

			Assert.AreEqual(SettingChangeAction.Add, eventArgs.Action);
			Assert.AreEqual("OnSettingChanged", eventArgs.Key, "Event not fired");
			Assert.AreEqual("boo", eventArgs.Value, "Values not set");
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