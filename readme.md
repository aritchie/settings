#ACR Xplat Settings Plugin for Xamarin and Windows
A cross platform settings plugin for Xamarin and Windows.  Unlike other setting libraries in the wild, this provides 3 unique features

* You can store/retrieve just about any type of object (thanks to Newtonsoft.Json)
* You can monitor for changes using the Changed event
* You can use roaming profiles which is useful for:
    * iOS app groups
    * iOS extensions
    * iWatch
    * Android Wear


##To use, simply call:

    var int1 = Settings.Local.Get<int>("Key");
    var int2 = Settings.Local.Get<int?>("Key");

    Settings.Local.Set("Key", AnyObject); // converts to JSON
    var obj = Settings.Local.Get<AnyObject>("Key");

##Roaming Settings
Used for things like iOS app groups and android wear.  It works the same way as Local (substitute with Roaming), but requires initialization.
The namespace setting only applies to iOS and Android.

Android/iOS
    Settings.InitRoaming("your app namespace");

Windows
    Settings.InitRoaming();

##To supply your own implementation:

    Settings.Local = new YourImplementationInheritingISettings();    


##Monitor setting changes:

    Settings.Local.Changed += (sender, args) => {
        Console.WriteLine(args.Action);
        Console.WriteLine(args.Key);
        Console.WriteLine(args.Value);
    };

###Dependency Injection:
ie. autofac
    containerBuilder.Register(x => Settings.Local).As<ISettings>().SingleInstance();

####For MvvmCross:
There is a platform shim for MvvmCross (Acr.MvvmCross.Plugins.Settings).  Roaming is not currently supported in the shim.

or it can be setup manually.  In each platfor

    Mvx.RegisterSingleton(Acr.Settings.Settings.Local);


