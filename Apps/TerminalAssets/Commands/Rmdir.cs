using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Rmdir
    {
        public static string[] aliases = { "rmdir", "removedir" };
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
                if (Directory.Exists(path))
                {
                    string[] path_files = Directory.GetFiles(path);
                    if (path_files.Count() > 0)
                    {
                        terminal.print_perm("This directory cannot be deleted because it has files inside\nplease remove all files before deleting");
                    }
                    else
                    {
                        Directory.Delete(path);
                        terminal.print_perm($"deleted directory ({path}) successfully");
                    }
                }
                else
                {
                    terminal.print_perm($"the directory ({path}) is not found");
                }
            }
            else
            {
                terminal.print_perm("invalid syntax!");
            }
        }
    }
}
