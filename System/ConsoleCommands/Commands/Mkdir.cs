﻿using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.ConsoleCommands.Commands
{
    internal class Mkdir
    {
        public static string[] aliases = { "mkdir", "makedir" };
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
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    WriteMessage.WriteError($"the directory {path} already exists");
                }
            }

            else
            {
                WriteMessage.WriteError("invalid syntax!");
            }
        }
    }
}