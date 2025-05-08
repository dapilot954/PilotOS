using PilotOS.System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PilotOS.Apps.TerminalAssets.Commands
{
    internal class Add
    {
        public static string[] aliases = { "add", "write" };
        public static void run(string[] words, Terminal terminal)
        {
            if (words.Length > 1)
            {
                bool valid;
                int a = 0;
                while (true)
                {
                    if (a == words.Length)
                    {
                        valid = false;
                        break;
                    }
                    else
                    {
                        if (words[a].Contains(">"))
                        {
                            valid = true;
                            break ;
                            
                        }
                        else { valid = false;}
                        a++;
                    }
                }
                int b = words.Length-1;
                if (words[b].EndsWith(">"))
                {
                    valid = false;
                    
                }
                if (valid)
                {
                    string wholestring = "";
                    for (int i = 0; i < words.Length; i++)
                    {
                        wholestring += words[i] + " ";

                    }
                    
                    
                    
                    int pathIndex = wholestring.LastIndexOf('>');
                    string text = wholestring.Substring(0, pathIndex);
                    string path = wholestring.Substring(pathIndex + 1);
                    if (!path.StartsWith(@"0:\"))
                    {
                        path = terminal.Path + path;
                    }
                    if (path.EndsWith(' '))
                    {
                        path = path.Substring(0, path.Length - 1);
                    }
                    text = text.Remove(0, 4);


                    File.WriteAllText(path, text);
                    terminal.print_perm("completed succesfully");

                    


                }
                else
                {
                    terminal.print_perm("invalid syntax");
                }
                

            }
            else
            {
                terminal.print_perm("invalid syntax!");
            }
        }
    }
}
