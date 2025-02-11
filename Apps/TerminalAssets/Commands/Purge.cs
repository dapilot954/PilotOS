using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Purge
    {
        public static string[] aliases = { "purge" };
        public static void run(string[] words)
        {
            if (words.Length > 1)
            {
                string path = words[1];

                if (!path.StartsWith(@"0:\"))
                {
                    path = Kernel.Path + path;
                }
                if (path.EndsWith(' '))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                if (Directory.Exists(path))
                {

                    PurgeFiles(path);
                    PurgeDirs(path);
                    Terminal.print_perm("Purged (" + path + ") successfully");

                }
                else
                {
                    Terminal.print_perm($"the directory ({path}) is not found or does not exist");
                }
            }
            else
            {
                 Terminal.print_perm("invalid syntax!");
            }
        }


        public static void PurgeFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            if (files.Length > 0)
            {
                int i = 0;
                while (true)
                {
                    if (i == files.Length)
                    {
                        break;
                    }
                    else
                    {
                        File.Delete(path + "\\" + files[i]);
                        Terminal.print_perm($"deleted file ({path + "\\" + files[i]}) successfully");
                        i++;

                    }

                }
            }
            if (directories.Length > 0)
            {
                int i = 0;
                while (true)
                {
                    if (i == directories.Length)
                    {
                        break;
                    }
                    else
                    {
                        PurgeFiles(path + "\\" + directories[i]);
                        PurgeDirs(path + "\\" + directories[i]);
                        i++;
                    }


                }


            }

        }

        public static void PurgeDirs(string path)
        {
            string[] directories = Directory.GetDirectories(path);
            if (directories.Length > 0)
            {
                int i = 0;
                while (true)
                {
                    if (i == directories.Length)
                    {
                        break;
                    }
                    else
                    {
                        string[] x = Directory.GetDirectories(path + "\\" + directories[i]);
                        if (x.Length == 0)
                        {
                            Directory.Delete(path + "\\" + directories[i]);
                            Terminal.print_perm($"deleted directory ({path + "\\" + directories[i]}) successfully");
                            i++;
                        }
                        else
                        {
                            PurgeDirs(path + "\\" + directories[i]);
                            i++;
                        }

                    }
                }
            }
        }
    }
}
