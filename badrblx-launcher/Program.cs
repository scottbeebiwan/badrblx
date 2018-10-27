using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace badrblx_launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(handler);
            Application.Run(new mainmenu(args));
        }
        private static void handler(object sender, ThreadExceptionEventArgs t)
        {
            Exception ex = default(Exception);
            ex = t.Exception;
            File.WriteAllText("crash.log", ex.Message+"\n"+ex.StackTrace+"\n"+ex.HResult.ToString());
            MessageBox.Show("AW NAW!!! badRBLX just crashed!\nContact badRBLX staff on Discord and give them the \"crash.log\" file in the same directory as your launcher.\nbadRBLX will now restart.", "badRBLX");
            Application.Restart();
        }
    }
}
