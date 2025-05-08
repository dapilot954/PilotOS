using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Ls
    {
        public static string[] aliases = { "ls", "list" };
        public static void run(Terminal terminal)
        {
            var Directories = Directory.GetDirectories(terminal.Path);
            var Files = Directory.GetFiles(terminal.Path);
            terminal.print_perm("Directories (" + Directories.Length + ")");
            for (int i = 0; i < Directories.Length; i++)
            {
                terminal.print_perm(Directories[i]);
            }
            terminal.print_perm("Files (" + Files.Length + ")");
            for (int i = 0; i < Files.Length; i++)
            {
                terminal.print_perm(Files[i]);
            }
        }
    }
}
