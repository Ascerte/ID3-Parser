using System;
using System.Text.RegularExpressions;
using System.IO;

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
            
            }
            
        }
    }
}
