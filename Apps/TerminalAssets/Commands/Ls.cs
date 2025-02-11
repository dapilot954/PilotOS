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
        public static void run()
        {
            var Directories = Directory.GetDirectories(Kernel.Path);
            var Files = Directory.GetFiles(Kernel.Path);
            Terminal.print_perm("Directories (" + Directories.Length + ")");
            for (int i = 0; i < Directories.Length; i++)
            {
                Terminal.print_perm(Directories[i]);
            }
            Terminal.print_perm("Files (" + Files.Length + ")");
            for (int i = 0; i < Files.Length; i++)
            {
                Terminal.print_perm(Files[i]);
            }
        }
    }
}
