using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Reboot
    {
        public static string[] aliases = { "reboot", "restart" };
        public static void run(Terminal terminal)
        {
            terminal.print_perm("Rebooting");
            Cosmos.System.Power.Reboot();
        }
    }
}
