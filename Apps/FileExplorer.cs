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
        public static Bitmap Refresh;
        public string Path = @"0:\";
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
            GUI.MainCanvas.DrawImageAlpha(Refresh, x + SizeX - 30, y + Window.TopSize);
            GUI.MainCanvas.DrawImageAlpha(Folder, x, y + Window.TopSize + 30);
            string Disp_path = TrimStringFromLeft(Path, SizeX - 30);
            GUI.MainCanvas.DrawString(Disp_path, GUI.FontDefault, GUI.colors.ColorText, x + 2, y + Window.TopSize + 7);

        }

        public static string TrimStringFromLeft(string input, int maxPixels)
        {
            const int charWidth = 8;
            const string ellipsis = "...";
            int maxChars = maxPixels / charWidth;

            if (string.IsNullOrEmpty(input) || maxChars <= 0)
                return "";

            if (input.Length * charWidth <= maxPixels)
                return input;

            // Reserve space for "..."
            int visibleChars = maxChars - ellipsis.Length;

            if (visibleChars <= 0)
                return ellipsis.Substring(0, maxChars); // Not enough room for any characters

            string trimmed = input.Substring(input.Length - visibleChars);
            return ellipsis + trimmed;
        }

    }

}
