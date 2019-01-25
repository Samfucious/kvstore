using System;
using System.Collections.Generic;

namespace kvstore
{
    public interface IDatabase
    {
        void Put(string key, string val);
        void Delete(string key);
        void PutRef(string key, string val);

        string Get(string key);
        string GetRef(string key);

        IDatabase Begin();
        IDatabase Commit();
        IDatabase Rollback();
    }

    public class DatabaseException : ApplicationException 
    {
        public DatabaseException() : base() { }
        public DatabaseException(String message) : base(message) { }
        public DatabaseException(String message, Exception innerException) : base(message, innerException) { }
    }

    public class Database : IDatabase
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private Dictionary<string, string> refs = new Dictionary<string, string>();
        private Database next;

        public void Put(string key, string val)
        {
            this.values[key] = val;
        }

        public void Delete(string key)
        {
            try
            {
                this.values.Remove(key);
            }
            catch (KeyNotFoundException e)
            {
                throw new DatabaseException("ERROR - Key not found", e);
            }
        }

        public void PutRef(string key, string val)
        {
            this.refs[key] = val;
        }

        public string Get(string key)
        {
            if (this.values.ContainsKey(key))
            {
                return this.values[key];
            }
            if (this.next != null)
            {
                return this.next.Get(key);
            }
            throw new DatabaseException("ERROR - Key not found");
        }

        public string GetRef(string key)
        {
            if (this.refs.ContainsKey(key))
            {
                return this.Get(this.refs[key]);
            }
            if (this.next != null)
            {
                return this.Get(this.next.GetRef(key));
            }
            throw new DatabaseException("ERROR - Key not found");
        }

        public IDatabase Begin()
        {
            Database newTop = new Database();
            newTop.next = this;
            return newTop;
        }

        public IDatabase Commit()
        {
            if (this.next == null)
            {
                return this;
            }

            Merge(this.values, this.next.values);
            Merge(this.refs, this.next.refs);
            return this.next;
        }

        private void Merge(Dictionary<string, string> source, Dictionary<string, string> destination)
        {
            foreach (String key in source.Keys)
            {
                destination[key] = source[key];
            }
        }

        public IDatabase Rollback()
        {
            if (this.next == null)
            {
                return this;
            }
            return this.next;
        }
    }
}
