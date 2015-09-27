namespace Radial.Tools.NhDbFirst
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer Dependency = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Dependency != null))
            {
                Dependency.Dispose();
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
            this.pgProfile = new System.Windows.Forms.PropertyGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tsProfile = new System.Windows.Forms.ToolStrip();
            this.tsbNewProfile = new System.Windows.Forms.ToolStripButton();
            this.tsbLoadProfile = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveProfile = new System.Windows.Forms.ToolStripButton();
            this.tsbReadSchema = new System.Windows.Forms.ToolStripButton();
            this.tsbBuild = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.clbTables = new System.Windows.Forms.CheckedListBox();
            this.ofdProfile = new System.Windows.Forms.OpenFileDialog();
            this.sfdProfile = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tsProfile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgProfile
            // 
            this.pgProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProfile.Location = new System.Drawing.Point(3, 45);
            this.pgProfile.Name = "pgProfile";
            this.pgProfile.Size = new System.Drawing.Size(322, 313);
            this.pgProfile.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer.Size = new System.Drawing.Size(724, 361);
            this.splitContainer.SplitterDistance = 328;
            this.splitContainer.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tsProfile);
            this.groupBox1.Controls.Add(this.pgProfile);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 361);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // tsProfile
            // 
            this.tsProfile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewProfile,
            this.tsbLoadProfile,
            this.tsbSaveProfile,
            this.tsbReadSchema,
            this.tsbBuild});
            this.tsProfile.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tsProfile.Location = new System.Drawing.Point(3, 17);
            this.tsProfile.Name = "tsProfile";
            this.tsProfile.Padding = new System.Windows.Forms.Padding(0);
            this.tsProfile.Size = new System.Drawing.Size(322, 23);
            this.tsProfile.TabIndex = 1;
            this.tsProfile.Text = "toolStripProfile";
            // 
            // tsbNewProfile
            // 
            this.tsbNewProfile.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewProfile.Image")));
            this.tsbNewProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewProfile.Name = "tsbNewProfile";
            this.tsbNewProfile.Size = new System.Drawing.Size(51, 20);
            this.tsbNewProfile.Text = "新建";
            this.tsbNewProfile.Click += new System.EventHandler(this.tsbNewProfile_Click);
            // 
            // tsbLoadProfile
            // 
            this.tsbLoadProfile.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadProfile.Image")));
            this.tsbLoadProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadProfile.Name = "tsbLoadProfile";
            this.tsbLoadProfile.Size = new System.Drawing.Size(51, 20);
            this.tsbLoadProfile.Text = "加载";
            this.tsbLoadProfile.ToolTipText = "加载";
            this.tsbLoadProfile.Click += new System.EventHandler(this.tsbLoadProfile_Click);
            // 
            // tsbSaveProfile
            // 
            this.tsbSaveProfile.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveProfile.Image")));
            this.tsbSaveProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveProfile.Name = "tsbSaveProfile";
            this.tsbSaveProfile.Size = new System.Drawing.Size(51, 20);
            this.tsbSaveProfile.Text = "保存";
            this.tsbSaveProfile.Click += new System.EventHandler(this.tsbSaveProfile_Click);
            // 
            // tsbReadSchema
            // 
            this.tsbReadSchema.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadSchema.Image")));
            this.tsbReadSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadSchema.Name = "tsbReadSchema";
            this.tsbReadSchema.Size = new System.Drawing.Size(87, 20);
            this.tsbReadSchema.Text = "读取数据表";
            this.tsbReadSchema.Click += new System.EventHandler(this.tsbReadSchema_Click);
            // 
            // tsbBuild
            // 
            this.tsbBuild.Image = ((System.Drawing.Image)(resources.GetObject("tsbBuild.Image")));
            this.tsbBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBuild.Name = "tsbBuild";
            this.tsbBuild.Size = new System.Drawing.Size(75, 20);
            this.tsbBuild.Text = "生成选中";
            this.tsbBuild.Click += new System.EventHandler(this.tsbBuild_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cbSelectAll);
            this.groupBox2.Controls.Add(this.clbTables);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 361);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据表";
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(5, 20);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(72, 16);
            this.cbSelectAll.TabIndex = 1;
            this.cbSelectAll.Text = "全部选中";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // clbTables
            // 
            this.clbTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTables.FormattingEnabled = true;
            this.clbTables.Location = new System.Drawing.Point(2, 38);
            this.clbTables.Name = "clbTables";
            this.clbTables.Size = new System.Drawing.Size(390, 324);
            this.clbTables.TabIndex = 0;
            this.clbTables.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTables_ItemCheck);
            // 
            // sfdProfile
            // 
            this.sfdProfile.DefaultExt = "xml";
            this.sfdProfile.FileName = "profile";
            this.sfdProfile.Filter = "Xml 文件|*.xml";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 361);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "NHibernate DbFirst Code Generator";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tsProfile.ResumeLayout(false);
            this.tsProfile.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgProfile;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip tsProfile;
        private System.Windows.Forms.ToolStripButton tsbNewProfile;
        private System.Windows.Forms.ToolStripButton tsbLoadProfile;
        private System.Windows.Forms.ToolStripButton tsbSaveProfile;
        private System.Windows.Forms.ToolStripButton tsbReadSchema;
        private System.Windows.Forms.OpenFileDialog ofdProfile;
        private System.Windows.Forms.SaveFileDialog sfdProfile;
        private System.Windows.Forms.ToolStripButton tsbBuild;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox clbTables;
        private System.Windows.Forms.CheckBox cbSelectAll;
    }
}

