using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace Acr.Settings.Tests
{

    public abstract class AbstractSettingTests : IDisposable
    {
        public ISettings Settings { get; protected set; }


        public void Dispose()
        {
            this.Settings.Clear();
        }


        [Fact]
        public async Task OnSettingChanged()
        {
            var tcs = new TaskCompletionSource<SettingChangeEventArgs>();
            this.Settings.Changed += (sender, args) => tcs.TrySetResult(args);

            this.Settings.Set("OnSettingChanged", "boo");
            var eventArgs = await tcs.Task;

            eventArgs.Action.Should().Be(SettingChangeAction.Add);
            eventArgs.Key.Should().Be("OnSettingChanged");
            eventArgs.Value.Should().Be("boo");
        }


        [Fact]
        public void SetDefault_DoesNotRemove_SimpleType()
        {
            this.Settings.Set("Bool", false);
            this.Settings.Contains("Bool").Should().BeTrue("Bool is missing");

            this.Settings.Set("Long", 0L);
            this.Settings.Contains("Long").Should().BeTrue("Long is missing");
        }


        [Fact]
        public void SetDefault_DoesRemove_ComplexType()
        {
            this.Settings.Set("Object", new object());
            this.Settings.Contains("Object").Should().BeTrue("Object is missing");

            this.Settings.Set<object>("Object", null);
            this.Settings.Contains("Object").Should().BeFalse("Object should be missing");
        }


        [Fact]
        public void Object()
        {
            var inv = new Tuple<int, string>(1, "2");
            this.Settings.Set("Object", inv);

            var outv = this.Settings.Get<Tuple<int, string>>("Object");
            inv.Item1.Should().Be(outv.Item1);
            inv.Item2.Should().Be(outv.Item2);
        }


        [Fact]
        public void IntTest()
        {
            this.Settings.Set("Test", 99);
            var value = this.Settings.Get<int>("Test");
            value.Should().Be(99);
        }


        [Fact]
        public void IntNullTest()
        {
            var nvalue = this.Settings.Get<int?>("Blah");
            nvalue.Should().BeNull("Int? should be null");

            nvalue = 199;
            this.Settings.Set("Blah", nvalue);

            nvalue = this.Settings.Get<int?>("Blah");
            nvalue.Value.Should().Be(199);
        }


        [Fact]
        public void DateTimeNullTest()
        {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            nvalue.Should().BeNull("Should be null");

            this.Settings.Set("DateTimeNullTest", dt);
            nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            nvalue.Should().Be(dt);
        }


        [Fact]
        public void SetOverride()
        {
            this.Settings.Set("Test", "1");
            this.Settings.Set("Test", "2");
            var r = this.Settings.Get<string>("Test");
            r.Should().Be("2");
        }


        [Fact]
        public void ContainsTest()
        {
            this.Settings
                .Contains(Guid.NewGuid().ToString())
                .Should()
                .BeFalse("Contains should have returned false");

            this.Settings.Set("Test", "1");
            this.Settings.Contains("Test").Should().BeTrue("Contains should have returned true");
        }


        [Fact]
        public void RemoveTest()
        {
            this.Settings.Set("Test", "1");
            this.Settings.Remove("Test").Should().BeTrue("Remove should have returned success");
        }


        [Fact]
        public void LongTest()
        {
            long value = 1;
            this.Settings.Set("LongTest", value);
            var value2 = this.Settings.Get<long>("LongTest");
            value.Should().Be(value2);
        }


        [Fact]
        public void GuidTest()
        {
            this.Settings.Get<Guid>("GuidTest").Should().Be(Guid.Empty);

            var guid = new Guid();
            this.Settings.Set("GuidTest", guid);
            this.Settings.Get<Guid>("GuidTest").Should().Be(guid);
        }


        [Fact]
        public void SetNullRemoves()
        {
            this.Settings.Set("SetNullRemoves", "Blah");
            this.Settings.Set<string>("SetNullRemoves", null);
            this.Settings.Contains("SetNullRemoves").Should().BeFalse();
        }


        [Fact]
        public void GetDefaultParameter()
        {
            var tmp = Guid.NewGuid().ToString();
            this.Settings.Get("GetDefaultParameter", tmp).Should().Be(tmp);
        }


        [Fact]
        public void TryDefaults()
        {
            this.Settings
                .SetDefault("TryDefaults", "Initial Value")
                .Should().BeTrue("Default value could not be set");

            this.Settings
                .SetDefault("TryDefaults", "Second Value")
                .Should().BeFalse("Default value was set and should not have been");

            this.Settings
                .Get<string>("TryDefaults")
                .Should().Be("Initial Value");
        }


        [Fact]
        public void ClearPreserveList()
        {
            this.Settings.Set("ClearPreserveTest", "Value");
            this.Settings.KeysNotToClear.Add("ClearPreserveTest");
            this.Settings.Clear();
            this.Settings.Get<string>("ClearPreserveTest").Should().Be("Value");
        }


#if !WINDOWS_UWP

        [Fact]
        public void CultureFormattingTest()
        {
            var value = 11111.1111m;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            this.Settings.Set("CultureFormattingTest", value);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
            var newValue = this.Settings.Get<decimal>("CultureFormattingTest");
            newValue.Should().Be(value);
        }
#endif


        [Fact]
        public void Binding_Basic()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.IgnoredProperty = 0;
            obj.StringProperty = "Hi";

            this.Settings.Contains("TestBind.StringProperty").Should().BeTrue();
            this.Settings.Contains("TestBind.IgnoredProperty").Should().BeFalse();
            this.Settings.Get<string>("TestBind.StringProperty").Should().Be("Hi");
        }


        [Fact]
        public void Binding_Persist()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.StringProperty = "Binding_Persist";

            var obj2 = this.Settings.Bind<TestBind>();
            obj.StringProperty.Should().Be(obj2.StringProperty);
        }
    }
}