namespace StarEdit.EnemyEditor
{
    partial class AddRectangleForm
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
            this.OpenImg = new System.Windows.Forms.OpenFileDialog();
            this.rectangleGroupBox = new System.Windows.Forms.GroupBox();
            this.NameGroupBox = new System.Windows.Forms.GroupBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.layerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.yOriginNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.xOriginNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.heightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.yPositionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.xPositionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.aColorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bColorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.gColorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rColorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rotationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.loadButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.imagePathTextBox = new System.Windows.Forms.TextBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.imageGroupBox = new System.Windows.Forms.GroupBox();
            this.rectangleGroupBox.SuspendLayout();
            this.NameGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layerNumericUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yOriginNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOriginNumericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPositionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xPositionNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aColorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bColorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gColorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rColorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationNumericUpDown)).BeginInit();
            this.imageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenImg
            // 
            this.OpenImg.DefaultExt = "png";
            this.OpenImg.FileName = "Picture";
            this.OpenImg.Title = "Open Picture";
            // 
            // rectangleGroupBox
            // 
            this.rectangleGroupBox.Controls.Add(this.NameGroupBox);
            this.rectangleGroupBox.Controls.Add(this.label9);
            this.rectangleGroupBox.Controls.Add(this.layerNumericUpDown);
            this.rectangleGroupBox.Controls.Add(this.groupBox4);
            this.rectangleGroupBox.Controls.Add(this.label6);
            this.rectangleGroupBox.Controls.Add(this.groupBox3);
            this.rectangleGroupBox.Controls.Add(this.groupBox2);
            this.rectangleGroupBox.Controls.Add(this.rotationNumericUpDown);
            this.rectangleGroupBox.Location = new System.Drawing.Point(12, 67);
            this.rectangleGroupBox.Name = "rectangleGroupBox";
            this.rectangleGroupBox.Size = new System.Drawing.Size(389, 148);
            this.rectangleGroupBox.TabIndex = 5;
            this.rectangleGroupBox.TabStop = false;
            this.rectangleGroupBox.Text = "Rectangle";
            // 
            // NameGroupBox
            // 
            this.NameGroupBox.Controls.Add(this.nameTextBox);
            this.NameGroupBox.Location = new System.Drawing.Point(10, 92);
            this.NameGroupBox.Name = "NameGroupBox";
            this.NameGroupBox.Size = new System.Drawing.Size(133, 46);
            this.NameGroupBox.TabIndex = 16;
            this.NameGroupBox.TabStop = false;
            this.NameGroupBox.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(6, 19);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(120, 20);
            this.nameTextBox.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(226, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Layer:";
            // 
            // layerNumericUpDown
            // 
            this.layerNumericUpDown.DecimalPlaces = 5;
            this.layerNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.layerNumericUpDown.Location = new System.Drawing.Point(286, 90);
            this.layerNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.layerNumericUpDown.Name = "layerNumericUpDown";
            this.layerNumericUpDown.Size = new System.Drawing.Size(63, 20);
            this.layerNumericUpDown.TabIndex = 12;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.yOriginNumericUpDown);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.xOriginNumericUpDown);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(223, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(133, 42);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Origin:";
            // 
            // yOriginNumericUpDown
            // 
            this.yOriginNumericUpDown.Location = new System.Drawing.Point(87, 16);
            this.yOriginNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yOriginNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.yOriginNumericUpDown.Name = "yOriginNumericUpDown";
            this.yOriginNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.yOriginNumericUpDown.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "X:";
            // 
            // xOriginNumericUpDown
            // 
            this.xOriginNumericUpDown.Location = new System.Drawing.Point(22, 16);
            this.xOriginNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.xOriginNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.xOriginNumericUpDown.Name = "xOriginNumericUpDown";
            this.xOriginNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.xOriginNumericUpDown.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(64, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Rotation:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.heightNumericUpDown);
            this.groupBox3.Controls.Add(this.widthNumericUpDown);
            this.groupBox3.Controls.Add(this.yPositionNumericUpDown);
            this.groupBox3.Controls.Add(this.xPositionNumericUpDown);
            this.groupBox3.Location = new System.Drawing.Point(10, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(134, 74);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "H:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "W:";
            // 
            // heightNumericUpDown
            // 
            this.heightNumericUpDown.Location = new System.Drawing.Point(87, 45);
            this.heightNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.heightNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.heightNumericUpDown.Name = "heightNumericUpDown";
            this.heightNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.heightNumericUpDown.TabIndex = 4;
            this.heightNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // widthNumericUpDown
            // 
            this.widthNumericUpDown.Location = new System.Drawing.Point(87, 19);
            this.widthNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.widthNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.widthNumericUpDown.Name = "widthNumericUpDown";
            this.widthNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.widthNumericUpDown.TabIndex = 3;
            this.widthNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // yPositionNumericUpDown
            // 
            this.yPositionNumericUpDown.Location = new System.Drawing.Point(22, 45);
            this.yPositionNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yPositionNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.yPositionNumericUpDown.Name = "yPositionNumericUpDown";
            this.yPositionNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.yPositionNumericUpDown.TabIndex = 2;
            // 
            // xPositionNumericUpDown
            // 
            this.xPositionNumericUpDown.Location = new System.Drawing.Point(22, 19);
            this.xPositionNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.xPositionNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.xPositionNumericUpDown.Name = "xPositionNumericUpDown";
            this.xPositionNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.xPositionNumericUpDown.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.aColorNumericUpDown);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.bColorNumericUpDown);
            this.groupBox2.Controls.Add(this.gColorNumericUpDown);
            this.groupBox2.Controls.Add(this.rColorNumericUpDown);
            this.groupBox2.Location = new System.Drawing.Point(150, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(67, 123);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RGBA";
            // 
            // aColorNumericUpDown
            // 
            this.aColorNumericUpDown.Location = new System.Drawing.Point(23, 97);
            this.aColorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.aColorNumericUpDown.Name = "aColorNumericUpDown";
            this.aColorNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.aColorNumericUpDown.TabIndex = 8;
            this.aColorNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "A:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "B:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "G:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "R:";
            // 
            // bColorNumericUpDown
            // 
            this.bColorNumericUpDown.Location = new System.Drawing.Point(23, 71);
            this.bColorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bColorNumericUpDown.Name = "bColorNumericUpDown";
            this.bColorNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.bColorNumericUpDown.TabIndex = 7;
            this.bColorNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // gColorNumericUpDown
            // 
            this.gColorNumericUpDown.Location = new System.Drawing.Point(23, 45);
            this.gColorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.gColorNumericUpDown.Name = "gColorNumericUpDown";
            this.gColorNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.gColorNumericUpDown.TabIndex = 6;
            this.gColorNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // rColorNumericUpDown
            // 
            this.rColorNumericUpDown.Location = new System.Drawing.Point(23, 19);
            this.rColorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.rColorNumericUpDown.Name = "rColorNumericUpDown";
            this.rColorNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.rColorNumericUpDown.TabIndex = 5;
            this.rColorNumericUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // rotationNumericUpDown
            // 
            this.rotationNumericUpDown.DecimalPlaces = 5;
            this.rotationNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.rotationNumericUpDown.Location = new System.Drawing.Point(286, 64);
            this.rotationNumericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.rotationNumericUpDown.Name = "rotationNumericUpDown";
            this.rotationNumericUpDown.Size = new System.Drawing.Size(63, 20);
            this.rotationNumericUpDown.TabIndex = 11;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 221);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 14;
            this.loadButton.Text = "Load...";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.Location = new System.Drawing.Point(326, 221);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(75, 23);
            this.abortButton.TabIndex = 15;
            this.abortButton.Text = "Abort";
            this.abortButton.UseVisualStyleBackColor = true;
            // 
            // imagePathTextBox
            // 
            this.imagePathTextBox.Location = new System.Drawing.Point(6, 19);
            this.imagePathTextBox.Name = "imagePathTextBox";
            this.imagePathTextBox.Size = new System.Drawing.Size(302, 20);
            this.imagePathTextBox.TabIndex = 0;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(314, 18);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(69, 20);
            this.selectButton.TabIndex = 0;
            this.selectButton.Text = "...";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // imageGroupBox
            // 
            this.imageGroupBox.Controls.Add(this.selectButton);
            this.imageGroupBox.Controls.Add(this.imagePathTextBox);
            this.imageGroupBox.Location = new System.Drawing.Point(12, 12);
            this.imageGroupBox.Name = "imageGroupBox";
            this.imageGroupBox.Size = new System.Drawing.Size(389, 49);
            this.imageGroupBox.TabIndex = 0;
            this.imageGroupBox.TabStop = false;
            this.imageGroupBox.Text = "Image";
            // 
            // AddRectangleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 249);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.rectangleGroupBox);
            this.Controls.Add(this.imageGroupBox);
            this.Name = "AddRectangleForm";
            this.ShowInTaskbar = false;
            this.Text = "AddRectangleForm";
            this.rectangleGroupBox.ResumeLayout(false);
            this.rectangleGroupBox.PerformLayout();
            this.NameGroupBox.ResumeLayout(false);
            this.NameGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layerNumericUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yOriginNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOriginNumericUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPositionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xPositionNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aColorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bColorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gColorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rColorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationNumericUpDown)).EndInit();
            this.imageGroupBox.ResumeLayout(false);
            this.imageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenImg;
        private System.Windows.Forms.GroupBox rectangleGroupBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown layerNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown yOriginNumericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown xOriginNumericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown heightNumericUpDown;
        private System.Windows.Forms.NumericUpDown widthNumericUpDown;
        private System.Windows.Forms.NumericUpDown yPositionNumericUpDown;
        private System.Windows.Forms.NumericUpDown xPositionNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown aColorNumericUpDown;
        private System.Windows.Forms.NumericUpDown bColorNumericUpDown;
        private System.Windows.Forms.NumericUpDown gColorNumericUpDown;
        private System.Windows.Forms.NumericUpDown rColorNumericUpDown;
        private System.Windows.Forms.NumericUpDown rotationNumericUpDown;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.GroupBox NameGroupBox;
        private System.Windows.Forms.TextBox imagePathTextBox;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.GroupBox imageGroupBox;
    }
}