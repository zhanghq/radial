using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using Radial.Tools.Hbm2Sql.NamingStrategy;
using Radial.Tools.Hbm2Sql.TSqlFormatter;
using Radial.Tools.Hbm2Sql.TSqlFormatter.Formatters;

namespace Radial.Tools.Hbm2Sql
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cbDbType.SelectedIndex = 0;
           tbBaseDirectory.Text += AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
           openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void btnChoseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (Path.GetDirectoryName(filePath) != AppDomain.CurrentDomain.BaseDirectory.Trim('\\'))
                {
                    MessageBox.Show(this, "不能选择运行目录以外的程序集", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                tbHbmAssembly.Text = Path.GetFileName(filePath);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbHbmAssembly.Text))
            {
                MessageBox.Show(this, "请选择Hbm程序集文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PendingForm pForm = new PendingForm();

            Thread exeThread = new Thread(ExecuteThread);

            exeThread.Start(new { pendingForm = pForm });


            pForm.ShowDialog(this);

        }

        private void ExecuteThread(object obj)
        {
            dynamic objd = (obj as dynamic);
            PendingForm pForm = objd.pendingForm;

            try
            {
                var cfg = new Configuration();

                switch (cbDbType.SelectedItem.ToString())
                {
                    case "SqlServer2005": cfg.DataBaseIntegration(c => c.Dialect<MsSql2005Dialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "SqlServer2008": cfg.DataBaseIntegration(c => c.Dialect<MsSql2008Dialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "SqlServer2012": cfg.DataBaseIntegration(c => c.Dialect<MsSql2012Dialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "SqlServerAzure2008": cfg.DataBaseIntegration(c => c.Dialect<MsSqlAzure2008Dialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "SqlServerCe40": cfg.DataBaseIntegration(c => c.Dialect<MsSqlCe40Dialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "SqlServerCe": cfg.DataBaseIntegration(c => c.Dialect<MsSqlCeDialect>()); cfg.SetNamingStrategy(new SqlServerNamingStrategy()); break;
                    case "MySql5": cfg.DataBaseIntegration(c => c.Dialect<MySQL5Dialect>()); cfg.SetNamingStrategy(new MySqlNamingStrategy()); break;
                    case "MySql": cfg.DataBaseIntegration(c => c.Dialect<MySQLDialect>()); cfg.SetNamingStrategy(new MySqlNamingStrategy()); break;
                    case "Sqlite": cfg.DataBaseIntegration(c => c.Dialect<SQLiteDialect>()); cfg.SetNamingStrategy(new SqliteNamingStrategy()); break;
                }

                cfg.AddAssembly(Assembly.LoadFrom(tbHbmAssembly.Text));

                var export = new SchemaExport(cfg);

                StringBuilder sb = new StringBuilder();

                export.Execute(s => sb.Append(s), false, false);

                string sql = sb.ToString();

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    TSqlStandardFormatter formatter = new TSqlStandardFormatter();
                    formatter.Options.TrailingCommas = true;
                    SqlFormattingManager m = new SqlFormattingManager(formatter);

                    rtbSql.Text = m.Format(sql);
                }
                else
                    rtbSql.Text = "No Sql Script Generated!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pForm.Close();
        }
    }
}
