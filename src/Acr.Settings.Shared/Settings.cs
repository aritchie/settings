using System;


namespace Acr.Settings {

    public static class Settings {
        private static readonly Lazy<ISettings> instanceInit = new Lazy<ISettings>(() => {
#if __PLATFORM__
            return new SettingsImpl();
#else
            throw new ArgumentException("Platform plugin not found.  Did you reference the nuget package in your platform project?");
#endif
        }, false);


        private static ISettings customInstance;
        public static ISettings Instance {
            get { return customInstance ?? instanceInit.Value; }
            set { customInstance = value; }
        }
    }
}
