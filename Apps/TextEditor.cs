using Cosmos.System;
using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        int prefixWidth = 4;

        List<(string line, int logicalIndex)> cachedWrappedLines = new List<(string, int)>();
        string lastWrappedHash = "";

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
            x = WindowData.WinPos.X;
            y = WindowData.WinPos.Y;
            SizeX = WindowData.WinPos.Width;
            SizeY = WindowData.WinPos.Height;

            // Scroll responsiveness config
            int linesPerScroll = 3;

            int delta = MouseManager.ScrollDelta;
            if (delta != 0)
            {
                ScrollOffset -= delta * linesPerScroll; // invert if needed depending on scroll direction
                int contentHeight = GetWrappedLineCount() * 16;
                int maxScroll = Math.Max(0, (contentHeight - (SizeY - Window.TopSize)) / 16);
                ScrollOffset = Math.Clamp(ScrollOffset, 0, maxScroll);

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

        private List<(string line, int logicalIndex)> GetWrappedLines()
        {
            var hash = ComputeHash(Lines);
            if (hash == lastWrappedHash)
                return cachedWrappedLines;

            int charsPerLine = (SizeX - 44) / 8;
            List<(string line, int logicalIndex)> wrappedLines = new List<(string, int)>();

            for (int i = 0; i < Lines.Count; i++)
            {
                string prefix = $"{i + 1}) ";
                string content = Lines[i];
                int lineStart = 0;

                string firstSegment = content.Substring(0, Math.Min(charsPerLine - prefix.Length, content.Length));
                wrappedLines.Add((prefix + firstSegment, i));
                lineStart += firstSegment.Length;

                while (lineStart < content.Length)
                {
                    int remaining = content.Length - lineStart;
                    int take = Math.Min(charsPerLine, remaining);
                    string segment = content.Substring(lineStart, take);
                    wrappedLines.Add(("".PadLeft(prefix.Length) + segment, i));
                    lineStart += take;
                }
            }

            lastWrappedHash = hash;
            cachedWrappedLines = wrappedLines;
            return wrappedLines;
        }

        public void DrawFileEditor()
        {
            int contentX = x + 40;
            int contentY = y + Window.TopSize;
            int maxVisibleLines = (SizeY - Window.TopSize) / 16;

            var wrappedLines = GetWrappedLines();

            int drawY = contentY;
            int visibleLines = 0;

            for (int i = ScrollOffset; i < wrappedLines.Count && visibleLines < maxVisibleLines; i++)
            {
                var (text, logicalLineIndex) = wrappedLines[i];

                if (logicalLineIndex == CursorY && CursorVisible)
                {
                    int cursorOffset = CursorX;
                    int charCount = 0;
                    foreach (var (lineText, lineIdx) in wrappedLines)
                    {
                        if (lineIdx != CursorY) continue;

                        string prefix = $"{CursorY + 1}) ";
                        string pureLineText = lineText.Substring(prefix.Length);
                        if (cursorOffset <= charCount + pureLineText.Length)
                        {
                            int localPos = cursorOffset - charCount + prefix.Length;
                            if (localPos >= 0 && localPos <= lineText.Length)
                                text = lineText.Insert(localPos, "_");
                            break;
                        }
                        charCount += pureLineText.Length;
                    }
                }

                GUI.MainCanvas.DrawString(text, GUI.FontDefault, Color.White, contentX, drawY);
                drawY += 16;
                visibleLines++;
            }

            // Draw scrollbar if needed
            int totalHeight = wrappedLines.Count * 16;
            int viewHeight = SizeY - Window.TopSize;
            if (totalHeight > viewHeight)
            {
                float ratio = viewHeight / (float)totalHeight;
                int scrollbarHeight = Math.Max(10, (int)(viewHeight * ratio));
                int scrollableHeight = totalHeight - viewHeight;
                int scrollbarY = y + Window.TopSize + (int)((ScrollOffset * 16f / scrollableHeight) * (viewHeight - scrollbarHeight));

                scrollbarY = Math.Clamp(scrollbarY, y + Window.TopSize, y + SizeY - scrollbarHeight);
                GUI.MainCanvas.DrawFilledRectangle(Color.Gray, x + SizeX - 4, scrollbarY, 2, scrollbarHeight);
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


        public void DrawNoFile()
        {
            GUI.MainCanvas.DrawString("File (" + Path + ") was not found or doesn't exist", GUI.FontDefault, Color.Red, x, y + Window.TopSize);
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
            LastContentHash = ComputeHash(Lines);
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