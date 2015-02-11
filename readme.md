#ACR Xplat Settings Plugin for Xamarin and Windows
A cross platform settings plugin for Xamarin iOS/Android and Windows Phone 8.  Unlike other libraries in this category though, you can serialize
just about anything.  It also has a change event to monitor for changes from other places in your app.

##To use, simply call:

    var int1 = Settings.Instance.Get<int>("Key");
    var int2 = Settings.Instance.Get<int?>("Key");

    Settings.Instance.Set("Key", AnyObject); // converts to JSON
    var obj = Settings.Instance.Get<AnyObject>("Key");

##To supply your own implementation:

    Settings.Instance = new YourImplementationInheritingISettings();


##Monitor setting changes:

    Settings.Changed += (sender, args) => {
        Console.WriteLine(args.Action);
        Console.WriteLine(args.Key);
        Console.WriteLine(args.Value);
    };

###In your platform project (NOT YOUR PCL)
####For DI:

    container.RegisterSingleton<ISettings, SettingsImpl>();

####For MvvmCross:

    Mvx.RegisterSingleton<ISettings, SettingsImpl>();


