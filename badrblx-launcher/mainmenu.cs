﻿using System;
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
        static bool dlfin = false;
        static bool dev = false; //change this when releasing a client. VERY IMPORTANT!!!
        static string pps = "";
        public mainmenu(string[] args)
        {
            InitializeComponent();
            labelDevWarn.Visible = dev;
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
            if (File.Exists("update.exe")) { //delete updater
                try { File.Delete("update.exe"); }
                catch (IOException)
                {
                    Process.Start("taskkill", "/im update.exe /f").WaitForExit();
                    try { File.Delete("update.exe"); }
                    catch (IOException)
                    {
                        MessageBox.Show("Could not delete \"update.exe\".\nPlease delete it yourself.", "badRBLX");
                    }
                }
            }
            label3.Text = "Checking for update...";
            string tvr = "3b"; //VERSION!!!!!!!!!!!!!!!!
            try
            {
                HttpClient client = new HttpClient();
                var cvr = await client.GetAsync("https://badrblx.scottbeebiwan.tk/dls/curver" + ifdevreturn("-dev"));
                var cv = await cvr.Content.ReadAsStringAsync();
                if (cmd_args.Contains("update_test")) { cv = tvr + "-random-invalidation-text"; MessageBox.Show("Forced update initiated!", "badRBLX"); }
                if (tvr != cv)
                {
                    WebClient wc = new WebClient();
                    wc.DownloadProgressChanged += dpc;
                    wc.DownloadFileCompleted += dlc;
                    progressBar1.Maximum = 100;
                    label3.Text = "Downloading update...";
                    wc.DownloadFileAsync(new Uri("https://badrblx.scottbeebiwan.tk/dls/" + ifdevreturn("dev/") + "badrblx-installer.exe"), "update.exe");
                    while (!dlfin) { await Task.Delay(25); }
                    dlfin = true;
                    label3.Text = "Installing update...";
                    Process p = Process.Start("update.exe", "update" + ifdevreturn(" dev"));
                    Application.DoEvents();
                    p.WaitForExit();
                }
            } catch (Exception exc)
            {
                MessageBox.Show("Couldn't update\n" + exc.ToString(), "badRBLX");
            }
            label3.Text = "ScottBeebiWan 2018";
            lockall(true);
        }
        private string ifdevreturn(string _in)
        {
            if (dev)
            {
                return _in;
            } else
            {
                return "";
            }
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
                postdict = new Dictionary<string, string>
                {
                    {"serverid", textBox3.Text}
                };
                postme = new FormUrlEncodedContent(postdict);
                var respber = await client.PostAsync("https://badrblx.scottbeebiwan.tk/scripes/getid.php", postme);
                string respver = await respber.Content.ReadAsStringAsync();
                progressBar1.PerformStep(); label3.Text = "Launching Client";
                Directory.SetCurrentDirectory("br" + respver);
                if (respver != "2")
                {
                    Process.Start("robloxapp.exe", "-script \"https://badrblx.scottbeebiwan.tk/scripes/clientrun.php?session="+respStr+"\"");
                } else
                {
                    Process.Start("robloxapp.exe", "-joinscript \"https://badrblx.scottbeebiwan.tk/scripes/clientrun.php?session=" + respStr + "\"");
                }
                Directory.SetCurrentDirectory("..");
                button1.Enabled = true;
                label3.Text = "ScottBeebiWan 2018";
                progressBar1.Value = 0;
            }
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
    }
}
