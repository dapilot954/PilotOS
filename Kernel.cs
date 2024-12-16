using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using PilotOS.System.Utils;
using PilotOS.System.ConsoleCommands;
using PilotOS.Graphics;
using Cosmos.Core.Memory;
using System.Security.Cryptography.X509Certificates;

namespace PilotOS
{
    public class Kernel : Sys.Kernel
    {
        public static string Version = "0.3.1";
        public static string Path = @"0:\";
        public static CosmosVFS fs;
        public static bool RunGUI;
        int lastHeapCollect;
        protected override void BeforeRun()
        {
            Console.SetWindowSize(90, 30);
            Console.Clear();
            Console.OutputEncoding = Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance.GetEncoding(437);
            fs = new Cosmos.System.FileSystem.CosmosVFS();
            Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            WriteMessage.Logo();
            Console.WriteLine();
            Console.WriteLine(WriteMessage.CenterText("PilotOS running version " + Version));


        }

        protected override void Run()
        {
            
            if (RunGUI == false)
            {
                Console.WriteLine();
                Console.Write(Path + ">");
                var command = Console.ReadLine();
                CommandRunner.commandrun(command);
            }
            else
            {
                GUI.Update();

            }
            if (lastHeapCollect >= 20)
            {
                Heap.Collect();
                lastHeapCollect = 0;
            }
            else
            {
                lastHeapCollect++;
            }


        }
    }
}
