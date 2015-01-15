using System;
using NUnit.Framework;


namespace Acr.Settings.Android.Tests {

    [TestFixture]
    public abstract class TestCases {
        private ISettings settings;


        protected abstract ISettings CreateInstance();

        [SetUp]
        public virtual void OnSetup() {
            this.settings = this.CreateInstance();
            this.settings.Clear();
        }


        [Test]
        public void Resync() {
            
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