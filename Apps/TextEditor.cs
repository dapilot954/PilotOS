using Cosmos.System;
using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps
{
    public class TextEditor : Process
    {
        public string Path = "";
        public override void Start()
        {
            Path = WindowData.args;
            
        }
        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;
            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);
            if (File.Exists(Path))
            {
                
            }

        }
        public override void OnKeyPressed(KeyEvent key)
        {
            
        }
    }
}
