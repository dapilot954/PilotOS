using Cosmos.System.Graphics;
using PilotOS.Graphics;
using PilotOS.Resources;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps
{
    public class FileExplorer : Process
    {
        public static Bitmap Folder;
        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;




            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, x, y + Window.TopSize, SizeX, 30);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, x + 1, y + Window.TopSize + 1, SizeX - 2, 28);
            GUI.MainCanvas.DrawImageAlpha(Folder, x, y + Window.TopSize);
        }
    }
    
}
