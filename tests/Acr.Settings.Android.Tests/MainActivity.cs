using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;

namespace Acr.Settings.Android.Tests {

    [Activity(Label = "Acr.Settings.Android.Tests", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : TestSuiteActivity {
        protected override void OnCreate(Bundle bundle) {
            this.AddTest(Assembly.GetExecutingAssembly());
            base.OnCreate(bundle);
        }
    }
}

