using System;


namespace Acr.Settings {

    public static class Settings {
        private static readonly Lazy<ISettings> localInit = new Lazy<ISettings>(() => CreateInstance(null), false);

#if __ANDROID__ || __IOS__
        public static void InitRoaming(string nameSpace) {
            Roaming = CreateInstance(nameSpace);
        }
#elif __WINDOWS__
        public static void InitRoaming() {
            Roaming = CreateInstance("Windows");
        }
#endif

        private static ISettings roaming;
        public static ISettings Roaming {
            get {
                if (roaming == null)
                    throw new ArgumentException("You must call InitRoaming");

                return roaming;
            }
            set { roaming = value; }
        }



        private static ISettings local;
        public static ISettings Local {
            get { return local ?? localInit.Value; }
            set { local = value; }
        }



        public static ISettings CreateInstance(string nameSpace = null) {
#if __ANDROID__ || __IOS__
            return new SettingsImpl(nameSpace);
#elif NET_CORE
            return new AppConfigSettingsImpl();
#elif __WINDOWS__
            return new SettingsImpl(nameSpace != null);
#else
            throw new ArgumentException("Platform plugin not found.  Did you reference the nuget package in your platform project?");
#endif
        }
    }
}
