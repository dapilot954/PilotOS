using Cosmos.HAL;
using Cosmos.System;
using PilotOS.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System
{
    public static class Window
    {
        public static int TopSize = 30;
        public static void DrawTop(Process proc)
        {
            CustomDrawing.DrawTopRoundedRectangle(proc.WindowData.WinPos.X, proc.WindowData.WinPos.Y, proc.WindowData.WinPos.Width, TopSize, TopSize/2, GUI.colors.ColorDark);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width - 30, proc.WindowData.WinPos.Height + proc.WindowData.WinPos.Y - 30, 30, 30);
            GUI.MainCanvas.DrawString(proc.Name, GUI.FontDefault, GUI.colors.ColorText, proc.WindowData.WinPos.X + 15, proc.WindowData.WinPos.Y + 8);
            GUI.MainCanvas.DrawFilledCircle(Color.Red, proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width - 15, proc.WindowData.WinPos.Y + 15, 10);

            if (MouseManager.MouseState == MouseState.Left)
            {
                int mx = (int)MouseManager.X;
                int my = (int)MouseManager.Y;

                if (mx > proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width - 30 && mx < proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width && my > proc.WindowData.WinPos.Y + 5 && my < proc.WindowData.WinPos.Y + 25)
                {
                    if (proc.WindowData.selected)
                    {
                        ProcessManager.ProcessList.Remove(proc);
                    }

                }
            }
            
        }
    }
}
