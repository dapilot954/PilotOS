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
            int drawY = y + Window.TopSize;
            for (int i = 0; i < Lines.Count && drawY + 16 < y + SizeY; i++)
            {
                string lineToDraw = Lines[i];
                if (i == CursorY && CursorVisible && CursorX <= lineToDraw.Length)
                {
                    lineToDraw = lineToDraw.Insert(CursorX, "_");
                }
                GUI.MainCanvas.DrawString(lineToDraw, GUI.FontDefault, Color.White, x + 4, drawY);
                drawY += 16;
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
