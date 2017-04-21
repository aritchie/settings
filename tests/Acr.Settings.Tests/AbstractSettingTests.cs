using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Acr.Settings.Tests
{

    public abstract class AbstractSettingTests
    {
        public ISettings Settings { get; set; }


        protected abstract ISettings Create();

        [SetUp]
        public virtual void OnSetup()
        {
            this.Settings = this.Create();
            this.Settings.Clear();
        }



        [Test]
        public async Task OnSettingChanged()
        {
            var tcs = new TaskCompletionSource<SettingChangeEventArgs>();
            this.Settings.Changed += (sender, args) => tcs.TrySetResult(args);

            this.Settings.Set("OnSettingChanged", "boo");
            var eventArgs = await tcs.Task;

            Assert.AreEqual(SettingChangeAction.Add, eventArgs.Action);
            Assert.AreEqual("OnSettingChanged", eventArgs.Key, "Event not fired");
            Assert.AreEqual("boo", eventArgs.Value, "Values not set");
        }


        [Test]
        public void SetDefault_DoesNotRemove_SimpleType()
        {
            this.Settings.Set("Bool", false);
            Assert.IsTrue(this.Settings.Contains("Bool"), "Bool is missing");

            this.Settings.Set("Long", 0L);
            Assert.IsTrue(this.Settings.Contains("Long"), "Long is missing");
        }


        [Test]
        public void SetDefault_DoesRemove_ComplexType()
        {
            this.Settings.Set("Object", new object());
            Assert.IsTrue(this.Settings.Contains("Object"), "Object is missing");

            this.Settings.Set<object>("Object", null);
            Assert.IsFalse(this.Settings.Contains("Object"), "Object should be missing");
        }


        [Test]
        public void Object()
        {
            var inv = new Tuple<int, string>(1, "2");
            this.Settings.Set("Object", inv);

            var outv = this.Settings.Get<Tuple<int, string>>("Object");
            Assert.AreEqual(inv.Item1, outv.Item1);
            Assert.AreEqual(inv.Item2, outv.Item2);
        }


        [Test]
        public void IntTest()
        {
            this.Settings.Set("Test", 99);
            var value = this.Settings.Get<int>("Test");
            Assert.AreEqual(99, value);
        }


        [Test]
        public void IntNullTest()
        {
            var nvalue = this.Settings.Get<int?>("Blah");
            Assert.IsNull(nvalue, "Int? should be null");

            nvalue = 199;
            this.Settings.Set("Blah", nvalue);

            nvalue = this.Settings.Get<int?>("Blah");
            Assert.AreEqual(199, nvalue.Value);
        }


        [Test]
        public void DateTimeNullTest()
        {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.IsNull(nvalue, "Should be null");

            this.Settings.Set("DateTimeNullTest", dt);
            nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            Assert.AreEqual(dt, nvalue);
        }


        [Test]
        public void SetOverride()
        {
            this.Settings.Set("Test", "1");
            this.Settings.Set("Test", "2");
            var r = this.Settings.Get<string>("Test");
            Assert.AreEqual("2", r);
        }


        [Test]
        public void ContainsTest()
        {
            var flag = this.Settings.Contains(Guid.NewGuid().ToString());
            Assert.IsFalse(flag, "Contains should have returned false");

            this.Settings.Set("Test", "1");
            flag = this.Settings.Contains("Test");
            Assert.IsTrue(flag, "Contains should have returned true");
        }


        [Test]
        public void RemoveTest()
        {
            this.Settings.Set("Test", "1");
            var flag = this.Settings.Remove("Test");
            Assert.IsTrue(flag, "Remove should have returned success");
        }


        [Test]
        public void LongTest()
        {
            long value = 1;
            this.Settings.Set("LongTest", value);
            var value2 = this.Settings.Get<long>("LongTest");
            Assert.AreEqual(value, value2);
        }


        [Test]
        public void GuidTest()
        {
            var guid = this.Settings.Get<Guid>("GuidTest");
            Assert.AreEqual(Guid.Empty, guid);

            guid = new Guid();
            this.Settings.Set("GuidTest", guid);
            var tmp = this.Settings.Get<Guid>("GuidTest");
            Assert.AreEqual(guid, tmp);
        }


        [Test]
        public void SetNullRemoves()
        {
            this.Settings.Set("SetNullRemoves", "Blah");
            this.Settings.Set<string>("SetNullRemoves", null);
            var contains = this.Settings.Contains("SetNullRemoves");
            Assert.IsFalse(contains);
        }


        [Test]
        public void GetDefaultParameter()
        {
            var tmp = Guid.NewGuid().ToString();
            var r = this.Settings.Get("GetDefaultParameter", tmp);
            Assert.AreEqual(r, tmp);
        }


        [Test]
        public void TryDefaults()
        {
            var flag = this.Settings.SetDefault("TryDefaults", "Initial Value");
            Assert.IsTrue(flag, "Default value could not be set");

            flag = this.Settings.SetDefault("TryDefaults", "Second Value");
            Assert.IsFalse(flag, "Default value was set and should not have been");

            var currentValue = this.Settings.Get<string>("TryDefaults");
            Assert.AreEqual("Initial Value", currentValue);
        }


        [Test]
        public void ClearPreserveList()
        {
            this.Settings.Set("ClearPreserveTest", "Value");
            this.Settings.KeysNotToClear.Add("ClearPreserveTest");
            this.Settings.Clear();
            var value = this.Settings.Get<string>("ClearPreserveTest");
            Assert.AreEqual("Value", value);
        }


#if !WINDOWS_UWP

        [Test]
        public void CultureFormattingTest()
        {
            var value = 11111.1111m;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            this.Settings.Set("CultureFormattingTest", value);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
            var newValue = this.Settings.Get<decimal>("CultureFormattingTest");
            Assert.AreEqual(value, newValue);
        }
#endif


        [Test]
        public void Binding_Basic()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.IgnoredProperty = 0;
            obj.StringProperty = "Hi";

            Assert.IsTrue(this.Settings.Contains("TestBind.StringProperty"));
            Assert.IsFalse(this.Settings.Contains("TestBind.IgnoredProperty"));
            Assert.AreEqual("Hi", this.Settings.Get<string>("TestBind.StringProperty"));
        }


        [Test]
        public void Binding_Persist()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.StringProperty = "Binding_Persist";

            var obj2 = this.Settings.Bind<TestBind>();
            Assert.AreEqual(obj.StringProperty, obj2.StringProperty);
        }
    }
}