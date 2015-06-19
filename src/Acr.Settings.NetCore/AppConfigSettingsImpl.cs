using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace Acr.Settings.NetCore {

    public class AppConfigSettingsImpl : AbstractSettings {
        private readonly Configuration config;


        public AppConfigSettingsImpl(string fileName) {
            this.ConfigurationFileName = fileName;

            if (this.ConfigurationFileName == null)
                this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            else {
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = this.ConfigurationFileName };
                this.config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }
            if (this.config == null)
                throw new ApplicationException("Could not open configuration file");
        }


        public string ConfigurationFileName { get; set; }


        public override bool Contains(string key) {
            return this.config.AppSettings.Settings[key] != null;
        }


        protected override void NativeClear() {
            this.config.AppSettings.Settings.Clear();
            this.Flush();
        }


        protected override object NativeGet(Type type, string key) {
            var el = this.config.AppSettings.Settings[key];
            return el == null
                ? null
                : el.Value;
        }


        protected override void NativeSet(Type type, string key, object value) {
            var @string = this.Serialize(type, value);
            var set = this.config.AppSettings.Settings;
            var el = set[key];
            if (el == null)
                set.Add(key, @string);
            else
                el.Value = @string;

            this.Flush();
        }


        protected override void NativeRemove(string key) {
            this.config.AppSettings.Settings.Remove(key);
            this.Flush();
        }


        protected override IDictionary<string, string> NativeValues() {
            return this.config
                .AppSettings
                .Settings
                .AllKeys
                .ToDictionary(
                    x => x,
                    x => this.config.AppSettings.Settings[x].Value
                );
        }


        protected virtual void Flush() {
            this.config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(this.config.AppSettings.SectionInformation.Name);
        }
    }
}
