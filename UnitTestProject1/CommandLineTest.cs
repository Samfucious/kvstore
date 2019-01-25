using kvstore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class CommandLineTest
    {
        [TestMethod]
        public void TestPut()
        {
            TestCommandProvider commandProvier = NewCommandProvider("put blue green");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.PutCalls);
        }

        [TestMethod]
        public void TestGet()
        {
            TestCommandProvider commandProvier = NewCommandProvider("get blue");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            commandProvier.DequeueMessage();
            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.GetCalls);
        }

        [TestMethod]
        public void TestDelete()
        {
            TestCommandProvider commandProvier = NewCommandProvider("delete blue");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.DeleteCalls);
        }

        [TestMethod]
        public void TestPutRef()
        {
            TestCommandProvider commandProvier = NewCommandProvider("putref blue green");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.PutRefCalls);
        }

        [TestMethod]
        public void TestGetRef()
        {
            TestCommandProvider commandProvier = NewCommandProvider("getref blue");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            commandProvier.DequeueMessage();
            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.GetRefCalls);
        }

        [TestMethod]
        public void TestBegin()
        {
            TestCommandProvider commandProvier = NewCommandProvider("begin");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.BeginCalls);
        }

        [TestMethod]
        public void TestCommit()
        {
            TestCommandProvider commandProvier = NewCommandProvider("commit");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.CommitCalls);
        }

        [TestMethod]
        public void TestRollback()
        {
            TestCommandProvider commandProvier = NewCommandProvider("rollback");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("bye", commandProvier.DequeueMessage());
            Assert.AreEqual(1, db.RollbackCalls);
        }

        [TestMethod]
        public void TestTooFewArgs()
        {
            TestCommandProvider commandProvier = NewCommandProvider("put");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("Not enough arguments for command put.", commandProvier.DequeueMessage());
            Assert.AreEqual("bye", commandProvier.DequeueMessage());
        }

        [TestMethod]
        public void TestUnknownCommand()
        {
            TestCommandProvider commandProvier = NewCommandProvider("fire all missiles");
            TestDatabase db = new TestDatabase();
            CommandLine commandLine = new CommandLine(db, commandProvier);
            commandLine.Run();

            Assert.AreEqual("Unknown command.", commandProvier.DequeueMessage());
            Assert.AreEqual("bye", commandProvier.DequeueMessage());
        }

        private TestCommandProvider NewCommandProvider(string command)
        {
            Queue<string> commandQueue = new Queue<string>();
            commandQueue.Enqueue(command);
            commandQueue.Enqueue("quit");
            return new TestCommandProvider(commandQueue);
        }
    }

    class TestDatabase : IDatabase
    {
        public int PutCalls;
        public int GetCalls;
        public int DeleteCalls;

        public int PutRefCalls;
        public int GetRefCalls;

        public int BeginCalls;
        public int CommitCalls;
        public int RollbackCalls;

        public void Delete(string key)
        {
            DeleteCalls++;
        }

        public string Get(string key)
        {
            GetCalls++;
            return "";
        }

        public string GetRef(string key)
        {
            GetRefCalls++;
            return "";
        }

        public void Put(string key, string val)
        {
            PutCalls++;
        }

        public void PutRef(string key, string val)
        {
            PutRefCalls++;
        }

        public IDatabase Begin()
        {
            BeginCalls++;
            return this;
        }

        public IDatabase Commit()
        {
            CommitCalls++;
            return this;
        }

        public IDatabase Rollback()
        {
            RollbackCalls++;
            return this;
        }
    }

    class TestCommandProvider : ICommandProvider
    {
        Queue<string> commands;
        Queue<string> messages = new Queue<string>();

        public string DequeueMessage()
        {
            return messages.Dequeue();
        }

        public TestCommandProvider(Queue<string> commands)
        {
            this.commands = commands;
        }

        public string Next()
        {
            return commands.Dequeue();
        }

        public void Write(string message)
        {
            messages.Enqueue(message);
        }
    }
}
