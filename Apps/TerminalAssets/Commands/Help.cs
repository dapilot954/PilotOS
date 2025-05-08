using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Help
    {
        public static string[] aliases = { "help", "info" };
        public static void run(string[] words, Terminal terminal)
        {
            if (words.Length > 1)
            {

            }
            else
            {
                terminal.print_perm("welcome to the help menu, below is a list of all of the possible commands in this terminal");
                terminal.print_perm("1) add");
                terminal.print_perm("2) cat");
                terminal.print_perm("3) cd");
                terminal.print_perm("4) clear");
                terminal.print_perm("5) echo");
                terminal.print_perm("6) format");
                terminal.print_perm("7) help");
                terminal.print_perm("8) ls");
                terminal.print_perm("9) mkdir");
                terminal.print_perm("10) purge");
                terminal.print_perm("11) reboot");
                terminal.print_perm("12) rm");
                terminal.print_perm("13) rmdir");
                terminal.print_perm("14) shutdown");
                terminal.print_perm("15) storageinfo");
            }
        }
    }
}
