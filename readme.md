#ACR Xplat Settings Plugin for Xamarin and Windows
To use, simply call:

    var guid = Settings.Get<Guid>("Key");

    Settings.Set("Key", AnyObject); // converts to JSON
    var obj = Settings.Get<AnyObject>("Key");

To supply your own implementation:

    Settings.Instance = new YourImplementationInheritingISettings();


Monitor setting changes:

    Settings.Changed += (sender, args) => {
        Console.WriteLine(args.Action);        
        Console.WriteLine(args.Key);
        Console.WriteLine(args.Value);
    };

