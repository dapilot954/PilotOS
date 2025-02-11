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
        public static void run(string[] words)
        {
            if (words.Length > 1)
            {

            }
            else
            {
                Terminal.print_perm("welcome to the help menu, below is a list of all of the possible commands in this terminal");
                Terminal.print_perm("1) add");
                Terminal.print_perm("2) cat");
                Terminal.print_perm("3) cd");
                Terminal.print_perm("4) clear");
                Terminal.print_perm("5) echo");
                Terminal.print_perm("6) format");
                Terminal.print_perm("7) help");
                Terminal.print_perm("8) ls");
                Terminal.print_perm("9) mkdir");
                Terminal.print_perm("10) purge");
                Terminal.print_perm("11) reboot");
                Terminal.print_perm("12) rm");
                Terminal.print_perm("13) rmdir");
                Terminal.print_perm("14) shutdown");
                Terminal.print_perm("15) storageinfo");
            }
        }
    }
}
