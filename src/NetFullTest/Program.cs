using System;
using Acr.Settings;


namespace NetFullTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hello("AppSettings", new AppConfigSettingsImpl("NetFullTest.exe.config"));
            Hello("AppSettings", new AppConfigSettingsImpl());
            Hello("Registry", new RegistrySettingsImpl("Acr.Settings", true));
            Console.ReadLine();
        }


        static void Hello(string providerName, ISettings provider)
        {
            var value = provider.Get("Hello", 0);
            Console.WriteLine($"Hello from {providerName} - value: {value}");
            value++;
            provider.Set("Hello", value);
        }
    }
}
