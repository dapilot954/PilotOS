﻿using PilotOS.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using PilotOS.Apps;

namespace PilotOS.System
{
    public static class Boot
    {
        public static void onBoot()
        {
            Kernel.RunGUI = true;
            GUI.Wallpaper = new Bitmap(Resources.Files.PilotOSBackroundRaw);
            GUI.Cursor = new Bitmap(Resources.Files.PilotOSCursorRaw);
            FileExplorer.Folder = new Bitmap(Resources.Files.PilotOSFolderRaw);
            FileExplorer.FileIcon = new Bitmap(Resources.Files.PilotOSFileRaw);
            FileExplorer.BackIcon = new Bitmap(Resources.Files.PilotOSBackRaw);
            FileExplorer.AddIcon = new Bitmap(Resources.Files .PilotOSAddRaw);
            FileExplorer.DeleteIcon = new Bitmap(Resources.Files.PilotOSDeleteRaw);
            GUI.StartGUI();
        }
    }
}
