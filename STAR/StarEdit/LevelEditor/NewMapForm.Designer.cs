namespace StarEdit.Editor
{
    partial class NewMapForm
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
            this.XUpDownNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.YUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.NewButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.XUpDownNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YUpDown2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // XUpDownNumericUpDown
            // 
            this.XUpDownNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XUpDownNumericUpDown.Location = new System.Drawing.Point(83, 19);
            this.XUpDownNumericUpDown.Maximum = new decimal(new int[] {
            -45967296,
            0,
            0,
            0});
            this.XUpDownNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XUpDownNumericUpDown.Name = "XUpDownNumericUpDown";
            this.XUpDownNumericUpDown.Size = new System.Drawing.Size(72, 20);
            this.XUpDownNumericUpDown.TabIndex = 0;
            this.XUpDownNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.XUpDownNumericUpDown.ValueChanged += new System.EventHandler(this.XUpDownNumericUpDown_ValueChanged);
            // 
            // YUpDown2
            // 
            this.YUpDown2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YUpDown2.Location = new System.Drawing.Point(83, 45);
            this.YUpDown2.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.YUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.YUpDown2.Name = "YUpDown2";
            this.YUpDown2.Size = new System.Drawing.Size(72, 20);
            this.YUpDown2.TabIndex = 1;
            this.YUpDown2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YUpDown2.ValueChanged += new System.EventHandler(this.YUpDown2_ValueChanged);
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(12, 83);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(70, 20);
            this.NewButton.TabIndex = 2;
            this.NewButton.Text = "New...";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(88, 83);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(85, 20);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.YUpDown2);
            this.groupBox1.Controls.Add(this.XUpDownNumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 71);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Size Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size X:";
            // 
            // NewMapForm
            // 
            this.AcceptButton = this.NewButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 111);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.NewButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NewMapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.XUpDownNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YUpDown2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown XUpDownNumericUpDown;
        private System.Windows.Forms.NumericUpDown YUpDown2;
        private System.Windows.Forms.Button NewButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}