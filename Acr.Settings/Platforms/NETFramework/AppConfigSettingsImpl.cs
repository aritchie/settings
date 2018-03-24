using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace Acr.Settings
{

    public class AppConfigSettingsImpl : AbstractSettings
    {
        readonly Configuration config;

        public AppConfigSettingsImpl() : this(null, ConfigurationUserLevel.None) {}
        public AppConfigSettingsImpl(string fileName) : this(fileName, ConfigurationUserLevel.None) {}
        public AppConfigSettingsImpl(string fileName, ConfigurationUserLevel configLevel)
        {
            this.ConfigurationFileName = fileName;

            if (this.ConfigurationFileName == null)
            {
                this.config = ConfigurationManager.OpenExeConfiguration(configLevel);
            }
            else
            {
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = this.ConfigurationFileName };
                this.config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, configLevel);
            }
            if (this.config == null)
                throw new ApplicationException("Could not open configuration file");
        }


        public string ConfigurationFileName { get; set; }


        public override bool Contains(string key)
        {
            return this.config.AppSettings.Settings[key] != null;
        }


        protected override object NativeGet(Type type, string key)
        {
            var el = this.config.AppSettings.Settings[key];
            var result = this.Deserialize(type, el?.Value);
            return result;
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            var @string = this.Serialize(type, value);
            var set = this.config.AppSettings.Settings;
            var el = set[key];
            if (el == null)
                set.Add(key, @string);
            else
                el.Value = @string;

            this.Flush();
        }


        protected override void NativeRemove(string[] keys)
        {
            foreach (var key in keys)
                this.config.AppSettings.Settings.Remove(key);

            this.Flush();
        }


        protected override IDictionary<string, string> NativeValues()
        {
            return this.config
                .AppSettings
                .Settings
                .AllKeys
                .ToDictionary(
                    x => x,
                    x => this.config.AppSettings.Settings[x].Value
                );
        }


        protected virtual void Flush()
        {
            this.config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(this.config.AppSettings.SectionInformation.Name);
        }
    }
}
