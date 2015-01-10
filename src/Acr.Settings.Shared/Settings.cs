using System;


namespace Acr.Settings {

    public static class Settings {
        private static ISettings instance;
        private static readonly object syncLock = new object();


        public static ISettings Instance {
            get {
                if (instance == null) {
                    lock (syncLock) {
                        if (instance == null) {
#if __PLATFORM__
                            instance = new SettingsImpl();
#else
                            throw new Exception("Platform implementation not found.  Have you added a nuget reference to your platform project?");
#endif
                        }
                    }
                }
                return instance;
            }
            set { instance = value; }
        }


        public static ISettingsDictionary All {
            get { return Instance.All; }
        }
    }
}
