using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Forms;


namespace Acr.Settings.Android.Tests
{
    [Activity(
        Label = "Acr.Settings.Android.Tests",
        MainLauncher = true
     )]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //this.AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            this.AddTestAssembly(typeof(BluetoothLE.Tests.DeviceTests).Assembly);
            this.AddTestAssembly(Assembly.GetExecutingAssembly());

            this.AutoStart = false;
            this.TerminateAfterExecution = false;
            //this.Writer =

            base.OnCreate(bundle);
        }
    }
}

