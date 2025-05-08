using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Shutdown
    {
        public static string[] aliases = { "shutdown", "poweroff" };
        public static void run(Terminal terminal)
        {
            terminal.print_perm("shutting down");
            Cosmos.System.Power.Shutdown();
        }
    }
}
