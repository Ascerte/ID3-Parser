using System;
using System.Text.RegularExpressions;
using System.IO;
using TagLib;

namespace ID3_Parser
{   
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleUI ui = new ConsoleUI();

            while (true)
            {
                ui.AwaitInput();
                ui.ValidateCommand();
                ui.SaveFile();

            }
        }
    }
}
