using PilotOS.System.ConsoleCommands.Commands;
using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands
{
    internal class CommandRunner
    {
        public static void commandrun(string command)
        {
            string[] words = command.Split(" ");
            if (words.Length > 0)
            {
                if (Echo.aliases.Contains(words[0]) == true)
                {
                    Echo.run(command);
                }
                else if (Ls.aliases.Contains(words[0]) == true)
                {
                    Ls.run();
                }
                else if (Reboot.aliases.Contains(words[0]) == true)
                {
                    Reboot.run();
                }
                else if (Shutdown.aliases.Contains(words[0]) == true)
                {
                    Shutdown.run();
                }
                else if (Format.aliases.Contains(words[0]) == true)
                {
                    Format.run(words);
                }
                else if (Storageinfo.aliases.Contains(words[0]) == true)
                {
                    Storageinfo.run();
                }
                else if (Add.aliases.Contains(words[0]) == true)
                {
                    Add.run(words);
                }
                else if (Cd.aliases.Contains(words[0]) == true)
                {
                    Cd.run(words);
                }
                else if (Mkdir.aliases.Contains(words[0]) == true)
                {
                    Mkdir.run(words);
                }
                else if (Rm.aliases.Contains(words[0]) == true)
                {
                    Rm.run(words);
                }
                else if (Rmdir.aliases.Contains(words[0]) == true)
                {
                    Rmdir.run(words);
                }
                else if (Cat.aliases.Contains(words[0]) == true)
                {
                    Cat.run(words);
                }
                else if (Clear.aliases.Contains(words[0]) == true)
                {
                    Clear.run();
                }
                else if (Gui.aliases.Contains(words[0]) == true)
                {
                    Gui.run();
                }
                else if (Purge.aliases.Contains(words[0]) == true)
                {
                    Purge.run(words);
                }
                else
                {
                    WriteMessage.WriteError("error, no command found");
                }

            }
            else
            {
                WriteMessage.WriteError("error, no command found");
            }
        }
    }
}
