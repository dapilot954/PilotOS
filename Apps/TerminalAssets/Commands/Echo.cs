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
        public static void run(string text)
        {
            try
            {
                Terminal.print_perm(text.Remove(0, 5));
            }
            catch (Exception e)
            {
                if (e.Message == "ArgumentOutOfRange_IndexCount Parameter name: count")
                {
                    Terminal.print_perm("This command requires you to add text after calling the command");
                }
                else
                {
                    Terminal.print_perm("an error occured");
                    Terminal.print_perm(e.Message);
                }

            }
        }
    }
}
