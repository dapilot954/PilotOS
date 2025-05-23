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
        public static void run(string[] words)
        {
            var disk = Kernel.fs.Disks[0];

            if (words.Length < 2)
            {
                Console.WriteLine("Usage: format <list|del <index>|mk <sizeMB>|fs <index>>");
                return;
            }

            var subcommand = words[1].ToLower();

            switch (subcommand)
            {
                case "list":
                    Console.WriteLine($"Disk 0: Size = {disk.Size} bytes, Partitions = {disk.Partitions.Count}");
                    for (int i = 0; i < disk.Partitions.Count; i++)
                    {
                        Console.WriteLine($"  Partition {i}");
                    }
                    break;

                case "del":
                    if (words.Length < 3 || !int.TryParse(words[2], out int delIndex))
                    {
                        Console.WriteLine("Usage: format del <partitionIndex>");
                        return;
                    }

                    if (delIndex < 0 || delIndex >= disk.Partitions.Count)
                    {
                        Console.WriteLine("Invalid partition index.");
                        return;
                    }

                    try
                    {
                        disk.DeletePartition(delIndex);
                        Console.WriteLine($"Deleted partition {delIndex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting partition: {ex.Message}");
                    }
                    break;

                case "mk":
                    if (words.Length < 3 || !uint.TryParse(words[2], out uint sizeMB))
                    {
                        Console.WriteLine("Usage: format mk <sizeMB>");
                        return;
                    }

                    try
                    {
                        disk.CreatePartition((int)sizeMB);
                        Console.WriteLine($"Created partition with size {sizeMB} MB");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating partition: {ex.Message}");
                    }
                    break;

                case "fs":
                    if (words.Length < 3 || !int.TryParse(words[2], out int fsIndex))
                    {
                        Console.WriteLine("Usage: format fs <partitionIndex>");
                        return;
                    }

                    if (fsIndex < 0 || fsIndex >= disk.Partitions.Count)
                    {
                        Console.WriteLine("Invalid partition index.");
                        return;
                    }

                    try
                    {
                        disk.FormatPartition(fsIndex, "FAT32", true);
                        Console.WriteLine($"Formatted partition {fsIndex} to FAT32");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error formatting partition: {ex.Message}");
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown subcommand: {subcommand}");
                    break;
            }
        }




    }



}
