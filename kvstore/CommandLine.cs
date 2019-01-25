using System;

namespace kvstore
{
    public interface ICommandProvider
    {
        string Next();
        void Write(string message);
    }

    internal class ConsoleCommandProvider : ICommandProvider
    {
        public string Next()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }

        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }

    public class CommandLine
    {
        IDatabase db;
        ICommandProvider commandProvider;

        public CommandLine(IDatabase db, ICommandProvider commandProvider)
        {
            this.db = db;
            this.commandProvider = commandProvider;
        }

        public void Run()
        {
            for (; ; )
            {
                string command = commandProvider.Next();
                string[] args = command.Split(null);

                if (args.Length == 0) continue;

                try
                {
                    switch (args[0].ToUpper())
                    {
                        case "PUT":
                            db.Put(args[1], args[2]);
                            break;
                        case "GET":
                            commandProvider.Write(db.Get(args[1]));
                            break;
                        case "DELETE":
                            db.Delete(args[1]);
                            break;
                        case "PUTREF":
                            db.PutRef(args[1], args[2]);
                            break;
                        case "GETREF":
                            commandProvider.Write(db.GetRef(args[1]));
                            break;
                        case "BEGIN":
                            db = db.Begin();
                            break;
                        case "COMMIT":
                            db = db.Commit();
                            break;
                        case "ROLLBACK":
                            db = db.Rollback();
                            break;
                        case "QUIT":
                            commandProvider.Write("bye");
                            return;
                        default:
                            commandProvider.Write("ERROR - Unknown command.");
                            break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    commandProvider.Write(
                        String.Format("ERROR - Not enough arguments for command {0}.", args[0])
                        );
                }
                catch (DatabaseException de)
                {
                    commandProvider.Write(de.Message);
                }
            }
        }
    }
}