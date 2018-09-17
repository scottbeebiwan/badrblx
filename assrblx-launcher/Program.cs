using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assrblx_launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("    = = = = = = = = assRBLX Launcher = = = = = = = =");
            Console.WriteLine("Written in a few minutes by ScottBeebiWan/ScottishScotty");
            Console.Write("Username: ");
            string usr = Console.ReadLine();
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Console.WriteLine("Launching assRBLX...");
            Cmd("");
        }
        static void Cmd(string command)
        {
            // Code proudly stolen from stackoverflow.com/questions/1469764
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
