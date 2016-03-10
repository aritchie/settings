using System;


namespace Acr.Settings.Fody
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        //public object DefaultValue { get; set; }
        //public bool IsSecure { get; set; }
    }
}
