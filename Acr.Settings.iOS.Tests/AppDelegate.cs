using System.Reflection;
using Acr.Settings.Tests;
using Foundation;
using UIKit;
using Xunit.Runner;
using Xunit.Sdk;


namespace Acr.Settings.iOS.Tests
{
    [Register("AppDelegate")]
    public class AppDelegate : RunnerAppDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // We need this to ensure the execution assembly is part of the app bundle
            this.AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            //this.AddTestAssembly(Assembly.GetExecutingAssembly());
            this.AddTestAssembly(typeof(DefaultSettingTests).Assembly);

            this.AutoStart = false;
            this.TerminateAfterExecution = false;

            //AddTestAssembly(typeof(OBJECT).Assembly);
            return base.FinishedLaunching(application, launchOptions);
        }
    }
}


