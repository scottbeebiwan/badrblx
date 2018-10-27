using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace badrblx_launcher
{
    public partial class host : Form
    {
        public host()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainmenu f1 = new mainmenu(new string[0]);
            f1.Show();
            this.Hide();
        }

        private void host_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainmenu f1 = new mainmenu(new string[0]);
            f1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //proudly stolen from stackoverflow.com/questions/4999734
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                textBox1.Text = file;
            }
        }
        private static string dofile(string place)
        {
            Regex p = new Regex("\\\\");
            return "dofile('" + p.Replace(place, "/") + "')";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                string client = comboBoxVersion.SelectedItem.ToString();
                string rootdir = Directory.GetCurrentDirectory();
                try
                {
                    Directory.SetCurrentDirectory("br" + client);
                    if (client != "2") { Process.Start("robloxapp", "-script \"" + dofile(rootdir + "/brs/host.lua") + "\" \"" + textBox1.Text + "\""); }
                    else { Process.Start("robloxapp", "-joinscripturl \"" + jsu(rootdir + "/brs/host.lua") + "\" \"" + textBox1.Text + "\""); }
                    Directory.SetCurrentDirectory("..");
                }
                catch (Win32Exception)
                {
                    MessageBox.Show("The client you selected doesn't exist.", "badRBLX");
                }
            }
            else
            {
                MessageBox.Show("the file you selected has been confirmed for not-exist-world");
            }
        }

        private void comboBoxVersion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Convert.ToInt16(comboBoxVersion.SelectedItem.ToString()) < 7)
            {
                labelVersion.Text = "201";
            } else
            {
                labelVersion.Text = "200";
            }
        }

        private void comboBoxVersion_DropDown(object sender, EventArgs e)
        {
            labelVersion.Text = "20?";
        }

        private static string jsu(string place)
        {
            Regex p = new Regex("\\\\");
            return p.Replace(place, "/");
        }
    }
}
