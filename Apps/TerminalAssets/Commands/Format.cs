﻿using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Format
    {
        public static string[] aliases = { "format" };
        public static void run(Terminal terminal)
        {
            if (Kernel.fs.Disks[0].Partitions.Count > 0)
            {
                Kernel.fs.Disks[0].DeletePartition(0);
            }
            Kernel.fs.Disks[0].Clear();
            Kernel.fs.Disks[0].CreatePartition((int)(Kernel.fs.Disks[0].Size / (1024 * 1024)));
            Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
            terminal.print_perm("Success");
            terminal.print_perm("DoorsOS will reboot in 3 seconds");
            Thread.Sleep(3000);
            Cosmos.System.Power.Reboot();
        }
    }
}
