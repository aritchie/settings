using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Acr.Settings {

    public class SettingsViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        readonly ISettings settings;


        public SettingsViewModel(ISettings settings) {
            this.settings = settings;
            this.settings.Changed += (sender, args) => this.OnPropertyChanged(args.Key);
        }


        protected virtual bool SetProperty<T>(T value, [CallerMemberName] string propertyName = null) {
            var current = this.settings.Get<T>(propertyName);
            if (Object.Equals(current, value))
                return false;

            this.settings.Set(propertyName, value);
            this.OnPropertyChanged(propertyName);
            return true;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
