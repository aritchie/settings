using System;
using SQLite;


namespace Acr.Settings
{
    public class ConfigSqliteConnection : SQLiteConnectionWithLock
    {
        public ConfigSqliteConnection(string databaseName)
            : base(
                new SQLiteConnectionString(databaseName, true),
                SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite
            )
        {
        }


        public void Init() => this.CreateTable<CacheItem>();
        public TableQuery<CacheItem> CacheItems => this.Table<CacheItem>();
    }
}