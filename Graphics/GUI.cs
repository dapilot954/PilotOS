﻿using Cosmos.System;
using Cosmos.System.Graphics;
using PilotOS.Apps;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Cosmos.System.Graphics.Fonts;
using System.Xml;
using Console = System.Console;
using Cosmos.HAL.Drivers.Video;

namespace PilotOS.Graphics
{
    public static class GUI
    {

        public static int ScreenSizeX = 1920, ScreenSizeY = 1080;
        public static SVGAIICanvas MainCanvas;
        public static Bitmap Wallpaper, Cursor;
        public static Colors colors = new Colors();
        public static bool Clicked;
        public static Process CurrentProcess;
        public static int MX, MY;
        static int oldX, oldY, oldWRS, oldHRS, OldXRS, OldYRS;
        static int CX, CY;
        static int ClickUpdate;
        public static PCScreenFont FontDefault = PCScreenFont.Default;
        public static bool moving, resizing;
        public static int resizeX, resizeY;
        public static void Update()
        {
            MX = (int)MouseManager.X;
            MY = (int)MouseManager.Y;
            ProcessManager.Update();
            MainCanvas.DrawImage(Wallpaper, 0, 0);
            MainCanvas.DrawFilledCircle(Color.Red, 1890, 30, 20);

            Move();
            Resize();
            PowerButton();
            Selector();
            ProcessManager.Update();
            MainCanvas.DrawImageAlpha(Cursor, (int)MouseManager.X, (int)MouseManager.Y);
            if (CurrentProcess != null)
            {
                ProcessManager.ProcessList.Remove(CurrentProcess);
                ProcessManager.ProcessList.Add(CurrentProcess);
            }

            if (KeyboardManager.TryReadKey(out var keyEvent))
            {
                foreach (var process in ProcessManager.ProcessList)
                {
                    if (process.WindowData.selected == true)
                    {
                        process.OnKeyPressed(keyEvent);
                    }
                }
                
                    
                
            }

            if (MouseManager.MouseState == MouseState.Left)
            {
                Clicked = true;
                if (ClickUpdate == 0)
                {
                    CX = MX;
                    CY = MY;
                }
                ClickUpdate++;
            }
            else if (MouseManager.MouseState == MouseState.None && Clicked)
            {
                Clicked = false;
                CurrentProcess = null;
                ClickUpdate = 0;


            }
            MainCanvas.Display();

        }
        public static void Selector()
        {
            foreach (var proc in ProcessManager.ProcessList)
            {
                if (proc == ProcessManager.ProcessList[ProcessManager.ProcessList.Count - 1])
                {
                    proc.WindowData.selected = true;
                }
                else
                {
                    proc.WindowData.selected = false;
                }
            }
        }


        public static void Move()
        {
            if (CurrentProcess != null && moving)
            {
                CurrentProcess.WindowData.WinPos.X = MX - oldX;
                CurrentProcess.WindowData.WinPos.Y = MY - oldY;
                return;
            }

            if (MouseManager.MouseState == MouseState.Left)
            {
                for (int i = ProcessManager.ProcessList.Count - 1; i >= 0; i--)
                {
                    var proc = ProcessManager.ProcessList[i];
                    if (!proc.WindowData.MoveAble) continue;

                    var win = proc.WindowData.WinPos;

                    // Check if mouse is inside the top bar area of this process
                    if (MX >= win.X && MX < win.X + win.Width - 30 &&
                        MY >= win.Y && MY < win.Y + Window.TopSize)
                    {
                        bool covered = false;

                        // Check if the mouse position is covered by any higher (above in z-order) process
                        for (int j = ProcessManager.ProcessList.Count - 1; j > i; j--)
                        {
                            var other = ProcessManager.ProcessList[j];
                            var otherWin = other.WindowData.WinPos;

                            if (MX >= otherWin.X && MX < otherWin.X + otherWin.Width &&
                                MY >= otherWin.Y && MY < otherWin.Y + otherWin.Height)
                            {
                                covered = true;
                                break;
                            }
                        }

                        if (!covered)
                        {
                            CurrentProcess = proc;
                            oldX = MX - win.X;
                            oldY = MY - win.Y;
                            moving = true;
                            resizing = false;
                            break;
                        }
                    }
                }
            }
        }




        // Define minimum size constants
        const int MIN_WIDTH = 200;  // Minimum allowed width
        const int MIN_HEIGHT = 200; // Minimum allowed height

        public static void Resize()
        {
            if (CurrentProcess != null)
            {
                if (resizing == true)
                {
                    // Calculate new size based on mouse movement
                    resizeX = (int)MouseManager.X - (OldXRS + oldWRS);
                    resizeY = (int)MouseManager.Y - (OldYRS + oldHRS);

                    // Apply minimum size constraints
                    int newWidth = Math.Max(oldWRS + resizeX, MIN_WIDTH);
                    int newHeight = Math.Max(oldHRS + resizeY, MIN_HEIGHT);

                    // Set new dimensions
                    CurrentProcess.WindowData.WinPos.Width = newWidth;
                    CurrentProcess.WindowData.WinPos.Height = newHeight;
                }
            }
            else if (MouseManager.MouseState == MouseState.Left)
            {
                foreach (var proc in ProcessManager.ProcessList)
                {
                    if (!proc.WindowData.MoveAble)
                    {
                        continue;
                    }

                    if (MX > proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width - 30 &&
                        MX < proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width)
                    {
                        if (MY > proc.WindowData.WinPos.Height + proc.WindowData.WinPos.Y - 30 &&
                            MY < proc.WindowData.WinPos.Height + proc.WindowData.WinPos.Y)
                        {
                            if (CX > proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width - 30 &&
                                CX < proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width)
                            {
                                if (CY > proc.WindowData.WinPos.Height + proc.WindowData.WinPos.Y - 30 &&
                                    CY < proc.WindowData.WinPos.Height + proc.WindowData.WinPos.Y)
                                {
                                    // Start resizing
                                    CurrentProcess = proc;

                                    OldXRS = proc.WindowData.WinPos.X;
                                    OldYRS = proc.WindowData.WinPos.Y;

                                    oldWRS = proc.WindowData.WinPos.Width;
                                    oldHRS = proc.WindowData.WinPos.Height;
                                    resizing = true;
                                    moving = false;
                                }
                            }
                        }
                    }

                }
            }
        }
        public static void StartGUI()
        {
            MainCanvas = new SVGAIICanvas(new Mode((uint)ScreenSizeX, (uint)ScreenSizeY, ColorDepth.ColorDepth32));
            MouseManager.ScreenWidth = (uint)ScreenSizeX;
            MouseManager.ScreenHeight = (uint)ScreenSizeY;
            MouseManager.X = (uint)ScreenSizeX / 2;
            MouseManager.Y = (uint)ScreenSizeY / 2;

            ProcessManager.start(new FileExplorer { WindowData = new WindowData { WinPos = new Rectangle(100, 100, 700, 700) }, Name = "File Explorer" });
            ProcessManager.start(new Terminal { WindowData = new WindowData { WinPos = new Rectangle(100, 100, 700, 700), args = "" }, Name = "Terminal" });

        }



        public static void PowerButton()
        {
            if (Clicked == true)
            {
                if (CX > 1890 - 20 && CX < 1890 + 20)
                {
                    if (CY > 30 - 20 && CY < 30 + 20)
                    {

                        Power.Reboot();
                    }
                }
            }
        }


    }
}