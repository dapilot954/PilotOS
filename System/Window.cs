﻿using Cosmos.HAL;
using Cosmos.System;
using PilotOS.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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
            
        }
    }
}
