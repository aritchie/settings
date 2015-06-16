using System;


namespace Acr.Settings {

	public class SettingChangeEventArgs : EventArgs {

		public SettingChangeAction Action { get; private set; }
		public string Key { get; private set; }
		public object Value { get; private set; }


		public SettingChangeEventArgs(SettingChangeAction action, string key, object value) {
			this.Action = action;
			this.Key = key;
			this.Value = value;
		}
	}
}

