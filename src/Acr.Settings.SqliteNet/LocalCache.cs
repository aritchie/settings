using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Settings
{
    public class LocalCache
    {
        IDictionary<string, CacheItem> values;
        readonly ConfigSqliteConnection conn;


        public LocalCache(ConfigSqliteConnection conn)
        {
            this.conn = conn;
            this.values = new Dictionary<string, CacheItem>();
        }


        public void Init()
        {
            this.conn.Init();
            this.values = this.conn
                .CacheItems
                .ToList()
                .ToDictionary(
                    x => x.Key,
                    x => x
                );
        }

        public object Get(string key)
        {
            using (this.conn.Lock())
            {
                if (this.values.ContainsKey(key))
                    return this.values[key].GetValue();

                return null;
            }
        }


        public void Set(string key, object value)
        {
            using (this.conn.Lock())
            {
                if (this.values.ContainsKey(key))
                {
                    var item = this.values[key];
                    item.SetValue(value);
                    this.conn.Update(item);
                }
                else
                {
                    var item = new CacheItem { Key = key };
                    item.SetValue(value);
                    this.values.Add(key, item);
                    this.conn.Insert(item);
                }
            }
        }


        public bool Remove(string key)
        {
            using (this.conn.Lock())
            {
                if (!this.values.ContainsKey(key))
                    return false;

                var item = this.values[key];
                this.conn.Delete(item);
                return true;
            }
        }


        public void Clear()
        {
            using (this.conn.Lock())
            {
                try
                {
                    this.conn.BeginTransaction();

                    foreach (var item in this.values.Values)
                        this.conn.Delete(item);

                    this.conn.Commit();
                    this.values.Clear();
                }
                finally
                {
                    this.conn.Rollback();
                }
            }
        }


        public bool Contains(string key)
        {
            using (this.conn.Lock())
                return this.values.ContainsKey(key);
        }
    }
}