using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;

namespace Radial.Tools.Hosts
{
    public partial class MainForm : Form
    {
        readonly string HostsFilePath;

        public MainForm()
        {
            InitializeComponent();

            HostsFilePath=System.Environment.SystemDirectory+@"\Drivers\etc\hosts";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show(this, "请以管理员身份运行本程序","启动",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = File.ReadAllText(HostsFilePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = richTextBox1.Text.Trim();

                File.WriteAllLines(HostsFilePath, txt.Split('\n'), Encoding.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message,"无法保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                e.Handled = true;
                button1.PerformClick();
            }
        }
    }
}
