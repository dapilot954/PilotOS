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
        private string selectedItem = null;
        private string lastClickedItem = null;
        private DateTime lastClickTime = DateTime.MinValue;
        private bool mousePreviouslyDown = false;
        public static Bitmap Folder;
        public static Bitmap FileIcon;
        public static Bitmap BackIcon;
        public static Bitmap AddIcon;
        public string Path = @"0:\";
        public static int scrollOffset = 0;
        private const int itemHeight = 30;
        private const int scrollSpeed = 10;

        private bool showPopup = false;
        private string newFileName = "";
        private bool waitingForInput = false;

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



            // Back icon position
            int backIconSize = 26;
            int backIconX = x + SizeX - backIconSize - 2;
            int backIconY = y + Window.TopSize + 2;

            // Draw the back icon
            GUI.MainCanvas.DrawImageAlpha(BackIcon, backIconX, backIconY);

            // Handle back icon click
            if (!showPopup)
            {



                if (MouseManager.MouseState == MouseState.Left)
                {
                    int mx = (int)MouseManager.X;
                    int my = (int)MouseManager.Y;

                    if (!mousePreviouslyDown && mx >= backIconX && mx <= backIconX + backIconSize &&
                        my >= backIconY && my <= backIconY + backIconSize)
                    {
                        mousePreviouslyDown = true;

                        if (Path != @"0:\")
                        {
                            // Trim trailing slash if present
                            if (Path.EndsWith("\\"))
                                Path = Path.Substring(0, Path.Length - 1);

                            int lastSlash = Path.LastIndexOf('\\');
                            if (lastSlash >= 0)
                                Path = Path.Substring(0, lastSlash);

                            if (!Path.EndsWith("\\"))
                                Path += "\\";

                            scrollOffset = 0;
                        }
                    }
                }
            }

            int addIconX = backIconX - 26 - 4; // 4px gap from back icon
            int addIconY = backIconY;
            GUI.MainCanvas.DrawImageAlpha(AddIcon, addIconX, addIconY);

            if (showPopup)
            {
                DrawPopup(x + (SizeX / 2) - 100, y + (SizeY / 2) - 40, 200, 80);
            }

            if (WindowData.selected)
            {
                // Draw Add icon


                if (MouseManager.MouseState == MouseState.Left)
                {
                    int mx = (int)MouseManager.X;
                    int my = (int)MouseManager.Y;

                    if (!mousePreviouslyDown && mx >= addIconX && mx <= addIconX + 26 &&
                        my >= addIconY && my <= addIconY + 26)
                    {
                        mousePreviouslyDown = true;
                        showPopup = true;
                        newFileName = "";
                        waitingForInput = true;
                    }
                }

                

                HandleMouseClick(x, y + Window.TopSize + 35, SizeX, SizeY - Window.TopSize - 35, Directories, Files);
                HandleTextInput();
            }


            HandleScrollWheel();

            DrawList(x, y + Window.TopSize + 35, SizeX, SizeY - Window.TopSize - 35, Directories, Files);


            if (WindowData.selected)
            {
                if (!showPopup)
                {
                    HandleMouseClick(x, y + Window.TopSize + 35, SizeX, SizeY - Window.TopSize - 35, Directories, Files);
                }

                
            }

            



        }

        private void DrawPopup(int px, int py, int w, int h)
        {
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorText, px, py, w, h);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, px + 2, py + 2, w - 4, h - 4);

            GUI.MainCanvas.DrawString("New file name:", GUI.FontDefault, GUI.colors.ColorText, px + 10, py + 10);
            GUI.MainCanvas.DrawString(newFileName + "_", GUI.FontDefault, GUI.colors.ColorText, px + 10, py + 30);

            // Buttons
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, px + 10, py + 55, 60, 18);
            GUI.MainCanvas.DrawString("Done", GUI.FontDefault, GUI.colors.ColorText, px + 20, py + 58);

            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorExplorer, px + w - 70, py + 55, 60, 18);
            GUI.MainCanvas.DrawString("Cancel", GUI.FontDefault, GUI.colors.ColorText, px + w - 60, py + 58);

            // Handle mouse clicks on buttons
            int mx = (int)MouseManager.X;
            int my = (int)MouseManager.Y;

            if (MouseManager.MouseState == MouseState.Left && !mousePreviouslyDown)
            {
                if (mx >= px + 10 && mx <= px + 70 && my >= py + 55 && my <= py + 73)
                {
                    // Done clicked
                    if (!string.IsNullOrWhiteSpace(newFileName))
                    {
                        string newFilePath = Path;
                        if (!newFilePath.EndsWith("\\")) newFilePath += "\\";
                        newFilePath += newFileName;

                        try
                        {
                            File.WriteAllText(newFilePath, "");
                        }
                        catch { }

                    }

                    showPopup = false;
                    waitingForInput = false;
                    newFileName = "";
                    mousePreviouslyDown = true;
                }
                else if (mx >= px + w - 70 && mx <= px + w - 10 && my >= py + 55 && my <= py + 73)
                {
                    // Cancel clicked
                    showPopup = false;
                    waitingForInput = false;
                    newFileName = "";
                    mousePreviouslyDown = true;
                }
            }
        }

        private void HandleTextInput()
        {
            if (!waitingForInput || !WindowData.selected)
                return;

            if (Cosmos.System.KeyboardManager.TryReadKey(out var key))
            {
                if (key.Key == ConsoleKeyEx.Backspace)
                {
                    if (newFileName.Length > 0)
                        newFileName = newFileName.Remove(newFileName.Length - 1);
                }
                else if (key.Key == ConsoleKeyEx.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(newFileName))
                    {
                        string newFilePath = Path;
                        if (!newFilePath.EndsWith("\\")) newFilePath += "\\";
                        newFilePath += newFileName;

                        try
                        {
                            File.WriteAllText(newFilePath, "");
                        }
                        catch { }

                    }

                    showPopup = false;
                    waitingForInput = false;
                    newFileName = "";
                }
                else
                {
                    newFileName += key.KeyChar;
                }
            }
        }

        private void HandleMouseClick(int startX, int startY, int width, int height, List<string> folders, List<string> files)
        {
            bool mouseDown = MouseManager.MouseState == MouseState.Left;
            if (!mouseDown)
            {
                mousePreviouslyDown = false;
                return;
            }

            // Prevent holding click
            if (mousePreviouslyDown)
                return;

            mousePreviouslyDown = true;

            int mouseX = (int)MouseManager.X;
            int mouseY = (int)MouseManager.Y;

            if (mouseX < startX || mouseX > startX + width || mouseY < startY || mouseY > startY + height)
                return;

            var allItems = folders.Select(f => (f, true)).Concat(files.Select(f => (f, false))).ToList();

            int yOffset = -scrollOffset;
            bool itemClicked = false;

            for (int i = 0; i < allItems.Count; i++)
            {
                int itemY = startY + (i * itemHeight) + yOffset;
                if (itemY + itemHeight < startY || itemY > startY + height)
                    continue;

                if (mouseY >= itemY && mouseY <= itemY + itemHeight)
                {
                    var (item, isFolder) = allItems[i];
                    selectedItem = item;
                    itemClicked = true;

                    TimeSpan timeSinceLastClick = DateTime.Now - lastClickTime;
                    bool doubleClicked = item == lastClickedItem && timeSinceLastClick.TotalMilliseconds < 500;

                    lastClickedItem = item;
                    lastClickTime = DateTime.Now;

                    if (doubleClicked)
                    {
                        if (isFolder)
                        {
                            string itemName = item.Substring(item.LastIndexOf('\\') + 1);
                            if (!Path.EndsWith("\\"))
                                Path += "\\";
                            Path += itemName + "\\";

                            scrollOffset = 0;
                        }
                        else
                        {
                            // Later: Add code to "open" files
                        }
                    }

                    break;
                }
            }

            // Deselect if clicked on empty area
            if (!itemClicked)
            {
                selectedItem = null;
            }
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

            int totalHeight = allItems.Count * itemHeight;
            int maxOffset = Math.Max(0, totalHeight - height);
            if (scrollOffset > maxOffset) scrollOffset = maxOffset;

            int yOffset = -scrollOffset;

            for (int i = 0; i < allItems.Count; i++)
            {
                int itemY = startY + (i * itemHeight) + yOffset;

                // Skip items outside the visible window bounds
                if (itemY + itemHeight < startY || itemY > startY + height)
                    continue;

                var (name, isFolder) = allItems[i];

                string itemName = name.Substring(name.LastIndexOf('\\') + 1);

                // Icon and text positions
                int iconX = startX + 4;
                int iconY = itemY + (itemHeight - 25) / 2;

                int textX = iconX + 25 + 5;
                int textY = itemY + (itemHeight - 16) / 2;

                if (name == selectedItem)
                {
                    GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorSelected, startX + 1, itemY + 1, width - 8, itemHeight - 2);
                }


                // Draw icon (only if vertically visible)
                if (iconY >= startY && iconY + 25 <= startY + height)
                {
                    if (isFolder)
                        GUI.MainCanvas.DrawImageAlpha(Folder, iconX, iconY);
                    else
                        GUI.MainCanvas.DrawImageAlpha(FileIcon, iconX, iconY);
                }

                // Draw text (only if vertically visible)
                if (itemY >= startY && itemY + itemHeight <= startY + height)
                {
                    GUI.MainCanvas.DrawString(
                        TrimStringFromRight(itemName, width - (textX - startX) - 10),
                        GUI.FontDefault,
                        GUI.colors.ColorText,
                        textX,
                        textY
                    );
                }
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
