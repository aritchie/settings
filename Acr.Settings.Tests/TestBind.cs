using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Acr.Settings.Tests
{
    public class TestBind : INotifyPropertyChanged
    {
        string stringProperty;
        public string StringProperty
        {
            get => this.stringProperty;
            set
            {
                this.stringProperty = value;
                this.OnPropertyChanged();
            }
        }


        Guid? nullProperty;
        public Guid? NullableProperty
        {
            get => this.nullProperty;
            set
            {
                this.nullProperty = value;
                this.OnPropertyChanged();
            }
        }


        int? ignoredProperty;
        [Ignore]
        public int? IgnoredProperty
        {
            get => this.ignoredProperty;
            set
            {
                this.ignoredProperty = value;
                this.OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
