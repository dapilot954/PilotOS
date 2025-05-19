using PilotOS.Apps;
using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Format
    {
        public static string[] aliases = { "format" };
        public static void run()
        {
            Console.WriteLine("Formatting disk...");

            if (Kernel.fs.Disks[0].Partitions.Count > 0)
                Kernel.fs.Disks[0].DeletePartition(0);

            Kernel.fs.Disks[0].Clear();
            Kernel.fs.Disks[0].CreatePartition(100); // safer fixed size
            Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);

            Cosmos.System.Power.Reboot();
        }
    }



}
