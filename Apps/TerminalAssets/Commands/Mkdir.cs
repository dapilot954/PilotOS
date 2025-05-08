using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Mkdir
    {
        public static string[] aliases = { "mkdir", "makedir" };
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
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                    terminal.print_perm($"created directory ({path}) successfully");
                }
                else
                {
                    terminal.print_perm($"the directory {path} already exists");
                }
            }

            else
            {
                terminal.print_perm("invalid syntax!");
            }
        }
    }
}
