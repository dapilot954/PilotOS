using Cosmos.System.Graphics;
using PilotOS.Graphics;
using PilotOS.Resources;
using PilotOS.System;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps
{
    public class FileExplorer : Process
    {
        public static Bitmap Folder;
        public static Bitmap File;
        public string Path = @"0:\";
        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;

            var Directories = Directory.GetDirectories(Path);
            List<string> Folders = new List<string>();

            for (int i = 0; i < Directories.Length; i++)
            {
                Folders.Add(Directories[i]);
            }

            List<string> Files = new List<string>();
            var File_raw = Directory.GetFiles(Path);

            for (int i = 0; i < File_raw.Length; i++)
            {
                Files.Add(File_raw[i]);
            }


            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, x, y + Window.TopSize, SizeX, 30);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, x + 1, y + Window.TopSize + 1, SizeX - 2, 28);
            string Disp_path = TrimStringFromLeft(Path, SizeX - 30);
            GUI.MainCanvas.DrawString(Disp_path, GUI.FontDefault, GUI.colors.ColorText, x + 2, y + Window.TopSize + 7);
            Positions(x, y + 60, SizeX, SizeY - 60, Folders , Files, 0);
            

        }

        public static void Positions(int startX, int startY, int width, int height, List<string> folders, List<string> files, int pageNo)
        {
            const int iconWidth = 70;
            const int iconHeight = 86;

            int itemsPerRow = width / iconWidth;
            if (itemsPerRow <= 0) itemsPerRow = 1;

            int itemsPerCol = height / iconHeight;
            if (itemsPerCol <= 0) itemsPerCol = 1;

            int itemsPerPage = itemsPerRow * itemsPerCol;

            // Combine folders and files into one list with folder priority
            var allItems = folders.Select(f => (f, true))   // true = isFolder
                           .Concat(files.Select(f => (f, false)))
                           .ToList();

            int startIndex = pageNo * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, allItems.Count);

            int indexOnPage = 0;

            for (int i = startIndex; i < endIndex; i++)
            {
                var (name, isFolder) = allItems[i];
                int row = indexOnPage / itemsPerRow;
                int col = indexOnPage % itemsPerRow;

                int posX = startX + col * iconWidth;
                int posY = startY + row * iconHeight;

                if (isFolder)
                    DrawFolder(posX, posY, name);
                else
                { DrawFile(posX, posY, name); }
                   

                indexOnPage++;
            }
        }

        public static void DrawFolder(int x, int y, string name)
        {
            GUI.MainCanvas.DrawImageAlpha(Folder, x+10, y+15);
            GUI.MainCanvas.DrawString(TrimStringFromRight(name, 70),GUI.FontDefault, GUI.colors.ColorText, x, y + 70);

        }

        public static void DrawFile(int x, int y, string name)
        {
            GUI.MainCanvas.DrawImageAlpha(File, x + 10, y + 15);
            GUI.MainCanvas.DrawString(TrimStringFromRight(name, 70), GUI.FontDefault, GUI.colors.ColorText, x, y + 70);

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

        public static string TrimStringFromRight(string input, int maxPixels)
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

            string trimmed = input.Substring(0, visibleChars);
            return trimmed + ellipsis;
        }


    }

}