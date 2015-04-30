using System;
using Foundation;
using UIKit;
using MonoTouch.NUnit.UI;


namespace Acr.Settings.iOS.Tests {

    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate {
        UIWindow window;
        TouchRunner runner;


        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);
            runner = new TouchRunner(window);

            // register every tests included in the main application/assembly
            runner.Add(System.Reflection.Assembly.GetExecutingAssembly());

            window.RootViewController = new UINavigationController(runner.GetViewController());

            // make the window visible
            window.MakeKeyAndVisible();

            return true;
        }
    }
}