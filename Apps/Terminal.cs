using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using Cosmos.System;
using Cosmos.System.Graphics;
using PilotOS.Apps.TerminalAssets;

namespace PilotOS.Apps
{
    public class Terminal : Process
    {
        private List<string> lines = new List<string>();
        private List<string> lines_perm = new List<string>();
        private string input = "";
        private bool execute = false;
        public string Path = @"0:\";

        public override void Start()
        {
            if (WindowData.args != "")
            {
                input = WindowData.args;
                execute = true;
            }
            
            
        }
        public static List<string> WrapStrings(List<string> texts, int maxChars)
        {
            List<string> wrappedLines = new List<string>();

            foreach (var text in texts)
            {
                string[] words = text.Split(' ');
                string currentLine = "";

                foreach (var word in words)
                {
                    if ((currentLine + word).Length > maxChars)
                    {
                        wrappedLines.Add(currentLine.Trim());
                        currentLine = word + " ";
                    }
                    else
                    {
                        currentLine += word + " ";
                    }
                }

                wrappedLines.Add(currentLine.Trim());
            }

            return wrappedLines;
        }

        public static void EnsureLastElement(List<string> list, string lastElement)
        {
            if (list.Contains(lastElement))
                list.Remove(lastElement);

            list.Add(lastElement);
        }

        public void print(string text)
        {
            lines.Add(text);
        }

        public void print_perm(string text)
        {
            lines_perm.Add(text);
        }

        public void clear()
        {
            lines_perm.Clear();
        }

        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;

            string commandline = Path + input;

            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);

            lines.Clear();
            print("PilotOS Terminal version 0.2");

            if (WindowData.selected)
            {
                if (Cosmos.System.KeyboardManager.TryReadKey(out var key))
                {
                    if (key.Key == ConsoleKeyEx.Backspace)
                    {
                        if (input.Length > 0)
                            input = input.Remove(input.Length - 1);
                    }
                    else if (key.Key == ConsoleKeyEx.Enter)
                    {
                        execute = true;
                    }
                    else
                    {
                        input += key.KeyChar;
                    }
                }
            }

            if (execute)
            {
                print_perm(Path + input);
                TerminalCommandRunner.commandrun(input, this);
                print_perm("");
                input = "";
                execute = false;
            }

            foreach (string permLine in lines_perm)
            {
                print(permLine);
            }

            decimal roundedvar = Math.Floor((decimal)SizeX / 8);
            decimal hightvar = Math.Floor(((decimal)SizeY - Window.TopSize) / 16);

            EnsureLastElement(lines, commandline);
            lines = WrapStrings(lines, (int)roundedvar);

            List<string> lines_to_write = new List<string>();
            MoveLastLines(lines, lines_to_write, (int)hightvar);

            int drawY = 0;
            foreach (string line in lines_to_write)
            {
                GUI.MainCanvas.DrawString(line, GUI.FontDefault, GUI.colors.ColorText, x, y + drawY + Window.TopSize);
                drawY += 16;
            }
        }

        private static void MoveLastLines(List<string> source, List<string> target, int count)
        {
            if (count <= 0 || source == null || target == null)
                return;

            if (count > source.Count)
                count = source.Count;

            List<string> lastLines = source.GetRange(source.Count - count, count);
            target.AddRange(lastLines);
        }
    }
}
