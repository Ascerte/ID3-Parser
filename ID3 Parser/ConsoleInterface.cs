using System;
using System.Text.RegularExpressions;

namespace ID3_Parser
{
    public enum Commands
    {
        cd,
        help,
        exit
    };

    public class InvalidCommandException : Exception
    {
        const string Message = "Invalid command";

        public InvalidCommandException() : base(Message)
        {
        }
    }

    class ConsoleUI
    {
        public string[] Command = new string[2];

        public void AwaitInput()
        {
            Console.Write(">");
            string line = Console.ReadLine();
            Command = line.Split(new char[] { ' ', ',', '.', '\t', ':' });

        }

        public bool CommandExists( string cmd)
        {
            foreach (var c in Enum.GetValues(typeof(Commands)))
                if (cmd == c.ToString())
                    return true;
            return false;
        }

        public bool ValidateCommand()
        {
            Regex CommandCheck = new Regex(@"[a-zA-z0-9]+");
            if (!CommandCheck.IsMatch(Command[0]) || CommandExists(Command[0]) == false)
            {
                Console.WriteLine("Error, '{0}' is not a valid command.", Command[0]);
            }



            return true;
        }
        
    }
}