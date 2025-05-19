using Cosmos.System;
using PilotOS.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Apps
{
    public class TextEditor : Process
    {
        public string Path = "";
        public override void Start()
        {
            Path = WindowData.args;
        }
        public override void Run()
        {
            Window.DrawTop(this);
        }
        public override void OnKeyPressed(KeyEvent key)
        {

        }
    }
}
