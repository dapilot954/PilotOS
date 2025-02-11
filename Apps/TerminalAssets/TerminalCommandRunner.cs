using PilotOS.Apps.TerminalAssets.Commands;
using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PilotOS.Apps.TerminalAssets
{
    internal class TerminalCommandRunner
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
                else if (Add.aliases.Contains(words[0]) == true)
                {
                    Add.run(words);
                }
                else if (Cat.aliases.Contains(words[0]) == true)
                {
                    Cat.run(words);
                }
                else if (Shutdown.aliases.Contains(words[0]) == true)
                {
                    Shutdown.run();
                }
                else if (Reboot.aliases.Contains(words[0]) == true)
                {
                    Reboot.run();
                }
                else if (Rm.aliases.Contains(words[0]) == true)
                {
                    Rm.run(words);
                }
                else if (Cd.aliases.Contains(words[0]) == true)
                {
                    Cd.run(words);
                }
                else if (Mkdir.aliases.Contains(words[0]) == true)
                {
                    Mkdir.run(words);
                }
                else if (Storageinfo.aliases.Contains(words[0]) == true)
                {
                    Storageinfo.run();
                }
                else if (Format.aliases.Contains(words[0]) == true)
                {
                    Format.run();
                }
                else if (Clear.aliases.Contains(words[0]) == true)
                {
                    Clear.run();
                }
                else if (Purge.aliases.Contains(words[0]) == true)
                {
                    Purge.run(words);
                }
                else if (Rmdir.aliases.Contains(words[0]) == true)
                {
                    Rmdir.run(words);
                }
                else if (Help.aliases.Contains(words[0]) == true)
                {
                    Help.run(words);
                }
                else
                {
                    Terminal.print_perm("error, no command found");
                }
            }
            else
            {
                Terminal.print_perm("error, no command found");
            }
        }
    }
}
