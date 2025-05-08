using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Clear
    {
        
        public static string[] aliases = { "clear", "cls" };
        public static void run(Terminal terminal)
        {
            terminal.clear();
        }
        
    }
}
