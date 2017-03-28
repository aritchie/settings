using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;


namespace Acr.Settings {

    public class DatabaseSettingsImpl : AbstractSettings {
        readonly DbProviderFactory factory;
        readonly Dictionary<string, string> items;
        readonly string connectionString;


        public DatabaseSettingsImpl(string connectionStringName, string appName, string environment = null, string tableName = "Settings") {
            this.ApplicationName = appName;
            this.Environment = environment ?? System.Environment.MachineName;
            this.TableName = tableName;
            this.ConnectionStringName = connectionStringName;
            this.items = new Dictionary<string, string>();

            var cn = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cn == null)
                throw new ArgumentException($"Invalid connection name {connectionStringName}");

            if (String.IsNullOrWhiteSpace(cn.ProviderName))
                throw new ArgumentException("No provider configured for connection string");

            this.factory = DbProviderFactories.GetFactory(cn.ProviderName);
            this.connectionString = cn.ConnectionString;
        }


        public string ApplicationName { get; }
        public string Environment { get; }
        public string TableName { get; }
        public string ConnectionStringName { get; }


        public override bool Contains(string key) {
            this.Init();
            lock (this.items)
                return this.items.ContainsKey(key);
        }


        protected override object NativeGet(Type type, string key) {
            this.Init();
            lock (this.items) {
                var value = this.items[key];
                var obj = this.Deserialize(type, value);
                return obj;
            }
        }


        protected override void NativeSet(Type type, string key, object value) {
            this.Init();
            lock (this.items) {
                var item = this.Serialize(type, value);
                var sql = this.items.ContainsKey(key)
                    ? $"INSERT INTO {this.TableName}(ApplicationName, Environment, SettingKey, SettingValue) VALUES ('{this.ApplicationName}', '{this.Environment}', '{key}', '{item}')"
                    : $"UPDATE {this.TableName} SET SettingValue = '{item}' WHERE Environment = '{this.Environment}' AND ApplicationName = '{this.ApplicationName}' AND SettingKey = '{key}'";

                this.Execute(sql);
                this.items[key] = item;
            }
        }


        protected override void NativeRemove(string[] keys) {
            this.Init();
            lock (this.items) {
                // TODO: remove singular deletes
                foreach (var key in keys) {
                    this.Execute($"DELETE FROM {this.TableName} WHERE Environment = '{this.Environment}' AND ApplicationName = '{this.ApplicationName}' AND SettingKey = '{key}'");
                    this.items.Remove(key);
                }
            }
        }


        protected override IDictionary<string, string> NativeValues() {
            this.Init();
            lock (this.items)
                return new Dictionary<string, string>(this.items);
        }


        protected virtual void Execute(string sql) {
            using (var conn = this.factory.CreateConnection()) {
                conn.ConnectionString = this.connectionString;
                conn.Open();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }


        bool init;
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual void Init() {
            if (this.init)
                return;

            // try table create?
            this.Resync();
            this.init = true;
        }


        public virtual void Resync() {
            lock (this.items) {
                this.items.Clear();

                using (var conn = this.factory.CreateConnection()) {
                    conn.ConnectionString = this.connectionString;
                    using (var cmd = conn.CreateCommand()) {
                        cmd.CommandText = $"SELECT SettingKey, SettingValue FROM {this.TableName} WHERE Environment = '{this.Environment}' AND ApplicationName = '{this.ApplicationName}'";
                        conn.Open();

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                            while (reader.Read()) {
                                var key = reader.GetString(0);
                                var value = reader.GetString(1);
                                this.items.Add(key, value);
                            }
                        }
                    }
                }
            }
        }


        // TODO: auto resync timer

        public virtual void DropTable() {
            this.Execute($"DROP TABLE {this.TableName}");
        }


        public virtual void CreateTable() {
            this.Execute($@"
CREATE TABLE {this.TableName}(
    ApplicationName nvarchar(50) NOT NULL,
    Environment nvarchar(50) NOT NULL,
    SettingKey nvarchar(50) NOT NULL,
    SettingValue nvarchar(2000) NOT NULL
)");
        }
    }
}
