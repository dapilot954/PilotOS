using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Add
    {
        public static string[] aliases = { "add", "write" };
        public static void run(string[] words)
        {
            if (words.Length > 1)
            {
                string wholestring = "";
                for (int i = 0; i < words.Length; i++)
                {
                    wholestring += words[i] + " ";

                }
                int pathIndex = wholestring.LastIndexOf('>');
                string text = wholestring.Substring(0, pathIndex);
                string path = wholestring.Substring(pathIndex + 1);
                if (!path.StartsWith(@"0:\"))
                {
                    path = Kernel.Path + path;
                }
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                text = text.Remove(0, 4);
                File.WriteAllText(path, text);
                WriteMessage.WriteOK("completed succesfully");

            }
            else
            {
                WriteMessage.WriteError("invalid syntax!");
            }
        }
    }
}
