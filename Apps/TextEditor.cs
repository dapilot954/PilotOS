using Cosmos.System;
using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PilotOS.Apps
{
    public class TextEditor : Process
    {
        public string Path = "";
        public string Screen = "";
        public int x, y, SizeX, SizeY;
        public List<string> Lines = new List<string>();
        public int CursorX = 0, CursorY = 0;
        public bool CursorVisible = true;
        public DateTime LastBlink = DateTime.Now;
        public DateTime LastSave = DateTime.Now;
        public bool Changed = false;
        public DateTime LastChecked = DateTime.Now;
        public DateTime LastModified = DateTime.MinValue;
        public string LastContentHash;
        public int ScrollOffset = 0; // vertical scroll position
        int lineNumberWidth = 5;
        int prefixWidth = 4;
        int localPos;


        public override void Start()
        {
            Path = WindowData.args;

            if (File.Exists(Path))
            {
                Lines = new List<string>(File.ReadAllLines(Path));
                LastContentHash = ComputeHash(Lines);
                Screen = "Editing";
            }
            else
            {
                Screen = "NoFile";
            }
        }

        private string ComputeHash(List<string> content)
        {
            int hash = 17;
            foreach (var line in content)
            {
                hash = hash * 31 + line.GetHashCode();
            }
            return hash.ToString();
        }



        public override void Run()
        {
            localPos = CursorX + prefixWidth;
            x = WindowData.WinPos.X;
            y = WindowData.WinPos.Y;
            SizeX = WindowData.WinPos.Width;
            SizeY = WindowData.WinPos.Height;

            int delta = MouseManager.ScrollDelta;

            if (delta != 0)
            {
                if (delta > 0 && ScrollOffset > 0)
                {
                    ScrollOffset -= 1; // scroll up
                }
                else if (delta < 0)
                {
                    ScrollOffset += 1; // scroll down
                }

                // Clamp ScrollOffset
                int contentHeight = GetWrappedLineCount() * 16;
                int maxScroll = Math.Max(0, (contentHeight - (SizeY - Window.TopSize)) / 16);
                ScrollOffset = Math.Clamp(ScrollOffset, 0, maxScroll);

                // Reset delta after handling it
                MouseManager.ResetScrollDelta();
            }


            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);

            if (!File.Exists(Path))
            {
                Screen = "NoFile";
                Lines.Clear();
                DrawNoFile();
                return;
            }

            // Check for external changes every 2 seconds
            if ((DateTime.Now - LastChecked).TotalSeconds >= 2)
            {
                LastChecked = DateTime.Now;
                var currentLines = new List<string>(File.ReadAllLines(Path));
                var currentHash = ComputeHash(currentLines);

                if (currentHash != LastContentHash)
                {
                    Lines = currentLines;
                    LastContentHash = currentHash;
                    CursorX = 0;
                    CursorY = 0;
                }
            }

            if (Screen == "Editing")
            {
                DrawFileEditor();

                if ((DateTime.Now - LastBlink).TotalMilliseconds > 500)
                {
                    CursorVisible = !CursorVisible;
                    LastBlink = DateTime.Now;
                }
            }
        }

        private int GetWrappedLineCount()
        {
            int charsPerLine = (SizeX - 44) / 8;
            int count = 0;

            for (int i = 0; i < Lines.Count; i++)
            {
                string content = Lines[i];
                int lineCount = 1 + (int)Math.Ceiling((content.Length - Math.Max(0, charsPerLine - 4)) / (float)charsPerLine);
                count += lineCount;
            }

            return count;
        }




        public override void OnKeyPressed(KeyEvent key)
        {
            if (Screen != "Editing") return;

            if (Lines.Count == 0) Lines.Add("");

            string currentLine = Lines[CursorY];

            bool modified = false;

            if (key.Key == ConsoleKeyEx.Enter)
            {
                string newLine = currentLine.Substring(CursorX);
                Lines[CursorY] = currentLine.Substring(0, CursorX);
                Lines.Insert(CursorY + 1, newLine);
                CursorY++;
                CursorX = 0;
                modified = true;
            }
            else if (key.Key == ConsoleKeyEx.Backspace)
            {
                if (CursorX > 0)
                {
                    Lines[CursorY] = currentLine.Remove(CursorX - 1, 1);
                    CursorX--;
                    modified = true;
                }
                else if (CursorY > 0)
                {
                    CursorX = Lines[CursorY - 1].Length;
                    Lines[CursorY - 1] += Lines[CursorY];
                    Lines.RemoveAt(CursorY);
                    CursorY--;
                    modified = true;
                }
            }
            else if (key.Key == ConsoleKeyEx.LeftArrow)
            {
                if (CursorX > 0) CursorX--;
                else if (CursorY > 0)
                {
                    CursorY--;
                    CursorX = Lines[CursorY].Length;
                }
            }
            else if (key.Key == ConsoleKeyEx.RightArrow)
            {
                if (CursorX < currentLine.Length) CursorX++;
                else if (CursorY < Lines.Count - 1)
                {
                    CursorY++;
                    CursorX = 0;
                }
            }
            else if (key.Key == ConsoleKeyEx.UpArrow)
            {
                if (CursorY > 0) CursorY--;
                CursorX = Math.Min(CursorX, Lines[CursorY].Length);
            }
            else if (key.Key == ConsoleKeyEx.DownArrow)
            {
                if (CursorY < Lines.Count - 1) CursorY++;
                CursorX = Math.Min(CursorX, Lines[CursorY].Length);
            }
            else if (key.KeyChar != '\0')
            {
                Lines[CursorY] = currentLine.Insert(CursorX, key.KeyChar.ToString());
                CursorX++;
                modified = true;
            }

            if (modified)
            {
                SaveFile();
            }
        }

        private void SaveFile()
        {
            using (var writer = new StreamWriter(Path, false))
            {
                foreach (var line in Lines)
                {
                    writer.WriteLine(line);
                }
            }
            LastContentHash = ComputeHash(Lines); // Update hash after saving
        }





        public void DrawNoFile()
        {
            GUI.MainCanvas.DrawString("File (" + Path + ") was not found or doesn't exist", GUI.FontDefault, Color.Red, x, y + Window.TopSize);
        }

        public void DrawFileEditor()
        {
            int contentX = x + 40; // Leave space for line numbers (5 digits = 40px)
            int contentY = y + Window.TopSize;
            int charsPerLine = (SizeX - 44) / 8;
            int maxVisibleLines = (SizeY - Window.TopSize) / 16;

            List<(string line, int logicalIndex)> wrappedLines = new List<(string, int)>();

            for (int i = 0; i < Lines.Count; i++)
            {
                string prefix = $"{i + 1}) ";
                string content = Lines[i];
                int lineStart = 0;

                // First wrapped line includes the prefix
                string firstSegment = content.Substring(0, Math.Min(charsPerLine - prefix.Length, content.Length));
                wrappedLines.Add((prefix + firstSegment, i));

                lineStart += firstSegment.Length;

                // Any additional wrapped lines
                while (lineStart < content.Length)
                {
                    int remaining = content.Length - lineStart;
                    int take = Math.Min(charsPerLine, remaining);
                    string segment = content.Substring(lineStart, take);
                    wrappedLines.Add(("".PadLeft(prefix.Length) + segment, i)); // align with line numbers
                    lineStart += take;
                }
            }

            int drawY = contentY;
            int cursorDrawnY = -1;
            int visibleLines = 0;

            for (int i = ScrollOffset; i < wrappedLines.Count && visibleLines < maxVisibleLines; i++)
            {
                var (text, logicalLineIndex) = wrappedLines[i];

                // Insert cursor (blinking underscore)
                if (logicalLineIndex == CursorY && CursorVisible)
                {
                    int totalChars = 0;
                    foreach (var (lineText, idx) in wrappedLines)
                    {
                        if (idx != CursorY) continue;

                        // Exclude the prefix for calculating position
                        string prefix = $"{CursorY + 1}) ";
                        int prefixLength = prefix.Length;

                        string pureLineText = lineText.Substring(prefixLength);
                        if (CursorX <= totalChars + pureLineText.Length)
                        {
                            int localPos = CursorX - totalChars + prefixLength;

                            if (localPos >= 0 && localPos <= lineText.Length)
                                text = lineText.Insert(localPos, "_");

                            break;
                        }
                        totalChars += pureLineText.Length;
                    }
                }



                GUI.MainCanvas.DrawString(text, GUI.FontDefault, Color.White, contentX, drawY);
                drawY += 16;
                visibleLines++;
            }

            // Optional: draw scrollbar
            int totalHeight = wrappedLines.Count * 16;
            int scrollHeight = (SizeY - Window.TopSize);
            if (totalHeight > scrollHeight)
            {
                int scrollbarHeight = Math.Max(10, (int)((scrollHeight * 1.0f) * (scrollHeight * 1.0f) / totalHeight));
                int scrollbarY = y + Window.TopSize + (int)((ScrollOffset * 16.0f) * scrollHeight / totalHeight);

                GUI.MainCanvas.DrawFilledRectangle(Color.Gray, x + SizeX - 4, scrollbarY, 2, scrollbarHeight);
            }
        }
        

        public void SaveToFile()
        {
            
            using (var stream = new StreamWriter(Path, false))
            {
                for (int i = 0; i < Lines.Count; i++)
                {
                    stream.WriteLine(Lines[i]);
                }
            }
        }

    }
}
