using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System.Utils
{
    public static class WriteMessage
    {
        public static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(error);
        }
        public static void WriteWarn(string warn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warning] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(warn);
        }
        public static void WriteInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[info] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(info);
        }
        public static void WriteOK(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[OK] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void Logo()
        {
            Console.WriteLine(" 888888ba  oo dP            dP    .88888.  .d88888b  \r\n 88    `8b    88            88   d8'   `8b 88.    \"' \r\na88aaaa8P' dP 88 .d8888b. d8888P 88     88 `Y88888b. \r\n 88        88 88 88'  `88   88   88     88       `8b \r\n 88        88 88 88.  .88   88   Y8.   .8P d8'   .8P \r\n dP        dP dP `88888P'   dP    `8888P'   Y88888P  ");
        }

        public static string CenterText(string text)
        {
            int consoleWidth = 90;
            int padding = (consoleWidth - text.Length) / 2;
            string centeredText = text.PadLeft(padding + text.Length).PadRight(consoleWidth);
            return centeredText;
        }
    }
}
