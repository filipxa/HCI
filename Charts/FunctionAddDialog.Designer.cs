namespace Charts
{
    partial class FunctionAddDialog
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
            this.comboBoxFun = new System.Windows.Forms.ComboBox();
            this.labelFun = new System.Windows.Forms.Label();
            this.listBoxSym = new System.Windows.Forms.ListBox();
            this.tbSym = new System.Windows.Forms.TextBox();
            this.labelSym1 = new System.Windows.Forms.Label();
            this.listBoxMarket = new System.Windows.Forms.ListBox();
            this.tbMarket = new System.Windows.Forms.TextBox();
            this.labelMarket = new System.Windows.Forms.Label();
            this.groupBoxReq = new System.Windows.Forms.GroupBox();
            this.groupBoxOptional = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.BtDone = new System.Windows.Forms.Button();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.labelInterval = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxFun
            // 
            this.comboBoxFun.BackColor = System.Drawing.Color.Lavender;
            this.comboBoxFun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFun.FormattingEnabled = true;
            this.comboBoxFun.Location = new System.Drawing.Point(12, 35);
            this.comboBoxFun.Name = "comboBoxFun";
            this.comboBoxFun.Size = new System.Drawing.Size(150, 21);
            this.comboBoxFun.TabIndex = 1;
            this.comboBoxFun.SelectedIndexChanged += new System.EventHandler(this.comboBoxFun_SelectedIndexChanged);
            // 
            // labelFun
            // 
            this.labelFun.AutoSize = true;
            this.labelFun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFun.ForeColor = System.Drawing.Color.Goldenrod;
            this.labelFun.Location = new System.Drawing.Point(11, 13);
            this.labelFun.Name = "labelFun";
            this.labelFun.Size = new System.Drawing.Size(79, 20);
            this.labelFun.TabIndex = 2;
            this.labelFun.Text = "Function";
            // 
            // listBoxSym
            // 
            this.listBoxSym.BackColor = System.Drawing.Color.Lavender;
            this.listBoxSym.FormattingEnabled = true;
            this.listBoxSym.Location = new System.Drawing.Point(171, 62);
            this.listBoxSym.Name = "listBoxSym";
            this.listBoxSym.Size = new System.Drawing.Size(267, 407);
            this.listBoxSym.TabIndex = 3;
            // 
            // tbSym
            // 
            this.tbSym.BackColor = System.Drawing.Color.Lavender;
            this.tbSym.Location = new System.Drawing.Point(172, 35);
            this.tbSym.Name = "tbSym";
            this.tbSym.Size = new System.Drawing.Size(266, 20);
            this.tbSym.TabIndex = 4;
            this.tbSym.TextChanged += new System.EventHandler(this.tbSym_TextChanged);
            // 
            // labelSym1
            // 
            this.labelSym1.AutoSize = true;
            this.labelSym1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSym1.ForeColor = System.Drawing.Color.Goldenrod;
            this.labelSym1.Location = new System.Drawing.Point(168, 13);
            this.labelSym1.Name = "labelSym1";
            this.labelSym1.Size = new System.Drawing.Size(67, 20);
            this.labelSym1.TabIndex = 5;
            this.labelSym1.Text = "Symbol";
            // 
            // listBoxMarket
            // 
            this.listBoxMarket.BackColor = System.Drawing.Color.Lavender;
            this.listBoxMarket.FormattingEnabled = true;
            this.listBoxMarket.Location = new System.Drawing.Point(466, 62);
            this.listBoxMarket.Name = "listBoxMarket";
            this.listBoxMarket.Size = new System.Drawing.Size(267, 407);
            this.listBoxMarket.TabIndex = 3;
            // 
            // tbMarket
            // 
            this.tbMarket.BackColor = System.Drawing.Color.Lavender;
            this.tbMarket.Location = new System.Drawing.Point(467, 36);
            this.tbMarket.Name = "tbMarket";
            this.tbMarket.Size = new System.Drawing.Size(266, 20);
            this.tbMarket.TabIndex = 4;
            this.tbMarket.TextChanged += new System.EventHandler(this.tbMarket_TextChanged);
            // 
            // labelMarket
            // 
            this.labelMarket.AutoSize = true;
            this.labelMarket.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMarket.ForeColor = System.Drawing.Color.Goldenrod;
            this.labelMarket.Location = new System.Drawing.Point(462, 13);
            this.labelMarket.Name = "labelMarket";
            this.labelMarket.Size = new System.Drawing.Size(64, 20);
            this.labelMarket.TabIndex = 5;
            this.labelMarket.Text = "Market";
            // 
            // groupBoxReq
            // 
            this.groupBoxReq.BackColor = System.Drawing.Color.DarkSlateGray;
            this.groupBoxReq.ForeColor = System.Drawing.Color.White;
            this.groupBoxReq.Location = new System.Drawing.Point(11, 62);
            this.groupBoxReq.Name = "groupBoxReq";
            this.groupBoxReq.Size = new System.Drawing.Size(149, 171);
            this.groupBoxReq.TabIndex = 6;
            this.groupBoxReq.TabStop = false;
            this.groupBoxReq.Text = "Required Parameters";
            //this.groupBoxReq.Enter += new System.EventHandler(this.groupBoxReq_Enter);
            // 
            // groupBoxOptional
            // 
            this.groupBoxOptional.BackColor = System.Drawing.Color.DarkSlateGray;
            this.groupBoxOptional.ForeColor = System.Drawing.Color.White;
            this.groupBoxOptional.Location = new System.Drawing.Point(11, 249);
            this.groupBoxOptional.Name = "groupBoxOptional";
            this.groupBoxOptional.Size = new System.Drawing.Size(149, 165);
            this.groupBoxOptional.TabIndex = 6;
            this.groupBoxOptional.TabStop = false;
            this.groupBoxOptional.Text = "Optional Parameters";
            //this.groupBoxOptional.Enter += new System.EventHandler(this.groupBoxOptional_Enter);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(466, 513);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // BtDone
            // 
            this.BtDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtDone.Location = new System.Drawing.Point(630, 513);
            this.BtDone.Name = "BtDone";
            this.BtDone.Size = new System.Drawing.Size(103, 26);
            this.BtDone.TabIndex = 7;
            this.BtDone.Text = "Draw";
            this.BtDone.UseVisualStyleBackColor = true;
            this.BtDone.Click += new System.EventHandler(this.btDone_Click);
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.BackColor = System.Drawing.Color.Lavender;
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Location = new System.Drawing.Point(10, 448);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(150, 21);
            this.comboBoxInterval.TabIndex = 1;
            this.comboBoxInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxFun_SelectedIndexChanged);
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.ForeColor = System.Drawing.Color.White;
            this.labelInterval.Location = new System.Drawing.Point(7, 432);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(42, 13);
            this.labelInterval.TabIndex = 2;
            this.labelInterval.Text = "Interval";
            // 
            // FunctionAddDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.BtDone);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBoxOptional);
            this.Controls.Add(this.groupBoxReq);
            this.Controls.Add(this.labelMarket);
            this.Controls.Add(this.labelSym1);
            this.Controls.Add(this.tbMarket);
            this.Controls.Add(this.tbSym);
            this.Controls.Add(this.listBoxMarket);
            this.Controls.Add(this.listBoxSym);
            this.Controls.Add(this.labelInterval);
            this.Controls.Add(this.labelFun);
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.comboBoxFun);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FunctionAddDialog";
            this.Text = "Add Graph";
            this.Load += new System.EventHandler(this.FunctionAddDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxFun;
        private System.Windows.Forms.Label labelFun;
        private System.Windows.Forms.ListBox listBoxSym;
        private System.Windows.Forms.TextBox tbSym;
        private System.Windows.Forms.Label labelSym1;
        private System.Windows.Forms.ListBox listBoxMarket;
        private System.Windows.Forms.TextBox tbMarket;
        private System.Windows.Forms.Label labelMarket;
        private System.Windows.Forms.GroupBox groupBoxReq;
        internal System.Windows.Forms.GroupBox groupBoxOptional;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtDone;
        private System.Windows.Forms.ComboBox comboBoxInterval;
        private System.Windows.Forms.Label labelInterval;
    }
}