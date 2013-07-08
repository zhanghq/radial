using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Radial.Tools.NhDbFirst.Kernel;

namespace Radial.Tools.NhDbFirst
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 当前配置文件
        /// </summary>
        Profile _currentProfile = new Profile();

        /// <summary>
        /// 从全选控件发出的操作
        /// </summary>
        bool _isFromSelectAllCheckBox = false;
        /// <summary>
        /// 从单选控件发出的操作
        /// </summary>
        bool _isFromSelectSingleCheckBox = false;

        public MainForm()
        {
            InitializeComponent();

            pgProfile.SelectedObject = _currentProfile.Clone();
        }

        private void tsbNewProfile_Click(object sender, EventArgs e)
        {
            var tp = pgProfile.SelectedObject as Profile;

            //修改了内容
            if (!tp.Equals(_currentProfile))
            {
                if (MessageBox.Show(this, "当前配置已被修改，是否立即保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tp.FilePath))
                        {
                            if (sfdProfile.ShowDialog(this) == DialogResult.OK)
                            {
                                tp.Save(sfdProfile.FileName);
                            }
                        }
                        else
                            tp.Save(tp.FilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            _currentProfile = new Profile();
            pgProfile.SelectedObject = _currentProfile.Clone();

            clbTables.Items.Clear();
            cbSelectAll.Checked = false;
        }

        private void tsbLoadProfile_Click(object sender, EventArgs e)
        {
            var tp = pgProfile.SelectedObject as Profile;

            //修改了内容
            if (!tp.Equals(_currentProfile))
            {
                if (MessageBox.Show(this, "当前配置已被修改，是否立即保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tp.FilePath))
                        {
                            if (sfdProfile.ShowDialog(this) == DialogResult.OK)
                            {
                                tp.Save(sfdProfile.FileName);
                            }
                        }
                        else
                            tp.Save(tp.FilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (ofdProfile.ShowDialog(this) == DialogResult.OK)
            {
                Profile lp = Profile.Load(ofdProfile.FileName);

                if (lp == null)
                {
                    MessageBox.Show(this, "无法从 " + ofdProfile.FileName + " 中读取配置信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentProfile = lp;
                pgProfile.SelectedObject = _currentProfile.Clone();

                clbTables.Items.Clear();
                cbSelectAll.Checked = false;
            }
        }

        private void tsbSaveProfile_Click(object sender, EventArgs e)
        {
            var tp = pgProfile.SelectedObject as Profile;

            try
            {
                if (string.IsNullOrWhiteSpace(tp.FilePath))
                {
                    if (sfdProfile.ShowDialog(this) == DialogResult.OK)
                    {
                        tp.Save(sfdProfile.FileName);
                    }
                }
                else
                    tp.Save(tp.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _currentProfile = tp;
            pgProfile.SelectedObject = _currentProfile.Clone();
        }

        private void tsbReadSchema_Click(object sender, EventArgs e)
        {
            var tp = pgProfile.SelectedObject as Profile;

            if (string.IsNullOrWhiteSpace(tp.ConnectionString))
            {
                MessageBox.Show(this, "请设置数据库连接字符串", "提示");
                return;
            }

            IList<TableDefinition> tableDefs = TableDefinition.Retrieve(tp);

            clbTables.Items.Clear();
            cbSelectAll.Checked = false;

            foreach (var t in tableDefs)
            {
                clbTables.Items.Add(t);
            }

        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_isFromSelectSingleCheckBox)
                return;

            _isFromSelectAllCheckBox = true;

            bool isSelectAll = false;

            if (cbSelectAll.Checked)
                isSelectAll = true;

            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                clbTables.SetItemChecked(i, isSelectAll);
            }

            _isFromSelectAllCheckBox = false;
        }

        private void tsbBuild_Click(object sender, EventArgs e)
        {
            var tp = pgProfile.SelectedObject as Profile;

            if (string.IsNullOrWhiteSpace(tp.ConnectionString))
            {
                MessageBox.Show(this, "请设置数据库连接字符串", "提示");
                return;
            }

            if (string.IsNullOrWhiteSpace(tp.ClassAssembly))
            {
                MessageBox.Show(this, "请设置生成类的程序集名称", "提示");
                return;
            }

            if (string.IsNullOrWhiteSpace(tp.ClassNamespace))
            {
                MessageBox.Show(this, "请设置生成类的命名空间", "提示");
                return;
            }

            if (string.IsNullOrWhiteSpace(tp.OutputDirectory))
            {
                MessageBox.Show(this, "请设置生成文件的输出目录", "提示");
                return;
            }

            var checks = clbTables.CheckedItems;

            if (checks.Count == 0)
            {
                MessageBox.Show(this, "请选择需要生成的数据表", "提示");
                return;
            }

            IList<TableDefinition> tableDefChecks = new List<TableDefinition>();

            foreach (var o in checks)
            {
                tableDefChecks.Add(o as TableDefinition);
            }

            PendingForm pForm = new PendingForm();

            Thread buildThread = new Thread(BuildThread);

            buildThread.Start(new { profile = tp, tableDefs = tableDefChecks, pendingForm = pForm });


            pForm.ShowDialog(this);
        }

        private void BuildThread(object obj)
        {
            dynamic objd = (obj as dynamic);

            Profile profile = objd.profile;
            IList<TableDefinition> tableDefs = objd.tableDefs;
            PendingForm pForm = objd.pendingForm;

            try
            {
                if (!Directory.Exists(profile.OutputDirectory))
                    Directory.CreateDirectory(profile.OutputDirectory);

                string tempbaseDir = Path.Combine(profile.OutputDirectory, profile.ClassAssembly);

                if (!Directory.Exists(tempbaseDir))
                    Directory.CreateDirectory(tempbaseDir);

                string baseDir = Path.Combine(tempbaseDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string modelPath = Path.Combine(baseDir, "Models");
                string mapPath = Path.Combine(baseDir, "Maps");

                if (!Directory.Exists(modelPath))
                    Directory.CreateDirectory(modelPath);
                if (!Directory.Exists(mapPath))
                    Directory.CreateDirectory(mapPath);

                foreach (TableDefinition tableDef in tableDefs)
                {
                    ClassMapper cm = new ClassMapper
                    {
                        TableDefinition = tableDef,
                        ClassName = Toolkits.NormalizeName(tableDef.Name, profile.NameSpliters),
                        AssemblyName = profile.ClassAssembly,
                        NamespaceName = profile.ClassNamespace,
                        LazyModel = profile.LazyModel,
                        DataSource = profile.DataSource
                    };

                    IList<FieldDefinition> fieldDefs = FieldDefinition.Generate(profile, tableDef);

                    foreach (FieldDefinition fd in fieldDefs)
                    {
                        PropertyMapper pm = new PropertyMapper
                        {
                            ClassMapper = cm,
                            FieldDefinition = fd,
                            TypeString = SqlTypeTransfer.GetPropertyTypeString(profile.DataSource, fd.SqlType),
                            PropertyName = Toolkits.NormalizeName(fd.Name, profile.NameSpliters),
                            DataSource = profile.DataSource
                        };

                        cm.Properties.Add(pm);
                    }

                    using (TextWriter codeWriter = new StreamWriter(Path.Combine(modelPath, cm.ClassName) + ".cs"))
                    {
                        cm.WriteCode(codeWriter);
                    }
                    using (TextWriter codeWriter = new StreamWriter(Path.Combine(mapPath, cm.ClassName) + ".hbm.xml"))
                    {
                        cm.WriteXml(codeWriter);
                    }
                }

                System.Diagnostics.Process.Start("explorer.exe ", baseDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pForm.Close();
        }

        private void clbTables_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_isFromSelectAllCheckBox)
                return;

            _isFromSelectSingleCheckBox = true;

            if (e.NewValue == CheckState.Unchecked && cbSelectAll.Checked)
            {
                var cic = clbTables.CheckedIndices;

                cbSelectAll.Checked = false;
            }

            if (e.NewValue == CheckState.Checked)
            {
                if (clbTables.CheckedItems.Count + 1 == clbTables.Items.Count && !cbSelectAll.Checked)
                    cbSelectAll.Checked = true;
            }

            _isFromSelectSingleCheckBox = false;
        }
    }
}
