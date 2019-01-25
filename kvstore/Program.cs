using System;

namespace kvstore
{
    class Program
    { 
        static void Main(string[] args)
        {
            IDatabase database = new Database();
            ICommandProvider commandProvider = new ConsoleCommandProvider();
            CommandLine command = new CommandLine(database, commandProvider);
            command.Run();
        }
    }
}
