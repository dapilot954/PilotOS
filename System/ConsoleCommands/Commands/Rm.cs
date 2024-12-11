using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Rm
    {
        public static string[] aliases = { "rm" };
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
                    File.Delete(path);
                    WriteMessage.WriteOK($"deleted file ({path}) successfully");
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
