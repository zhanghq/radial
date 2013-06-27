using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookNine.Application;
using BookNine.TransferObject;
using Radial.Boot;

namespace BookNine.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Bootstrapper.Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bootstrapper.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Bootstrapper.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserModel model = UserService.Create(string.Format("{0}@abc.com", Radial.RandomCode.Create(6).ToLower()), "123456");

            MessageBox.Show(model.Mail);
        }
    }
}
