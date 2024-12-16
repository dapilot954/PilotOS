using PilotOS.Resources;
using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
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
                    WriteMessage.WriteOK("Purged ("+ path+") successfully");
                    
                }
                else
                {
                    WriteMessage.WriteError($"the file {path} is not found");
                }
            }
            else
            {
                WriteMessage.WriteError("invalid syntax!");
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
