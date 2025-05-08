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
        public static void commandrun(string command, Terminal terminal)
        {
            string[] words = command.Split(" ");
            if (words.Length > 0)
            {
                if (Echo.aliases.Contains(words[0]) == true)
                {
                    Echo.run(command, terminal);
                }
                else if (Ls.aliases.Contains(words[0]) == true)
                {
                    Ls.run(terminal);
                }
                else if (Add.aliases.Contains(words[0]) == true)
                {
                    Add.run(words, terminal);
                }
                else if (Cat.aliases.Contains(words[0]) == true)
                {
                    Cat.run(words, terminal);
                }
                else if (Shutdown.aliases.Contains(words[0]) == true)
                {
                    Shutdown.run(terminal);
                }
                else if (Reboot.aliases.Contains(words[0]) == true)
                {
                    Reboot.run(terminal);
                }
                else if (Rm.aliases.Contains(words[0]) == true)
                {
                    Rm.run(words, terminal);
                }
                else if (Cd.aliases.Contains(words[0]) == true)
                {
                    Cd.run(words, terminal);
                }
                else if (Mkdir.aliases.Contains(words[0]) == true)
                {
                    Mkdir.run(words, terminal);
                }
                else if (Storageinfo.aliases.Contains(words[0]) == true)
                {
                    Storageinfo.run(terminal);
                }
                else if (Format.aliases.Contains(words[0]) == true)
                {
                    Format.run(terminal);
                }
                else if (Clear.aliases.Contains(words[0]) == true)
                {
                    Clear.run(terminal);
                }
                else if (Purge.aliases.Contains(words[0]) == true)
                {
                    Purge.run(words, terminal);
                }
                else if (Rmdir.aliases.Contains(words[0]) == true)
                {
                    Rmdir.run(words, terminal);
                }
                else if (Help.aliases.Contains(words[0]) == true)
                {
                    Help.run(words, terminal);
                }
                else
                {
                    terminal.print_perm("error, no command found");
                }
            }
            else
            {
                terminal.print_perm("error, no command found");
            }
        }
    }
}
