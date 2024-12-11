using PilotOS.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;

namespace PilotOS.System
{
    public static class Boot
    {
        public static void onBoot()
        {
            Kernel.RunGUI = true;
            GUI.Wallpaper = new Bitmap(Resources.Files.PilotOSBackroundRaw);
            GUI.Cursor = new Bitmap(Resources.Files.PilotOSCursorRaw);
            GUI.StartGUI();
        }
    }
}
