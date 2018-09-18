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
using System.Net.Http;
using System.Text.RegularExpressions;

namespace badrblx_launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            var resp = await client.PostAsync("https://scottbeebiwan.tk/badrblx/scripes/login.php", postme);
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
                Directory.SetCurrentDirectory("badrblx");
                File.WriteAllText("join.lua", respStr);
                Process.Start("robloxapp.exe", "-script \""+dofile(Directory.GetCurrentDirectory()+"\\join.lua")+"\"");
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
            Process.Start("cmd.exe", "/c start \"\" explorer %localappdata%\\roblox\\logs");
        }
    }
}
