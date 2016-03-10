#ACR Settings Plugin for Xamarin and Windows
A cross platform settings plugin for Xamarin and Windows.  Unlike other setting libraries in the wild, this library provides several unique features

* You can store/retrieve just about any type of object (thanks to Newtonsoft.Json)
* You can monitor for changes using the Changed event
* iCloud Settings Provider
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

##Strongly Typed Binding (works with all platforms - no fancy reflection that breaks on iOS)

    var myInpcObj = Settings.Local.Bind<MyInpcObject>(); // Your object must implement INotifyPropertyChanged
    myInpcObj.SomeProperty = "Hi"; // everything is automatically synchronized to settings right here


##Roaming Settings
Used for things like iOS app groups and android wear.  It works the same way as Local (substitute with Roaming), but requires initialization.
The namespace setting only applies to iOS and Android.

*Android*

        Launch Activity - Settings.InitRoaming("your app namespace");

*iOS*

        AppDelegate - Settings.InitRoaming("your app namespace");

*Windows*

        Settings.InitRoaming();

*iOS iCloud*

        AppDelegate - Settings.Roaming = new Acr.Settings.iCloudSettingsImpl();

##To supply your own implementation:

    Settings.Local = new YourImplementationInheritingISettings();


##Monitor setting changes:

    Settings.Local.Changed += (sender, args) => {
        Console.WriteLine(args.Action);
        Console.WriteLine(args.Key);
        Console.WriteLine(args.Value);
    };

###Dependency Injection:

*Autofac*

        containerBuilder.Register(x => Settings.Local).As<ISettings>().SingleInstance();

*MvvmCross (manual - can use bootstrap below)*

        Mvx.RegisterSingleton(Settings.Local);


####For MvvmCross:
There is a platform shim for MvvmCross (Acr.MvvmCross.Plugins.Settings).  Roaming is not currently supported in the shim.

or it can be setup manually.  In each platfor

    Mvx.RegisterSingleton(Acr.Settings.Settings.Local);


