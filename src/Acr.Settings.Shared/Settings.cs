using System;


namespace Acr.Settings {

    public static class Settings {
        private static ISettings instance;


        public static void Init() {
            #if __PLATFORM__
            instance = new SettingsImpl();
            #else
            throw new Exception("Platform implementation not found.  Have you added a nuget reference to your platform project?");
            #endif
        }

        public static ISettings Instance { get; set; }
    }
}
