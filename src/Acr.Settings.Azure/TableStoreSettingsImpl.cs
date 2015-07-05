using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace Acr.Settings.Azure {

    public class TableStoreSettingsImpl : AbstractSettings {
        private readonly CloudTableClient tableClient;
        private readonly CloudTable table;


        public TableStoreSettingsImpl(string connectionString, string tableName) {
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            this.tableClient = storageAccount.CreateCloudTableClient();
            this.table = this.tableClient.GetTableReference("");
            this.table.CreateIfNotExists();
        }

        public override bool Contains(string key) {
            throw new NotImplementedException();
        }


        protected override void NativeClear() {
            throw new NotImplementedException();
        }


        protected override object NativeGet(Type type, string key) {
            throw new NotImplementedException();
        }


        protected override void NativeSet(Type type, string key, object value) {
            throw new NotImplementedException();
        }


        protected override void NativeRemove(string key) {
            throw new NotImplementedException();
        }


        protected override IDictionary<string, string> NativeValues() {
            throw new NotImplementedException();
        }
    }
}
