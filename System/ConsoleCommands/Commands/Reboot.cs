using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Reboot
    {
        public static string[] aliases = { "reboot", "restart" };
        public static void run()
        {
            WriteMessage.WriteInfo("rebooting");
            Cosmos.System.Power.Reboot();
        }
    }
}
