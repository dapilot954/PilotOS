using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Ls
    {
        public static string[] aliases = { "ls", "list" };
        public static void run()
        {
            var Directories = Directory.GetDirectories(Kernel.Path);
            var Files = Directory.GetFiles(Kernel.Path);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Directories (" + Directories.Length + ")");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < Directories.Length; i++)
            {
                Console.WriteLine(Directories[i]);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Files (" + Files.Length + ")");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < Files.Length; i++)
            {
                Console.WriteLine(Files[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
