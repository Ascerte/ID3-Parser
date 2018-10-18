using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
        string dirPath;
        IEnumerable<string> filesArray;
        
        List<TagLib.File> MusicList = new List<TagLib.File>();

        public int FileIndex = 0;
        public string[] Command = new string[2] { "", "" };
        private const int CmdMax = 6;
        public string[,] CmdArray = new string[CmdMax,2] { { "cd","" }, { "exit","" }, { "help","" }, { "clear", "" }, { "list", "" }, { "select", "" } };

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
                case "select":
                    SelectFile(cmd);
                    break;
            }

        }

        private void SelectFile(string[] cmd)
        {
            if (MusicList.Count == 0)
                Console.WriteLine("Target directory is not valid or null");
            else
            {
                Int32.TryParse(cmd[1], out FileIndex);
                Console.WriteLine(MusicList.ElementAt(FileIndex).Name);
            }
        }

        public void PrintTags(TagLib.File file)
        {
            //Console.WriteLine("ID3 Tags :")
            
            //Console.WriteLine("{0,-10} | {1} | {2}", file.Tag.Title, String.Join(" ",file.Tag.Performers), String.Join(" ",file.Tag.Genres));
        }

        private void ListFiles()
        {
            int index = 0;

            foreach(var file in MusicList)
            {
                Console.Write("{0} | ",index++);
                    Console.WriteLine("{0}", file.Name);
            }

            

        }

        private void ShowHelp()
        {
            

            CmdArray[0, 1] = "Changes the current directory";
            CmdArray[1, 1] = "Provides information for available commands";
            CmdArray[2, 1] = "Terminates the program";
            CmdArray[3, 1] = "Clears the screen";
            CmdArray[4, 1] = "Lists the name, extension and size of music files in the current directory";
            CmdArray[5, 1] = "Selects a specific file using an index after the base directory has been set";


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
                filesArray = Directory.GetFiles(dirPath, "*.*",SearchOption.AllDirectories).Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".m4a") || s.EndsWith(".wma"));
                Console.WriteLine("The current path is now {0}", dirPath);
                CreateMusicList();
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

        public TagLib.File CreateFile( string path)
        {
            TagLib.File MusicFile = TagLib.File.Create(path);
            return MusicFile;
        }

        public void CreateMusicList()
        {
            foreach(var file in filesArray)
            {
                MusicList.Add(CreateFile(file));


            }
        }
        
    }
}