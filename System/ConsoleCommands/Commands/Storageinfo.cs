using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Storageinfo
    {
        public static string[] aliases = { "storageinfo", "storage" };
        public static void run()
        {
            float free = Kernel.fs.GetAvailableFreeSpace(Kernel.Path);
            float total = Kernel.fs.GetTotalSize(Kernel.Path);
            float used = total - free;
            float percentage_used = used / total * 100;
            float percentage_free = free / total * 100;
            string data_order1 = "B";
            string data_order2 = "B";
            string data_order3 = "B";
            if (free > 1024)
            {
                free /= 1024;
                data_order1 = "KB";
                if (free > 1024)
                {
                    free /= 1024;
                    data_order1 = "MB";
                    if (free > 1024)
                    {
                        free /= 1024;
                        data_order1 = "GB";
                    }
                }
            }
            if (total > 1024)
            {
                total /= 1024;
                data_order2 = "KB";
                if (total > 1024)
                {
                    total /= 1024;
                    data_order2 = "MB";
                    if (total > 1024)
                    {
                        total /= 1024;
                        data_order2 = "GB";
                    }
                }
            }
            if (used > 1024)
            {
                used /= 1024;
                data_order3 = "KB";
                if (used > 1024)
                {
                    used /= 1024;
                    data_order3 = "MB";
                    if (used > 1024)
                    {
                        used /= 1024;
                        data_order3 = "GB";
                    }
                }
            }
            Console.WriteLine("used space on drive = " + used + " " + data_order3 + $" ({percentage_used}%)");
            Console.WriteLine("Free space on drive = " + free + " " + data_order1 + $" ({percentage_free}%)");
            Console.WriteLine("total size of drive = " + total + " " + data_order2);
        }
    }
}
