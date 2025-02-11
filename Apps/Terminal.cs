using PilotOS.Graphics;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.X509Certificates;
using Cosmos.HAL.BlockDevice.Registers;
using System.Runtime.InteropServices;
using System.Drawing;
using Cosmos.System;
using Cosmos.System.Keyboard;
using PilotOS.Apps.TerminalAssets;

namespace PilotOS.Apps
{
    public class Terminal : Process
    {
        public static List<string> WrapStrings(List<string> texts, int maxChars)
        {
            List<string> wrappedLines = new List<string>();

            foreach (var text in texts)
            {
                string[] words = text.Split(' ');
                string currentLine = "";

                foreach (var word in words)
                {
                    // Check if adding the word would exceed the limit
                    if ((currentLine + word).Length > maxChars)
                    {
                        wrappedLines.Add(currentLine.Trim());
                        currentLine = word + " "; // Start a new line with the current word
                    }
                    else
                    {
                        currentLine += word + " ";
                    }
                }

                // Add the last line if there's remaining text
                //if (!string.IsNullOrWhiteSpace(currentLine))
              //  {
                    wrappedLines.Add(currentLine.Trim());
                //}
            }

            return wrappedLines;
        }
        public static void EnsureLastElement(List<string> list, string lastElement)
        {
            if (list.Contains(lastElement))
            {
                // Remove the existing instance
                list.Remove(lastElement);
            }

            // Add it back at the end
            list.Add(lastElement);
        }

        static List<string> lines = new List<string>();
        static List<string> lines_perm = new List<string>();

        public static void print(string text)
        {
            lines.Add(text);
        }
        public static void print_perm(string text)
        {
            lines_perm.Add(text);
        }
        public static void clear()
        {
            lines_perm.Clear();
        }

        public static string input = "";
        public static bool execute = false;
        public override void Run()
        {
            int x = WindowData.WinPos.X;
            int y = WindowData.WinPos.Y;
            int SizeX = WindowData.WinPos.Width;
            int SizeY = WindowData.WinPos.Height;
            string commandline = Kernel.Path + input;
            Window.DrawTop(this);
            GUI.MainCanvas.DrawFilledRectangle(GUI.colors.ColorMain, x, y + Window.TopSize, SizeX, SizeY - Window.TopSize);

            lines.Clear();

            print("PilotOS Terminal version 0.1");
            
            if (WindowData.selected == true)
            {
                if (Cosmos.System.KeyboardManager.TryReadKey(out var key))
                {

                    //input += key.KeyChar + "";

                    if (key.Key == ConsoleKeyEx.Backspace)
                    {
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
            else if (WindowData.selected == false)
            {
                if (Cosmos.System.KeyboardManager.TryReadKey(out var key))
                {

                }
            }

            if (execute)
            {
                print_perm(Kernel.Path + input);
                TerminalCommandRunner.commandrun(input);
                print_perm("");
                input = "";
                execute = false;
                
            }


            int i = 0;
            while (true)
            {
                if (i == lines_perm.Count) { break; }
                else
                {
                    print(lines_perm[i]);
                    i++;
                }
            }


            decimal roundedvar = Math.Floor((decimal)SizeX / 8);
            decimal hightvar = Math.Floor(((decimal)SizeY - Window.TopSize) / 16);

            int b = 0;
            int a = 0;
            EnsureLastElement(lines, commandline);
            lines = WrapStrings(lines, (int)roundedvar);
            List<string> up_lines = lines;
            List<string> lines_to_write = new List<string>();



            MoveLastLines(up_lines, lines_to_write, (int)hightvar);

            while (true)
            {
                if (a == lines_to_write.Count)
                { break; }
                else
                {
                    GUI.MainCanvas.DrawString(lines_to_write[a], GUI.FontDefault, GUI.colors.ColorText, x, y + b + Window.TopSize);
                    b += 16;
                    a++;
                }

            }

        }
        static void MoveLastLines(List<string> list1, List<string> list2, int x)
        {
            // Validate input
            if (x <= 0)
            {
                return;
            }

            if (list1 == null || list2 == null)
            {
                return;
            }

            if (x > list1.Count)
            {
                x = list1.Count; // Move all lines if x exceeds list size
            }

            // Get the last x lines
            List<string> linesToMove = list1.GetRange(list1.Count - x, x);

            // Add to List2
            list2.AddRange(linesToMove);

            // Remove from List1
            list1.RemoveRange(list1.Count - x, x);
        }


    }
}