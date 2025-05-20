using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Cat
    {
        public static string[] aliases = { "cat", "read" };
        public static void run(string[] words, Terminal terminal)
        {
            if (words.Length > 1)
            {
                string path = words[1];

                if (!path.StartsWith(@"0:\"))
                {
                    path = terminal.Path + path;
                }
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    foreach (string line in lines)
                    {
                        terminal.print_perm(line);
                    }
                }
                else
                {
                    terminal.print_perm($"the file {path} is not found");
                }


            }
            else
            {
                terminal.print_perm("invalid syntax!");
            }
        }
    }
}
