using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Echo
    {
        public static string[] aliases = { "echo", "test" };
        public static void run(string text)
        {
            try
            {
                Console.WriteLine(text.Remove(0, 5));
            }
            catch (Exception e)
            {
                if (e.Message == "ArgumentOutOfRange_IndexCount Parameter name: count")
                {
                    WriteMessage.WriteError("This command requires you to add text after calling the command");
                }
                else
                {
                    WriteMessage.WriteInfo("an error occured");
                    WriteMessage.WriteError(e.Message);
                }
                
            }
        }

    }
}
