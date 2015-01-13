using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;


namespace Acr.Settings.Windows {

    public class SettingsImpl : AbstractSettings {

        protected override IDictionary<string, string> GetNativeSettings() {
            return ApplicationData
                .Current
                .LocalSettings
                .Values
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
        }


        protected override void AddOrUpdateNative(IEnumerable<KeyValuePair<string, string>> saves) {
            var list = ApplicationData.Current.LocalSettings.Values;
            foreach (var save in saves)
                list[save.Key] = save.Value;
        }


        protected override void RemoveNative(IEnumerable<KeyValuePair<string, string>> deletes) {
            var list = ApplicationData.Current.LocalSettings.Values;
            foreach (var del in deletes)
                if (list.ContainsKey(del.Key))
                    list.Remove(del.Key);
        }


        protected override void ClearNative() {
            ApplicationData.Current.LocalSettings.Values.Clear();
        }
    }
}
