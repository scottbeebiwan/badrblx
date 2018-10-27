using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace badrblx_launcher
{
    public partial class mainmenu : Form
    {
        private static string[] cmd_args;
        static bool dlfin=false;
        static string pps = "";
        public mainmenu(string[] args)
        {
            InitializeComponent();
            cmd_args = args;
        }
        void dpc(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value=e.ProgressPercentage;
            pps = e.ProgressPercentage.ToString();
        }
        void dlc(object sender, AsyncCompletedEventArgs e)
        {
            dlfin = true;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            lockall(false);
            label3.Text = "Checking for update...";
            string tvr = "3a";
            HttpClient client = new HttpClient();
            var cvr = await client.GetAsync("https://badrblx.scottbeebiwan.tk/dls/curver-dev");
            var cv = await cvr.Content.ReadAsStringAsync();
            if (cmd_args.Length > 0)
            { if (cmd_args[0]=="update_test") { cv = tvr + "-random-invalidation-text"; } }
            if (tvr != cv) {
                WebClient wc = new WebClient();
                wc.DownloadProgressChanged += dpc;
                wc.DownloadFileCompleted += dlc;
                progressBar1.Maximum = 100;
                label3.Text = "Downloading update...";
                wc.DownloadFileAsync(new Uri("https://badrblx.scottbeebiwan.tk/dls/dev/badrblx-installer.exe"), "update.exe");
                while (!dlfin) { await Task.Delay(25); }
                dlfin = true;
                label3.Text = "Installing update... (Please wait, the launcher will freeze)";
                var p = Process.Start("update.exe");
                Application.DoEvents();
                p.WaitForExit();
                label3.Text = "Exiting...";
                wc.DownloadFileAsync(new Uri("https://badrblx.scottbeebiwan.tk/dls/dev/aus.bat"), "..\\aus.bat");
                while (!dlfin) { await Task.Delay(25); }
                dlfin = true;
                Process.Start("cmd.exe", "/c start ..\\aus.bat");
                Application.Exit();
            }
            label3.Text = "ScottBeebiWan 2018";
            lockall(true);
        }
        private void lockall(bool locc)
        {
            textBox1.Enabled = locc;
            textBox2.Enabled = locc;
            textBox3.Enabled = locc;
            button1.Enabled = locc;
            button2.Enabled = locc;
            button3.Enabled = locc;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var postdict = new Dictionary<string, string>
            {
                {"username", textBox1.Text},
                {"password", textBox2.Text},
                {"serverid", textBox3.Text}
            };
            var postme = new FormUrlEncodedContent(postdict);
            button1.Enabled = false;
            progressBar1.PerformStep(); label3.Text = "Logging in";
            var resp = await client.PostAsync("https://badrblx.scottbeebiwan.tk/scripes/login.php", postme);
            var respStr = await resp.Content.ReadAsStringAsync();
            if (respStr == "user or pass wrong")
            {
                label3.Text = "Username or password incorrect";
                button1.Enabled = true;
                progressBar1.Value = 0;
            } else if (respStr == "serverid invalid") {
                label3.Text = "ServerID Invalid";
                progressBar1.Value = 0;
                button1.Enabled = true;
            } else
            {
                progressBar1.PerformStep(); label3.Text = "Launching Client";
                string respver = respStr.Substring(0,1);
                respStr = respStr.Substring(1);
                if (respver=="0")
                {
                    respStr = File.ReadAllText("brs\\Resizefix.lua") + "\n" + respStr;
                }
                File.WriteAllText("br" + respver + "\\join.lua", respStr);
                Directory.SetCurrentDirectory("br"+respver);
                Process.Start("robloxapp.exe", "-script \"" + dofile(Directory.GetCurrentDirectory() + "/join.lua") + "\"");
                Directory.SetCurrentDirectory("..");
                button1.Enabled = true;
                label3.Text = "ScottBeebiWan 2018";
                progressBar1.Value = 0;
            }
        }
        private static string dofile(string place)
        {
            Regex p = new Regex("\\\\");
            return "dofile('" + p.Replace(place, "/") + "')";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "%localappdata%\\roblox\\logs");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            host hos = new host();
            hos.Show();
            Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void seperator1_Click(object sender, EventArgs e)
        {

        }
    }
}
