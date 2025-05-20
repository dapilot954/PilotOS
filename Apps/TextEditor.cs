using Cosmos.System;
using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps
{
    public class TextEditor : Process
    {
        public string Path = "";
        public string Screen = "";
        int x;
        int y;
        int SizeX;
        int SizeY;

        public override void Start()
        {
            Path = WindowData.args;
            
        }
        public override void Run()
        {
            x = WindowData.WinPos.X;
            y = WindowData.WinPos.Y;
            SizeX = WindowData.WinPos.Width;
            SizeY = WindowData.WinPos.Height;
            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);
            if (File.Exists(Path))
            {
                Screen = "Editing";

            }
            else
            {
                Screen = "NoFile";
                DrawNoFile();
            }

        }
        public override void OnKeyPressed(KeyEvent key)
        {
            
        }
        public void DrawNoFile()
        {
            GUI.MainCanvas.DrawString("File (" + Path + ") was not found or doesnt exist", GUI.FontDefault, Color.Red, x, y + Window.TopSize);
        }
        public void DrawFileEditor()
        {

        }
    }
}
