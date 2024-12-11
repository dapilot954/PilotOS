using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.System
{
    public static class ProcessManager
    {
        public static List<Process> ProcessList = new List<Process>();

        public static void Update()
        {
            foreach (Process process in ProcessList)
            {
                process.Run();
            }
        }
        public static void start(Process process)
        {
            ProcessList.Add(process);
            process.Start();
        }
        public static void stop(Process process)
        {
            ProcessList.Remove(process);
        }
    }
}
