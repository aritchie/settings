using System;


namespace Acr.Settings
{

    public static class Settings
    {
        static ISettings current;
        public static ISettings Current
        {
            get
            {
#if PCL
                if (current == null)
                    throw new ArgumentException("[Acr.Settings] Platform plugin not found.  Did you reference the nuget package in your platform project?");

                return current;
#else
                current = current ?? new SettingsImpl();
                return current;
#endif
            }
            set { current = value; }
        }
    }
}
