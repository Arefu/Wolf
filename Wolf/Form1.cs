using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Wolf
{
    public partial class Form1 : Form
    {
        public static string InstallDir;
        public static bool IsFileLoaded; //Replace With Is Stream Open Logic.


        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object Sender, EventArgs Args)
        {
           
            try
            {
                using (var Root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var Key =Root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 480650"))
                    {
                        InstallDir = Key?.GetValue("InstallLocation").ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw new FileNotFoundException("Can't Find Game");
            }

            GameLocLabel.ForeColor = Color.Red;
            GameLocLabel.Text = "Game Not Loaded";
        }

        private void ExitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (IsFileLoaded)
            {
                var Reply = MessageBox.Show(this, "The Yu-Gi-Oh! Data File Is Still Loaded, Are You Sure You Want To Quit?", "Data File Still In Use...", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (Reply != DialogResult.Yes) return;
            }
            Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsFileLoaded = true;
            GameLocLabel.ForeColor = Color.Green;
            GameLocLabel.Text = "Game Loaded";
        }
    }
}
