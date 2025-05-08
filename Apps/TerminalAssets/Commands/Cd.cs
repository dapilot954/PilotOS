using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Cd
    {
        public static string[] aliases = { "cd" };
        public static void run(string[] words, Terminal terminal)
        {
            if (words.Length > 1)
            {
                if (words[1] == "..")
                {
                    if (terminal.Path != @"0:\")
                    {
                        string tempPath = terminal.Path.Substring(0, terminal.Path.Length - 1);
                        terminal.Path = tempPath.Substring(0, tempPath.LastIndexOf(@"\"));
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                string path = words[1];

                if (!path.Contains(@"\"))
                    path = terminal.Path + path + @"\";
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                if (!path.EndsWith(@"\"))
                    path += @"\";
                if (Directory.Exists(path))
                {
                    terminal.Path = path;
                }
                else
                {
                    terminal.print_perm($"Directory {path} not found or doesnt exist!");
                }

            }
            else
                terminal.Path = @"0:\";
        }
    }
}
