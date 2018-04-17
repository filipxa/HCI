namespace Charts
{
    partial class Chart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chart));
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.btOdabirGrafa = new System.Windows.Forms.Button();
            this.groupBoxGraphControl = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btResetZoom = new System.Windows.Forms.Button();
            this.btNewInstance = new System.Windows.Forms.Button();
            this.btCloseAll = new System.Windows.Forms.Button();
            this.groupBoxGraphControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cartesianChart1.Location = new System.Drawing.Point(12, 12);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1052, 312);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // btOdabirGrafa
            // 
            this.btOdabirGrafa.BackColor = System.Drawing.Color.Goldenrod;
            this.btOdabirGrafa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btOdabirGrafa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOdabirGrafa.Location = new System.Drawing.Point(6, 19);
            this.btOdabirGrafa.Name = "btOdabirGrafa";
            this.btOdabirGrafa.Size = new System.Drawing.Size(213, 39);
            this.btOdabirGrafa.TabIndex = 2;
            this.btOdabirGrafa.Text = "Ucitaj graf";
            this.btOdabirGrafa.UseVisualStyleBackColor = false;
            this.btOdabirGrafa.Click += new System.EventHandler(this.btOdabirGrafa_Click);
            // 
            // groupBoxGraphControl
            // 
            this.groupBoxGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxGraphControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxGraphControl.Controls.Add(this.btOdabirGrafa);
            this.groupBoxGraphControl.ForeColor = System.Drawing.Color.Lavender;
            this.groupBoxGraphControl.Location = new System.Drawing.Point(12, 330);
            this.groupBoxGraphControl.Name = "groupBoxGraphControl";
            this.groupBoxGraphControl.Size = new System.Drawing.Size(225, 176);
            this.groupBoxGraphControl.TabIndex = 3;
            this.groupBoxGraphControl.TabStop = false;
            this.groupBoxGraphControl.Text = "Graph control";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Location = new System.Drawing.Point(243, 330);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(497, 176);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.Visible = false;
            // 
            // btResetZoom
            // 
            this.btResetZoom.Location = new System.Drawing.Point(945, 330);
            this.btResetZoom.Name = "btResetZoom";
            this.btResetZoom.Size = new System.Drawing.Size(119, 23);
            this.btResetZoom.TabIndex = 6;
            this.btResetZoom.Text = "Reset zoom";
            this.btResetZoom.UseVisualStyleBackColor = true;
            this.btResetZoom.Click += new System.EventHandler(this.btResetZoom_Click);
            // 
            // btNewInstance
            // 
            this.btNewInstance.Location = new System.Drawing.Point(945, 359);
            this.btNewInstance.Name = "btNewInstance";
            this.btNewInstance.Size = new System.Drawing.Size(119, 23);
            this.btNewInstance.TabIndex = 7;
            this.btNewInstance.Text = "Start new instance";
            this.btNewInstance.UseVisualStyleBackColor = true;
            this.btNewInstance.Click += new System.EventHandler(this.btNewInstance_Click);
            // 
            // btCloseAll
            // 
            this.btCloseAll.Location = new System.Drawing.Point(945, 388);
            this.btCloseAll.Name = "btCloseAll";
            this.btCloseAll.Size = new System.Drawing.Size(119, 23);
            this.btCloseAll.TabIndex = 8;
            this.btCloseAll.Text = "Close all instances";
            this.btCloseAll.UseVisualStyleBackColor = true;
            this.btCloseAll.Click += new System.EventHandler(this.btCloseAll_Click);
            // 
            // Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(1076, 518);
            this.Controls.Add(this.btCloseAll);
            this.Controls.Add(this.btNewInstance);
            this.Controls.Add(this.btResetZoom);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBoxGraphControl);
            this.Controls.Add(this.cartesianChart1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Chart";
            this.Text = "Charts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Chart_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxGraphControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.Button btOdabirGrafa;
        private System.Windows.Forms.GroupBox groupBoxGraphControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btResetZoom;
        private System.Windows.Forms.Button btNewInstance;
        private System.Windows.Forms.Button btCloseAll;
    }
}

