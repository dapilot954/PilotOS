using Cosmos.System;
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
        static int oldX, oldY;
        static int CX, CY;
        static int ClickUpdate;
        public static PCScreenFont FontDefault = PCScreenFont.Default;
        public static void Update()
        {
            MX = (int)MouseManager.X;
            MY = (int)MouseManager.Y;
            ProcessManager.Update();
            MainCanvas.DrawImage(Wallpaper, 0, 0);
            MainCanvas.DrawFilledCircle(Color.Red, 1890, 30, 20);
            Move();
            PowerButton();
            ProcessManager.Update();
            MainCanvas.DrawImageAlpha(Cursor, (int)MouseManager.X, (int)MouseManager.Y);

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
        

        public static void Move()
        {

            if (CurrentProcess != null)
            {
                CurrentProcess.WindowData.WinPos.X = (int)MouseManager.X - oldX;
                CurrentProcess.WindowData.WinPos.Y = (int)MouseManager.Y - oldY;

            }
            else if (MouseManager.MouseState == MouseState.Left)
            {
                foreach (var proc in ProcessManager.ProcessList)
                {
                    if (!proc.WindowData.MoveAble)
                    {
                        continue;
                    }
                    if (MX > proc.WindowData.WinPos.X && MX < proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width)
                    {
                        if (MY >  proc.WindowData.WinPos.Y && MY < proc.WindowData.WinPos.Y + Window.TopSize)
                        {
                            if (CX > proc.WindowData.WinPos.X && CX < proc.WindowData.WinPos.X + proc.WindowData.WinPos.Width)
                            {
                                if (CY > proc.WindowData.WinPos.Y && CY < proc.WindowData.WinPos.Y + Window.TopSize)
                                {


                                    CurrentProcess = proc;
                                    oldX = MX - proc.WindowData.WinPos.X;
                                    oldY = MY - proc.WindowData.WinPos.Y;


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
            MouseManager.X = (uint)ScreenSizeX/2;
            MouseManager.Y = (uint)ScreenSizeY/2;
            ProcessManager.start(new MessageBox { WindowData = new WindowData { WinPos = new Rectangle(100, 100, 350, 200)}, Name = "Window1" });

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