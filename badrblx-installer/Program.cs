using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace badrblx_installer
{
    class Program
    {
        static bool dlfin = false;

        static void Main(string[] args)
        {
            bool updater = false;
            string rootpath = "https://badrblx.scottbeebiwan.tk/dls/";
            if (args.Contains("update")) { updater = true; }
            if (args.Contains("dev")) { rootpath += "dev/"; }
            if (updater) { Process.Start("taskkill", "/im badrblx-launcher.exe /f").WaitForExit(); }
            Console.WriteLine("Downloading Launcher...");
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += dpc;
            wc.DownloadFileCompleted += dlc;
            Uri uri = new Uri(rootpath+"badrblx-launcher.exe");
            wc.DownloadFileAsync(uri, "badrblx-launcher.exe");
            while (!dlfin) { }
            dlfin = false;
            Console.WriteLine("Downloading 7ZA");
            uri = new Uri(rootpath + "7za.exe");
            wc.DownloadFileAsync(uri, "7za.exe");
            while (!dlfin) { }
            dlfin = false;
            Console.WriteLine("Downloading Clients");
            uri = new Uri(rootpath + "clients.zip");
            wc.DownloadFileAsync(uri, "clients.zip");
            while (!dlfin) { }
            dlfin = false;
            Console.WriteLine("Decompressing Client");
            if (!updater)
            {
                Directory.CreateDirectory("badrblx");
                Process.Start("7za.exe", "x -obadrblx\\ clients.zip").WaitForExit();
            } else
            {
                Process.Start("7za.exe", "x -y clients.zip").WaitForExit();
            }
            if (!updater)
            {
                Console.WriteLine("Moving files  ");
                File.Move("badrblx-launcher.exe", "badrblx\\badrblx-launcher.exe");
            }
            Console.WriteLine("Deleting temporary files");
            File.Delete("client.7z");
            File.Delete("7za.exe");
            if (!updater)
            {
                Console.WriteLine("badRBLX was installed in the badRBLX folder in the same folder as the installer exe.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } else
            {
                Process.Start("cmd", "/c start badrblx-launcher.exe");
            }
        }
        static void dpc(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write(e.ProgressPercentage.ToString()+"% Completed   \r");
        }
        static void dlc(object sender, AsyncCompletedEventArgs e)
        {
            dlfin = true;
        }
    }
}
