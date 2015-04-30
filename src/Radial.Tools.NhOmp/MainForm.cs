using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radial.Tools.NhOmp
{
    public partial class MainForm : Form
    {
        Assembly classAssembly;
        
        public MainForm()
        {
            InitializeComponent();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();

            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    classAssembly = Assembly.LoadFrom(openFileDialog1.FileName);

                    foreach(var type in classAssembly.GetTypes())
                    {
                        this.comboBox1.Items.Add(type);
                    }

                    if (this.comboBox1.Items.Count > 0)
                        this.comboBox1.SelectedIndex = 0;
                }
                catch(Exception ex)
                {
                    classAssembly = null;
                    richTextBox1.Text = null;
                    MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var classType = this.comboBox1.SelectedItem as Type;

            if(classAssembly==null)
            {
                MessageBox.Show(this, "请先加载程序集", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (classType == null)
            {
                MessageBox.Show(this, "请选择需生成的类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Generator gen = new Generator(classAssembly, classType);
            try
            {
                richTextBox1.Text = gen.BuildHbm();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = null;
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
