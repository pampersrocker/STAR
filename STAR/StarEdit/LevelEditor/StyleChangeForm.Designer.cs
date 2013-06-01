namespace StarEdit.Editor
{
    partial class StyleChangeForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FrontParallaxSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.RearparallaxSpeedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FrontParallaxBox = new System.Windows.Forms.ComboBox();
            this.RearparallaxBox = new System.Windows.Forms.ComboBox();
            this.BackgroundImgBox = new System.Windows.Forms.ComboBox();
            this.GraphXPackBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.loadbutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrontParallaxSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RearparallaxSpeedNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.FrontParallaxSpeedUpDown);
            this.groupBox1.Controls.Add(this.RearparallaxSpeedNumericUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.FrontParallaxBox);
            this.groupBox1.Controls.Add(this.RearparallaxBox);
            this.groupBox1.Controls.Add(this.BackgroundImgBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 182);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "FrontParallaxSpeedDivider";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "RearParallaxSpeedDivider";
            // 
            // FrontParallaxSpeedUpDown
            // 
            this.FrontParallaxSpeedUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrontParallaxSpeedUpDown.DecimalPlaces = 3;
            this.FrontParallaxSpeedUpDown.Location = new System.Drawing.Point(171, 126);
            this.FrontParallaxSpeedUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.FrontParallaxSpeedUpDown.Name = "FrontParallaxSpeedUpDown";
            this.FrontParallaxSpeedUpDown.Size = new System.Drawing.Size(120, 20);
            this.FrontParallaxSpeedUpDown.TabIndex = 8;
            this.FrontParallaxSpeedUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // RearparallaxSpeedNumericUpDown
            // 
            this.RearparallaxSpeedNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RearparallaxSpeedNumericUpDown.DecimalPlaces = 3;
            this.RearparallaxSpeedNumericUpDown.Location = new System.Drawing.Point(171, 73);
            this.RearparallaxSpeedNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.RearparallaxSpeedNumericUpDown.Name = "RearparallaxSpeedNumericUpDown";
            this.RearparallaxSpeedNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.RearparallaxSpeedNumericUpDown.TabIndex = 7;
            this.RearparallaxSpeedNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "FrontParallaxImg";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RearparallaxImg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "BackgroundImg";
            // 
            // FrontParallaxBox
            // 
            this.FrontParallaxBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrontParallaxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FrontParallaxBox.FormattingEnabled = true;
            this.FrontParallaxBox.Location = new System.Drawing.Point(171, 99);
            this.FrontParallaxBox.Name = "FrontParallaxBox";
            this.FrontParallaxBox.Size = new System.Drawing.Size(121, 21);
            this.FrontParallaxBox.TabIndex = 2;
            // 
            // RearparallaxBox
            // 
            this.RearparallaxBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RearparallaxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RearparallaxBox.FormattingEnabled = true;
            this.RearparallaxBox.Location = new System.Drawing.Point(171, 46);
            this.RearparallaxBox.Name = "RearparallaxBox";
            this.RearparallaxBox.Size = new System.Drawing.Size(121, 21);
            this.RearparallaxBox.TabIndex = 1;
            // 
            // BackgroundImgBox
            // 
            this.BackgroundImgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundImgBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BackgroundImgBox.FormattingEnabled = true;
            this.BackgroundImgBox.Location = new System.Drawing.Point(171, 19);
            this.BackgroundImgBox.Name = "BackgroundImgBox";
            this.BackgroundImgBox.Size = new System.Drawing.Size(121, 21);
            this.BackgroundImgBox.Sorted = true;
            this.BackgroundImgBox.TabIndex = 0;
            // 
            // GraphXPackBox
            // 
            this.GraphXPackBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GraphXPackBox.FormattingEnabled = true;
            this.GraphXPackBox.Location = new System.Drawing.Point(183, 10);
            this.GraphXPackBox.Name = "GraphXPackBox";
            this.GraphXPackBox.Size = new System.Drawing.Size(121, 21);
            this.GraphXPackBox.TabIndex = 3;
            this.GraphXPackBox.SelectedIndexChanged += new System.EventHandler(this.GraphXPackBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "GraphXPack";
            // 
            // loadbutton
            // 
            this.loadbutton.Location = new System.Drawing.Point(12, 225);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(103, 22);
            this.loadbutton.TabIndex = 8;
            this.loadbutton.Text = "Load";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton.Location = new System.Drawing.Point(214, 225);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(96, 21);
            this.cancelbutton.TabIndex = 9;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Click += new System.EventHandler(this.cancelbutton_Click);
            // 
            // StyleChangeForm
            // 
            this.AcceptButton = this.loadbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton;
            this.ClientSize = new System.Drawing.Size(322, 254);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.loadbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GraphXPackBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleChangeForm";
            this.Text = "StyleChangeForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrontParallaxSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RearparallaxSpeedNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox GraphXPackBox;
        private System.Windows.Forms.ComboBox FrontParallaxBox;
        private System.Windows.Forms.ComboBox RearparallaxBox;
        private System.Windows.Forms.ComboBox BackgroundImgBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FrontParallaxSpeedUpDown;
        private System.Windows.Forms.NumericUpDown RearparallaxSpeedNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.Button cancelbutton;

    }
}