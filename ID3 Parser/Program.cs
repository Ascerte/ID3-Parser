using System;
using System.Text.RegularExpressions;
using System.IO;

namespace ID3_Parser
{

    class Target_Dir
    {
        DirectoryInfo dir;
        string dirPath;
        FileInfo[] filesArray;

        public Target_Dir ()
        {
            Console.WriteLine("Input the target directory");
            dirPath = Console.ReadLine();

            while (!isValidPath(dirPath) || !Directory.Exists(dirPath))
            {
                Console.WriteLine("Error, invalid path or directory does not exist");
                dirPath = Console.ReadLine();
            }

        }

        public bool isValidPath(string s)
        {
            if (s.Length < 4)
                return false;
            Regex drive = new Regex(@"^[a-zA-Z]:\\$"); //first character should be a letter, next two should be ":\"

            if (! drive.IsMatch(s.Substring(0,3)))
                return false;

            string invalidChars = new string(Path.GetInvalidPathChars());
            invalidChars += ":/?*" + "\"";

            Regex invalidPathChars = new Regex("[" + Regex.Escape(invalidChars) + "]");

            if (invalidPathChars.IsMatch(s.Substring(3, s.Length - 3)))
                return false;

            return true;

        }

        public string getPath()
        {
            return dirPath;
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {

            // Target_Dir dir = new Target_Dir();

            ConsoleUI ui = new ConsoleUI();

            while (true)
            { 
            ui.AwaitInput();
            ui.ValidateCommand();
            
            }
            
        }
    }
}
