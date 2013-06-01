namespace StarEdit.EnemyEditor
{
    partial class OffsetForm
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonAbort = new System.Windows.Forms.Button();
			this.numericUpDownOffset = new System.Windows.Forms.NumericUpDown();
			this.comboBoxRectangle = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxFrameOffset = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(5, 159);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonAbort
			// 
			this.buttonAbort.Location = new System.Drawing.Point(204, 159);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(75, 23);
			this.buttonAbort.TabIndex = 0;
			this.buttonAbort.Text = "Abort";
			this.buttonAbort.UseVisualStyleBackColor = true;
			this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
			// 
			// numericUpDownOffset
			// 
			this.numericUpDownOffset.Location = new System.Drawing.Point(9, 22);
			this.numericUpDownOffset.Name = "numericUpDownOffset";
			this.numericUpDownOffset.Size = new System.Drawing.Size(70, 20);
			this.numericUpDownOffset.TabIndex = 1;
			// 
			// comboBoxRectangle
			// 
			this.comboBoxRectangle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxRectangle.FormattingEnabled = true;
			this.comboBoxRectangle.Location = new System.Drawing.Point(144, 21);
			this.comboBoxRectangle.Name = "comboBoxRectangle";
			this.comboBoxRectangle.Size = new System.Drawing.Size(121, 21);
			this.comboBoxRectangle.TabIndex = 3;
			this.comboBoxRectangle.SelectedIndexChanged += new System.EventHandler(this.comboBoxRectangle_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxRectangle);
			this.groupBox1.Controls.Add(this.numericUpDownOffset);
			this.groupBox1.Enabled = false;
			this.groupBox1.Location = new System.Drawing.Point(5, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(271, 48);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// checkBoxFrameOffset
			// 
			this.checkBoxFrameOffset.AutoSize = true;
			this.checkBoxFrameOffset.Location = new System.Drawing.Point(11, 6);
			this.checkBoxFrameOffset.Name = "checkBoxFrameOffset";
			this.checkBoxFrameOffset.Size = new System.Drawing.Size(86, 17);
			this.checkBoxFrameOffset.TabIndex = 5;
			this.checkBoxFrameOffset.Text = "Frame Offset";
			this.checkBoxFrameOffset.UseVisualStyleBackColor = true;
			this.checkBoxFrameOffset.CheckedChanged += new System.EventHandler(this.checkBoxFrameOffset_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.numericUpDown2);
			this.groupBox2.Controls.Add(this.numericUpDown1);
			this.groupBox2.Enabled = false;
			this.groupBox2.Location = new System.Drawing.Point(5, 93);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(271, 60);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(8, 89);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(94, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Position Offset";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(35, 27);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(55, 20);
			this.numericUpDown1.TabIndex = 1;
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(128, 27);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            -2147483648});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(55, 20);
			this.numericUpDown2.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(17, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "X:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(105, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Y:";
			// 
			// OffsetForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(291, 194);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.checkBoxFrameOffset);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonAbort);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OffsetForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "OffsetForm";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonAbort;
		private System.Windows.Forms.NumericUpDown numericUpDownOffset;
        private System.Windows.Forms.ComboBox comboBoxRectangle;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBoxFrameOffset;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.CheckBox checkBox1;
    }
}