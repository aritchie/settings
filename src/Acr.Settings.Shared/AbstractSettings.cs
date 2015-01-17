using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;


namespace Acr.Settings {

    public abstract class AbstractSettings : ISettings {

        protected abstract IDictionary<string, string> GetNativeSettings();
        protected abstract void AddOrUpdateNative(IEnumerable<KeyValuePair<string, string>> saves);
        protected abstract void RemoveNative(IEnumerable<KeyValuePair<string, string>> deletes);
        protected abstract void ClearNative();

        #region Internals

		protected virtual void OnChanged(SettingChangeEventArgs args) {
			if (this.Changed != null)
				this.Changed(this, args);
		}


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
					var adds = e.NewItems.Cast<KeyValuePair<string, string>>();
					this.AddOrUpdateNative(adds);
					foreach (var x in adds)
						this.OnChanged(new SettingChangeEventArgs(SettingChangeAction.Add, x.Key, x.Value));
					break;

                case NotifyCollectionChangedAction.Replace:
                    var updates = e.NewItems.Cast<KeyValuePair<string, string>>();
                    this.AddOrUpdateNative(updates);
					foreach (var x in updates)
						this.OnChanged(new SettingChangeEventArgs(SettingChangeAction.Update, x.Key, x.Value));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var dels = e.OldItems.Cast<KeyValuePair<string, string>>();
                    this.RemoveNative(dels);
					foreach (var x in dels)
						this.OnChanged(new SettingChangeEventArgs(SettingChangeAction.Remove, x.Key, x.Value));
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.ClearNative();
                    break;
            }
        }

        #endregion

        #region ISettings Members

		public event EventHandler<SettingChangeEventArgs> Changed;


        private ISettingsDictionary all;
        public ISettingsDictionary All {
            get {
                if (this.all == null)
                    this.Resync();

                return this.all;
            }
        }


        /// <summary>
        /// This resynchronizes the settings from the native settings dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        public void Resync() {
            if (this.all != null) {
                this.all.CollectionChanged -= this.OnCollectionChanged;
                this.all = null;
            }
            var settings = this.GetNativeSettings();
            this.all = new SettingsDictionary(settings);
            this.all.CollectionChanged += this.OnCollectionChanged;
        }


        public virtual string Get(string key, string defaultValue = null) {
            return (this.All.ContainsKey(key)
                ? this.All[key]
                : defaultValue
            );
        }


        public virtual void Set(string key, string value) {
            this.All[key] = value;
        }


        public virtual void Remove(string key) {
            if (this.All.ContainsKey(key))
                this.All.Remove(key);
        }


        public virtual void Clear() {
            this.All.Clear();
        }


        public virtual bool Contains(string key) {
            return this.All.ContainsKey(key);
        }

        #endregion
    }
}