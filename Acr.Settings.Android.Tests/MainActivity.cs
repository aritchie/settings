using System;
using Acr.Settings.Tests;
using Android.App;
using Android.OS;
using Xamarin.Forms;
using Xunit.Runners.UI;


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
            this.AddTestAssembly(typeof(DefaultSettingTests).Assembly);

            this.AutoStart = false;
            this.TerminateAfterExecution = false;
            //this.Writer =

            base.OnCreate(bundle);
        }
    }
}

