using System;
using System.Text.RegularExpressions;
using System.IO;

namespace ID3_Parser
{ 

    public class InvalidCommandException : Exception
    {
        const string Message = "Invalid command";

        public InvalidCommandException() : base(Message)
        {
        }
    }

    class ConsoleUI
    {
        DirectoryInfo dir;
        string dirPath;
        FileInfo[] filesArray;

        public string[] Command = new string[2] { "", "" };
        private const int CmdMax = 5;
        public string[,] CmdArray = new string[CmdMax,2] { { "cd","" }, { "exit","" }, { "help","" }, { "clear", "" }, { "list", "" } };

        public void AwaitInput()
        {
            Console.Write(">");
            string line = Console.ReadLine();
            Command = line.Split(new char[] { ' ', ',', '.', '\t', ':' },2);

        }

        public bool CommandExists( string cmd)
        {
            for (int i = 0; i < CmdMax; i++)
                if (CmdArray[i, 0] == cmd)
                    return true;
            return false;
        }

        public void ExecuteCommand(string[] cmd)
        {
            switch (cmd[0].ToLower())
            {
                case "cd":
                    SetDirectory(cmd);
                    break;
                case "help":
                    ShowHelp();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "list":
                    ListFiles();
                    break;
            }

        }

        private void ListFiles()
        {
            //throw new NotImplementedException();

            foreach( FileInfo file in filesArray)
            {
                int Index = 0;
                Console.Write("{0}| ", Index++);
                Console.WriteLine("{0}  {1} bytes",file.Name,file.Length);
            }

        }

        private void ShowHelp()
        {
            

            CmdArray[0, 1] = "Changes the current directory";
            CmdArray[1, 1] = "Provides information for available commands";
            CmdArray[2, 1] = "Terminates the program";
            CmdArray[3, 1] = "Clears the screen";
            CmdArray[4, 1] = "Lists the name, extension and size of music files in the current directory";


            for (int i = 0; i < CmdMax; i++)
            {
                Console.WriteLine("{0}{1}",CmdArray[i,0].PadRight(8), CmdArray[i, 1]) ;
            }
        }

        private void SetDirectory(string[] CmdArg)
        {
            if (CmdArg.GetLength(0) == 1)
            {
                if (dirPath == null)
                    Console.WriteLine("Please input a valid path");
                else
                    Console.WriteLine("The current path is {0}",dirPath);

                return;
            }

            if (!isValidPath(CmdArg[1]) || !Directory.Exists(CmdArg[1]))
            {
                Console.WriteLine("Error, invalid path or directory does not exist");
            }
            else
            {
                dirPath = CmdArg[1];
                dir = new DirectoryInfo(dirPath);
                filesArray = dir.GetFiles();
                Console.WriteLine("The current path is now {0}", dir.Name);
            }
            
        }

        public bool isValidPath(string s)
        {
            if (s.Length < 4)
                return false;
            Regex drive = new Regex(@"^[a-zA-Z]:\\$"); //first character should be a letter, next two should be ":\"

            if (!drive.IsMatch(s.Substring(0, 3)))
                return false;

            string invalidChars = new string(Path.GetInvalidPathChars());
            invalidChars += ":/?*" + "\"";

            Regex invalidPathChars = new Regex("[" + Regex.Escape(invalidChars) + "]");

            if (invalidPathChars.IsMatch(s.Substring(3, s.Length - 3)))
                return false;

            return true;

        }

        public void ValidateCommand()
        {
            Regex CommandCheck = new Regex(@"[a-zA-z0-9]+");
            if (!CommandCheck.IsMatch(Command[0]) || CommandExists(Command[0]) == false)
            {
                Console.WriteLine("Error, '{0}' is not a valid command.", Command[0]);
            }



            ExecuteCommand(Command);
        }
        
    }
}