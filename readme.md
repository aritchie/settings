TODO

iOS/WinPhone/Win8: Settings.Init();
Android: Settings.Init(Activity);

MvvmCross in each platform setup:
Init just like above
Mvx.LazyConstructAndRegisterSingleton<ISettings, SettingsImpl>();
Plugin version coming soon

Any platform:
Settings.Get(key, defaultValue);