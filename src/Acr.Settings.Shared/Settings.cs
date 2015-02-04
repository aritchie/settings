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


        public static event EventHandler<SettingChangeEventArgs> Changed {
            add { Instance.Changed += value; }
            remove { Instance.Changed -= value; }
        }


        public static T Get<T>(string key, T defaultValue = default(T)) {
            return Instance.Get(key, defaultValue);
        }


        public static void Set<T>(string key, T value) {
            Instance.Set(key, value);
        }


        public static void Remove(string key) {
            Instance.Remove(key);
        }


        public static bool Contains(string key) {
            return Instance.Contains(key);
        }


        public static void Clear() {
            Instance.Clear();
        }
    }
}
