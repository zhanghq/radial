namespace Radial.Tools.Nh.Model2Sql
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChoseFile = new System.Windows.Forms.Button();
            this.tbHbmAssembly = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDbType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtbSql = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBaseDirectory = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbBaseDirectory);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.btnChoseFile);
            this.groupBox1.Controls.Add(this.tbHbmAssembly);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbDbType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // btnChoseFile
            // 
            this.btnChoseFile.Location = new System.Drawing.Point(544, 48);
            this.btnChoseFile.Name = "btnChoseFile";
            this.btnChoseFile.Size = new System.Drawing.Size(75, 23);
            this.btnChoseFile.TabIndex = 4;
            this.btnChoseFile.Text = "选择文件";
            this.btnChoseFile.UseVisualStyleBackColor = true;
            this.btnChoseFile.Click += new System.EventHandler(this.btnChoseFile_Click);
            // 
            // tbHbmAssembly
            // 
            this.tbHbmAssembly.Location = new System.Drawing.Point(300, 49);
            this.tbHbmAssembly.Name = "tbHbmAssembly";
            this.tbHbmAssembly.ReadOnly = true;
            this.tbHbmAssembly.Size = new System.Drawing.Size(244, 21);
            this.tbHbmAssembly.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "hbm程序集";
            // 
            // cbDbType
            // 
            this.cbDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbType.FormattingEnabled = true;
            this.cbDbType.Items.AddRange(new object[] {
            "SqlServer2008",
            "SqlServer2005",
            "SqlServer2012",
            "SqlServerAzure2008",
            "SqlServerCe40",
            "SqlServerCe",
            "MySql5",
            "MySql",
            "Sqlite"});
            this.cbDbType.Location = new System.Drawing.Point(73, 50);
            this.cbDbType.Name = "cbDbType";
            this.cbDbType.Size = new System.Drawing.Size(156, 20);
            this.cbDbType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库类型";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "程序集文件(*.dll)|*.dll";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtbSql);
            this.groupBox2.Location = new System.Drawing.Point(12, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(704, 273);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL";
            // 
            // rtbSql
            // 
            this.rtbSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSql.Location = new System.Drawing.Point(3, 17);
            this.rtbSql.Name = "rtbSql";
            this.rtbSql.ReadOnly = true;
            this.rtbSql.Size = new System.Drawing.Size(698, 253);
            this.rtbSql.TabIndex = 0;
            this.rtbSql.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(619, 48);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "生成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "运行目录";
            // 
            // tbBaseDirectory
            // 
            this.tbBaseDirectory.Location = new System.Drawing.Point(73, 21);
            this.tbBaseDirectory.Name = "tbBaseDirectory";
            this.tbBaseDirectory.ReadOnly = true;
            this.tbBaseDirectory.Size = new System.Drawing.Size(621, 21);
            this.tbBaseDirectory.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 384);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(745, 380);
            this.Name = "MainForm";
            this.Text = "NHibernate Model To Sql Tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbDbType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChoseFile;
        private System.Windows.Forms.TextBox tbHbmAssembly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtbSql;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBaseDirectory;
    }
}

