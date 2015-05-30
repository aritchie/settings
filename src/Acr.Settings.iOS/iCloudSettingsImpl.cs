using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;


namespace Acr.Settings {

    public class iCloudSettingsImpl : AbstractSettings {

        //public iCloudSettingsImpl() {
            //NSNotificationCenter.DefaultCenter.AddObserver(
            //    NSUbiquitousKeyValueStore.DidChangeExternallyNotification,
            //    x => {
            //        //var reasonNumber = (NSNumber)x.UserInfo.ObjectForKey(NSUbiquitousKeyValueStore.ChangeReasonKey); .IntValue
            //        var array = (NSArray)x.UserInfo.ObjectForKey(NSUbiquitousKeyValueStore.ChangedKeysKey);
            ////    var changedKeysList = new List<string> ();
            ////    for (uint i = 0; i < changedKeys.Count; i++) {
            ////        var key = new NSString (changedKeys.ValueAt(i)); // resolve key to a string
            ////        changedKeysList.Add (key);
            ////    }
            //    }
            //);
        //}


        public override bool Contains(string key) {
            return (NSUbiquitousKeyValueStore.DefaultStore.ValueForKey(new NSString(key)) != null);
        }


        protected override void NativeClear() {
            var prefs = NSUbiquitousKeyValueStore.DefaultStore;
            var values = this.NativeValues();
            foreach (var item in values)
                if (this.CanTouch(item.Key))
                    prefs.RemoveObject(item.Key);

            prefs.Synchronize();
        }


        protected override string NativeGet(string key) {
            return NSUbiquitousKeyValueStore.DefaultStore.GetString(key);
        }


        protected override void NativeRemove(string key) {
            NSUbiquitousKeyValueStore.DefaultStore.Remove(key);
            NSUbiquitousKeyValueStore.DefaultStore.Synchronize();
        }


        protected override void NativeSet(string key, string value) {
            NSUbiquitousKeyValueStore.DefaultStore.SetString(key, value);
            NSUbiquitousKeyValueStore.DefaultStore.Synchronize();
        }


        protected override IDictionary<string, string> NativeValues() {
            return NSUbiquitousKeyValueStore
                .DefaultStore
                .ToDictionary()
                .ToDictionary(
                    x => x.Key.ToString(),
                    x => x.Value.ToString()
                );
        }
    }
}