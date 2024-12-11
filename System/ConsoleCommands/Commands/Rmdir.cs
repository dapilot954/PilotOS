using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Rmdir
    {
        public static string[] aliases = { "rmdir", "removedir" };
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
                if (Directory.Exists(path))
                {
                    string[] path_files = Directory.GetFiles(path);
                    if (path_files.Count() > 0)
                    {
                        WriteMessage.WriteError("This directory cannot be deleted because it has files inside\nplease remove all files before deleting");
                    }
                    else
                    {
                        Directory.Delete(path);
                        WriteMessage.WriteOK($"deleted directory ({path}) successfully");
                    }
                }
                else
                {
                    WriteMessage.WriteError($"the directory {path} is not found");
                }
            }
            else
            {
                WriteMessage.WriteError("invalid syntax!");
            }
        }
    }
}
