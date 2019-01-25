using Microsoft.VisualStudio.TestTools.UnitTesting;
using kvstore;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void TestPutAndGet() {
            IDatabase db = new Database();
            db.Put("testkey", "testvalue");
            Assert.AreEqual("testvalue", db.Get("testkey"));
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public void TestGetBadKey()
        {
            IDatabase db = new Database();
            db.Get("badkey");
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public void TestPutGetDeleteGet()
        {
            IDatabase db = new Database();
            db.Put("testkey", "testvalue");
            Assert.AreEqual("testvalue", db.Get("testkey"));
            db.Delete("testkey");
            db.Get("testkey");
        }

        [TestMethod]
        public void TestPutPutGet()
        {
            IDatabase db = new Database();
            db.Put("key", "value1");
            db.Put("key", "value2");
            Assert.AreEqual("value2", db.Get("key"));
        }

        [TestMethod]
        public void TestPutRefAndGetRef()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db.PutRef("refkey", "key");
            Assert.AreEqual("value", db.GetRef("refkey"));
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public void TestBadPutRef()
        {
            IDatabase db = new Database();
            db.PutRef("refkey", "badref");
            db.GetRef("refkey");
        }

        [TestMethod]
        public void TestBeginAndCommit()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db = db.Begin();
            db.Put("key", "value2");
            db = db.Commit();
            Assert.AreEqual("value2", db.Get("key"));
        }

        [TestMethod]
        public void TestBeginAndCommitRefs()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db = db.Begin();
            db.PutRef("key", "key");
            db = db.Commit();
            Assert.AreEqual("value", db.GetRef("key"));
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public void TestBeginAndRollbackRefs()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db = db.Begin();
            db.PutRef("key", "key");
            db = db.Rollback();
            db.GetRef("key");
        }

        [TestMethod]
        public void TestBeginAndGet()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db = db.Begin();
            Assert.AreEqual("value", db.Get("key"));
        }

        [TestMethod]
        public void TestBeginAndRollback()
        {
            IDatabase db = new Database();
            db.Put("key", "value");
            db = db.Begin();
            db.Put("key", "value2");
            db = db.Rollback();
            Assert.AreEqual("value", db.Get("key"));
        }
    }
}
