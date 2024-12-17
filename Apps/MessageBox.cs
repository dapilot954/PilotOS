using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;

namespace PilotOS.Apps
{
    public class MessageBox : Process
    {
        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY-Window.TopSize);
            Window.DrawTop(this);
        }
    }
}
