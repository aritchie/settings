using System;


namespace Acr.Settings {

	public class SettingChangeEventArgs : EventArgs {

		public SettingChangeAction Action { get; private set; }
		public string Key { get; private set; }
		public string Value { get; private set; }


		public SettingChangeEventArgs(SettingChangeAction action, string key, string value) {
			this.Action = action;
			this.Key = key;
			this.Value = value;
		}
	}
}

