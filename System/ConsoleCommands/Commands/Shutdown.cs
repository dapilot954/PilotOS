using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Shutdown
    {
        public static string[] aliases = { "shutdown", "poweroff" };
        public static void run()
        {
            WriteMessage.WriteInfo("shutting down");
            Cosmos.System.Power.Shutdown();
        }
    }
}
