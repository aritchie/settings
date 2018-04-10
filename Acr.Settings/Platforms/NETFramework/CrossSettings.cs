using System;


namespace Acr.Settings
{
    public static partial class CrossSettings
    {
        static CrossSettings()
        {
            Current = new AppConfigSettingsImpl();
        }
    }
}
