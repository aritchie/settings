using System;


namespace Acr.Settings {

    public static class Settings {

#if __PLATFORM__
        private static ISettings instance;


        public static void Init() {
            instance = new SettingsImpl();
        }

#else
        [Obsolete("This is the PCL version.  You should be adding the nuget package to your platform project and calling Init() there.")]
        public static void Init() {
            throw new Exception("This is the PCL version.  You should be adding the nuget package to your platform project and calling Init() there.");
        }
#endif

        public static ISettings Instance { get; set; }
    }
}
