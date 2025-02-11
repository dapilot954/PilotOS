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
        public static void run(string[] words)
        {
            if (words.Length > 1)
            {
                if (words[1] == "..")
                {
                    if (Kernel.Path != @"0:\")
                    {
                        string tempPath = Kernel.Path.Substring(0, Kernel.Path.Length - 1);
                        Kernel.Path = tempPath.Substring(0, tempPath.LastIndexOf(@"\"));
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                string path = words[1];

                if (!path.Contains(@"\"))
                    path = Kernel.Path + path + @"\";
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                if (!path.EndsWith(@"\"))
                    path += @"\";
                if (Directory.Exists(path))
                {
                    Kernel.Path = path;
                }
                else
                {
                    Terminal.print_perm($"Directory {path} not found or doesnt exist!");
                }

            }
            else
                Kernel.Path = @"0:\";
        }
    }
}
