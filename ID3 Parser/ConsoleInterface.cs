using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ID3_Parser
{ 
    class ConsoleUI
    {
        string dirPath;
        IEnumerable<string> filesArray; // an IEnumerable that holds the file paths of valid music files
        
        List<TagLib.File> MusicList = new List<TagLib.File>(); //list of music files

        public int FileIndex = -1;
        public string[] Command = new string[2] { "", "" }; //array that holds the command line input
        private const int CmdMax = 12;
        public string[,] CmdArray = new string[CmdMax,2]  //2d array that holds available commands and their respective description
        { 
            { "cd","" }, { "exit","" }, { "help","" }, { "clear", "" }, 
            { "list", "" },{ "select", "" }, { "tags", "" }, { "album", "" }, 
            { "title", "" },{ "performers", "" }, { "genres", "" }, { "year", "" }
        };

        public void AwaitInput()
        {
            Console.Write(">");
            string line = Console.ReadLine();
            Command = line.Split(new char[] { ' ', ',', '.', '\t', ':' },2); //parses input and then splits the string in 2 parts, command and argument

        }

        private bool CommandExists( string cmd) //function that checks whether the command exists
        {
            for (int i = 0; i < CmdMax; i++)
                if (CmdArray[i, 0] == cmd.ToLower())
                    return true;
            return false;
        }

        private void ExecuteCommand(string[] cmd) //a long switch statement where we execute the command equivalent to input
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
                case "tags":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                    PrintTags(MusicList.ElementAt(FileIndex));
                    break;
                case "title":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                        ModifyTitle(MusicList.ElementAt(FileIndex));
                    break;
                case "album":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                        ModifyAlbum(MusicList.ElementAt(FileIndex));
                    break;
                case "genres":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                        ModifyGenres(MusicList.ElementAt(FileIndex));
                    break;
                case "performers":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                        ModifyPerformers(MusicList.ElementAt(FileIndex));
                    break;
                case "year":
                    if (FileIndex < 0)
                        Console.WriteLine("No file has been selected");
                    else
                        ModifyYear(MusicList.ElementAt(FileIndex));
                    break;
            }

        }

        private void SelectFile(string[] cmd)  // first argument represents the music files' index, if no arguments are input then prints the current index if applicable
        {
            if (MusicList.Count == 0)
            {
                Console.WriteLine("Target directory is not valid or null");
                return;
            }
            if( cmd.Length < 2)
            {
                if (FileIndex < 0)
                {
                    Console.WriteLine("No file has been selected");
                    return;
                }
                else
                {
                    Console.WriteLine("Selected file is {0}", FileIndex);
                    return;
                }
            }

            int aux;

            if (Int32.TryParse(cmd[1], out aux) == false)  //if the argument is not a number or is outside the range then it prints an error
            {
                Console.WriteLine("Index is invalid");
                return;
            }
            else if (aux >= MusicList.Count || aux < 0)
            {
                Console.WriteLine("Index is out of range");
                return;
            }
            else
            {
                FileIndex = aux;
                Console.WriteLine("Selected file is {0}", FileIndex);
            }
        }

        public void SaveFile()  //saves changes for current files
        {
            if (FileIndex >= 0)
                MusicList.ElementAt(FileIndex).Save();
        }

        private void PrintTags(TagLib.File file) //prints all supported tags of the selected music file
        {
            if (FileIndex < 0)
                Console.WriteLine("Select a file first");
            else
            {
                Console.WriteLine("{0,-10} | {1}",@"Title", file.Tag.Title);
                Console.WriteLine("{0,-10} | {1}",@"Album", file.Tag.Album);
                Console.WriteLine("{0,-10} | {1}",@"Performers", String.Join(", ",file.Tag.Performers));
                Console.WriteLine("{0,-10} | {1}",@"Genres", String.Join(", ",file.Tag.Genres));
                Console.WriteLine("{0,-10} | {1}",@"Year", file.Tag.Year);
            }

        }

        private void ModifyTitle(TagLib.File file)
        {
            if (Command[1].Length == 0)
                Console.WriteLine("Not enough arguments for the specified command");
            else
                file.Tag.Title = Command[1];  
        }

        private void ModifyYear(TagLib.File file)
        {
            if (Command[1].Length == 0)
            {
                Console.WriteLine("Not enough arguments for the specified command");
                return;
            }
            else if (UInt32.TryParse(Command[1], out uint AuxYear))
                file.Tag.Year = AuxYear;
            else
                Console.WriteLine("Error, not a number or number is negative");
                
        }

        private void ModifyAlbum(TagLib.File file)
        {
            if (Command[1].Length == 0)
                Console.WriteLine("Not enough arguments for the specified command");
            else
                file.Tag.Album = Command[1];
        }

        private void ModifyPerformers(TagLib.File file)
        {
            if (Command.Length < 2)
                Console.WriteLine("Not enough arguments for the specified command");
            else
            {
                Command[1].Trim(' ');
                string[] aux = Command[1].Split(',');
                file.Tag.Performers = aux;
            }
        }

        private void ModifyGenres(TagLib.File file)
        {
            if (Command.Length < 0)
                Console.WriteLine("Not enough arguments for the specified command");
            else
            {
                Command[1].Trim(' ');
                string[] aux = Command[1].Split(',');
                file.Tag.Genres = aux;
            }
        }

        private void ListFiles()// prints the list of all music files parsed
        {
            int index = 0;

            foreach(var file in MusicList)
            {
                Console.Write("{0} | ",index++);
                    Console.WriteLine("{0}", file.Name);
            }
        }

        private void ShowHelp()  //prints all available commands and their description
        {
            CmdArray[0, 1] = "Changes the current directory";
            CmdArray[1, 1] = "Provides information for available commands";
            CmdArray[2, 1] = "Terminates the program";
            CmdArray[3, 1] = "Clears the screen";
            CmdArray[4, 1] = "Lists the name, extension and size of music files in the current directory";
            CmdArray[5, 1] = "Selects a specific file using an index after the base directory has been set, or prints the current selected index";
            CmdArray[6, 1] = "Prints all supported tags of the selected music file";
            CmdArray[7, 1] = "Modifies the album of the selected music file";
            CmdArray[8, 1] = "Modifies the title of the selected music file";
            CmdArray[9, 1] = "Modifies the performers of the selected music file. Accepts multiple names separated by ','";
            CmdArray[10, 1] = "Modifies the genres of the selected music file. Accepts multiple names separated by ','";
            CmdArray[11, 1] = "Modifies the year of the selected music file. Accepts only positive numbers";
         
            for (int i = 0; i < CmdMax; i++)
            {
                Console.WriteLine("{0}{1}",CmdArray[i,0].PadRight(8), CmdArray[i, 1]) ;
            }
        }

        private void SetDirectory(string[] CmdArg) //changes the current directory or prints the currently selected directory
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
                filesArray = Directory.GetFiles(dirPath, "*.*",SearchOption.AllDirectories).Where(
                s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".m4a") || s.EndsWith(".wma"));  //we are using a linq query to obtain the path of supported music files
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
            Regex CommandCheck = new Regex(@"[a-zA-z0-9]+"); // directory path must be of the format : 'X:\directory_name'
            if (!CommandCheck.IsMatch(Command[0]) || CommandExists(Command[0]) == false)
            {
                Console.WriteLine("Error, '{0}' is not a valid command.", Command[0]);
            }

            ExecuteCommand(Command);
        }

        private TagLib.File CreateFile( string path) //takes a string containing a file path as parameter then creates a new music file object
        {
            TagLib.File MusicFile = TagLib.File.Create(path);
            return MusicFile;
        }

        private void CreateMusicList()  //populates list with the parsed music files
        {
            foreach(var file in filesArray)
            {
                MusicList.Add(CreateFile(file));
            }
        }
    }
}