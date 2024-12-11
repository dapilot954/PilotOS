using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Clear
    {
        public static string[] aliases = { "clear", "cls" };
        public static void run()
        {
            Console.Clear();
        }
    }
}
