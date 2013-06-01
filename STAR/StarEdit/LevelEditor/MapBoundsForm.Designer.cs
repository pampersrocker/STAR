namespace StarEdit.LevelEditor
{
    partial class MapBoundsForm
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
            this.PositionXBox = new System.Windows.Forms.NumericUpDown();
            this.PositionYBox = new System.Windows.Forms.NumericUpDown();
            this.SizeXBox = new System.Windows.Forms.NumericUpDown();
            this.SizeYBox = new System.Windows.Forms.NumericUpDown();
            this.PositionGroup = new System.Windows.Forms.GroupBox();
            this.SizeGroup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeYBox)).BeginInit();
            this.PositionGroup.SuspendLayout();
            this.SizeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // PositionXBox
            // 
            this.PositionXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionXBox.Location = new System.Drawing.Point(31, 14);
            this.PositionXBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PositionXBox.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.PositionXBox.Name = "PositionXBox";
            this.PositionXBox.Size = new System.Drawing.Size(57, 20);
            this.PositionXBox.TabIndex = 0;
            this.PositionXBox.ValueChanged += new System.EventHandler(this.PositionXBox_ValueChanged);
            // 
            // PositionYBox
            // 
            this.PositionYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionYBox.Location = new System.Drawing.Point(31, 51);
            this.PositionYBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PositionYBox.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.PositionYBox.Name = "PositionYBox";
            this.PositionYBox.Size = new System.Drawing.Size(57, 20);
            this.PositionYBox.TabIndex = 1;
            this.PositionYBox.ValueChanged += new System.EventHandler(this.PositionYBox_ValueChanged);
            // 
            // SizeXBox
            // 
            this.SizeXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SizeXBox.Location = new System.Drawing.Point(33, 14);
            this.SizeXBox.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SizeXBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SizeXBox.Name = "SizeXBox";
            this.SizeXBox.Size = new System.Drawing.Size(57, 20);
            this.SizeXBox.TabIndex = 2;
            this.SizeXBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SizeYBox
            // 
            this.SizeYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SizeYBox.Location = new System.Drawing.Point(33, 51);
            this.SizeYBox.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SizeYBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SizeYBox.Name = "SizeYBox";
            this.SizeYBox.Size = new System.Drawing.Size(57, 20);
            this.SizeYBox.TabIndex = 3;
            this.SizeYBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PositionGroup
            // 
            this.PositionGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PositionGroup.Controls.Add(this.label2);
            this.PositionGroup.Controls.Add(this.label1);
            this.PositionGroup.Controls.Add(this.PositionYBox);
            this.PositionGroup.Controls.Add(this.PositionXBox);
            this.PositionGroup.Location = new System.Drawing.Point(9, 12);
            this.PositionGroup.Name = "PositionGroup";
            this.PositionGroup.Size = new System.Drawing.Size(94, 77);
            this.PositionGroup.TabIndex = 4;
            this.PositionGroup.TabStop = false;
            this.PositionGroup.Text = "Position";
            // 
            // SizeGroup
            // 
            this.SizeGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SizeGroup.Controls.Add(this.label3);
            this.SizeGroup.Controls.Add(this.SizeYBox);
            this.SizeGroup.Controls.Add(this.label4);
            this.SizeGroup.Controls.Add(this.SizeXBox);
            this.SizeGroup.Location = new System.Drawing.Point(109, 12);
            this.SizeGroup.Name = "SizeGroup";
            this.SizeGroup.Size = new System.Drawing.Size(101, 77);
            this.SizeGroup.TabIndex = 5;
            this.SizeGroup.TabStop = false;
            this.SizeGroup.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "X:";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(9, 95);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(135, 95);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // MapBoundsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 126);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.SizeGroup);
            this.Controls.Add(this.PositionGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapBoundsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MapBounds";
            ((System.ComponentModel.ISupportInitialize)(this.PositionXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeYBox)).EndInit();
            this.PositionGroup.ResumeLayout(false);
            this.PositionGroup.PerformLayout();
            this.SizeGroup.ResumeLayout(false);
            this.SizeGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown PositionXBox;
        private System.Windows.Forms.NumericUpDown PositionYBox;
        private System.Windows.Forms.NumericUpDown SizeXBox;
        private System.Windows.Forms.NumericUpDown SizeYBox;
        private System.Windows.Forms.GroupBox PositionGroup;
        private System.Windows.Forms.GroupBox SizeGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button cancel;
    }
}