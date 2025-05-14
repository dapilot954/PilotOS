using Cosmos.System.Graphics;
using Cosmos.System;
using PilotOS.Graphics;
using PilotOS.Resources;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PilotOS.Apps
{
    public class FileExplorer : Process
    {
        public static Bitmap Folder;
        public static Bitmap File;
        public string Path = @"0:\";
        public static int scrollOffset = 0;
        private const int itemHeight = 30;
        private const int scrollSpeed = 10;

        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;

            var Directories = Directory.GetDirectories(Path).ToList();
            var Files = Directory.GetFiles(Path).ToList();

            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, x, y + Window.TopSize, SizeX, 30);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, x + 1, y + Window.TopSize + 1, SizeX - 2, 28);

            string Disp_path = TrimStringFromLeft(Path, SizeX - 30);
            GUI.MainCanvas.DrawString(Disp_path, GUI.FontDefault, GUI.colors.ColorText, x + 2, y + Window.TopSize + 7);

            HandleScrollWheel();

            DrawList(x, y + Window.TopSize + 35, SizeX, SizeY - Window.TopSize - 35, Directories, Files);
        }

        private void HandleScrollWheel()
        {
            int delta = MouseManager.ScrollDelta;
            if (delta != 0)
            {
                scrollOffset -= delta * scrollSpeed;
                if (scrollOffset < 0) scrollOffset = 0;
                // Add a max scrollOffset clamp if needed.
                MouseManager.ResetScrollDelta();
            }
        }

        private void DrawList(int startX, int startY, int width, int height, List<string> folders, List<string> files)
        {
            var allItems = folders.Select(f => (f, true)).Concat(files.Select(f => (f, false))).ToList();

            int maxVisibleItems = height / itemHeight;
            int totalHeight = allItems.Count * itemHeight;
            int maxOffset = Math.Max(0, totalHeight - height);
            if (scrollOffset > maxOffset) scrollOffset = maxOffset;

            int yOffset = -scrollOffset;

            for (int i = 0; i < allItems.Count; i++)
            {
                int itemY = startY + (i * itemHeight) + yOffset;
                if (itemY + itemHeight < startY || itemY > startY + height)
                    continue; // Skip drawing outside visible area

                var (name, isFolder) = allItems[i];

                string itemName = name.Substring(name.LastIndexOf('\\') + 1);

                // Icon position (left-aligned)
                int iconX = startX + 4;
                int iconY = itemY + (itemHeight - 25) / 2;

                // Text position (to the right of icon)
                int textX = iconX + 25 + 5;
                int textY = itemY + (itemHeight - 16) / 2; // roughly center vertically

                GUI.MainCanvas.DrawString(
                    TrimStringFromRight(itemName, width - (textX - startX) - 5),
                    GUI.FontDefault,
                    GUI.colors.ColorText,
                    textX,
                    textY
                );

                if (isFolder)
                    GUI.MainCanvas.DrawImageAlpha(Folder, iconX, iconY);
                else
                    GUI.MainCanvas.DrawImageAlpha(File, iconX, iconY);
            }

            DrawScrollBar(startX + width - 6, startY, 6, height, totalHeight);
        }


        private void DrawScrollBar(int x, int y, int width, int height, int contentHeight)
        {
            if (contentHeight <= height) return;

            int barHeight = Math.Max((height * height) / contentHeight, 20);
            int barY = y + (scrollOffset * height) / contentHeight;

            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, x, y, width, height);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, x, barY, width, barHeight);
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

            int visibleChars = maxChars - ellipsis.Length;
            if (visibleChars <= 0)
                return ellipsis.Substring(0, maxChars);

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

            int visibleChars = maxChars - ellipsis.Length;
            if (visibleChars <= 0)
                return ellipsis.Substring(0, maxChars);

            string trimmed = input.Substring(0, visibleChars);
            return trimmed + ellipsis;
        }
    }
}
