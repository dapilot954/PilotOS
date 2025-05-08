using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Echo
    {
        public static string[] aliases = { "echo", "test" };
        public static void run(string text, Terminal terminal)
        {
            try
            {
                terminal.print_perm(text.Remove(0, 5));
            }
            catch (Exception e)
            {
                if (e.Message == "ArgumentOutOfRange_IndexCount Parameter name: count")
                {
                    terminal.print_perm("This command requires you to add text after calling the command");
                }
                else
                {
                    terminal.print_perm("an error occured");
                    terminal.print_perm(e.Message);
                }

            }
        }
    }
}
