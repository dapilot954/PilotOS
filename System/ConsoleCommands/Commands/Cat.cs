using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Cat
    {
        public static string[] aliases = { "cat", "read" };
        public static void run(string[] words)
        {
            if (words.Length > 1)
            {
                string path = words[1];

                if (!path.StartsWith(@"0:\"))
                {
                    path = Kernel.Path + path;
                }
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    WriteMessage.WriteError($"the file {path} is not found");
                }


            }
            else
            {
                WriteMessage.WriteError("invalid syntax!");
            }
        }
    }
}
