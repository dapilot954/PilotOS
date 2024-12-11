using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Gui
    {
        public static string[] aliases = { "gui" };
        public static void run()
        {
            Boot.onBoot();
        }
    }
}
