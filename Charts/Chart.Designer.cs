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
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.btOdabirGrafa = new System.Windows.Forms.Button();
            this.groupBoxGraphControl = new System.Windows.Forms.GroupBox();
            this.labelLastUpdated = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBoxGraphControl.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cartesianChart1.Location = new System.Drawing.Point(12, 12);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1137, 507);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // btOdabirGrafa
            // 
            this.btOdabirGrafa.Location = new System.Drawing.Point(6, 19);
            this.btOdabirGrafa.Name = "btOdabirGrafa";
            this.btOdabirGrafa.Size = new System.Drawing.Size(213, 23);
            this.btOdabirGrafa.TabIndex = 2;
            this.btOdabirGrafa.Text = "Ucitaj graf";
            this.btOdabirGrafa.UseVisualStyleBackColor = true;
            this.btOdabirGrafa.Click += new System.EventHandler(this.btOdabirGrafa_Click);
            // 
            // groupBoxGraphControl
            // 
            this.groupBoxGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxGraphControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxGraphControl.Controls.Add(this.labelLastUpdated);
            this.groupBoxGraphControl.Controls.Add(this.btOdabirGrafa);
            this.groupBoxGraphControl.Location = new System.Drawing.Point(12, 525);
            this.groupBoxGraphControl.Name = "groupBoxGraphControl";
            this.groupBoxGraphControl.Size = new System.Drawing.Size(225, 176);
            this.groupBoxGraphControl.TabIndex = 3;
            this.groupBoxGraphControl.TabStop = false;
            this.groupBoxGraphControl.Text = "Graph control";
            // 
            // labelLastUpdated
            // 
            this.labelLastUpdated.AutoSize = true;
            this.labelLastUpdated.Location = new System.Drawing.Point(6, 45);
            this.labelLastUpdated.Name = "labelLastUpdated";
            this.labelLastUpdated.Size = new System.Drawing.Size(184, 13);
            this.labelLastUpdated.TabIndex = 3;
            this.labelLastUpdated.Text = "Last Refreshed: 2018-04-06 16:00:00";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(243, 525);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(418, 176);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(410, 150);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(410, 150);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 713);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBoxGraphControl);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "Chart";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Chart_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxGraphControl.ResumeLayout(false);
            this.groupBoxGraphControl.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.Button btOdabirGrafa;
        private System.Windows.Forms.GroupBox groupBoxGraphControl;
        private System.Windows.Forms.Label labelLastUpdated;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

