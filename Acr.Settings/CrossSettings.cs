using System;


namespace Acr.Settings
{

    public static partial class CrossSettings
    {
        static ISettings current;
        public static ISettings Current
        {
            get
            {
                if (current == null)
                    throw new ArgumentException("[Acr.Settings] Platform plugin not found.  Did you reference the nuget package in your platform project?");

                return current;
            }
            set => current = value;
        }
    }
}
