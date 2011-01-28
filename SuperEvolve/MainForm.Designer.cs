namespace SuperImageEvolver {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.mutationStats = new System.Windows.Forms.TextBox();
            this.diffCanvas1 = new SuperImageEvolver.DiffCanvas();
            this.canvas1 = new SuperImageEvolver.Canvas();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point( 3, 3 );
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size( 165, 71 );
            this.textBox1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point( 3, 3 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size( 35, 38 );
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add( this.pictureBox1 );
            this.flowLayoutPanel1.Controls.Add( this.canvas1 );
            this.flowLayoutPanel1.Controls.Add( this.diffCanvas1 );
            this.flowLayoutPanel1.Controls.Add( this.panel1 );
            this.flowLayoutPanel1.Controls.Add( this.mutationStats );
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size( 618, 336 );
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.label5 );
            this.panel1.Controls.Add( this.comboBox3 );
            this.panel1.Controls.Add( this.label4 );
            this.panel1.Controls.Add( this.label3 );
            this.panel1.Controls.Add( this.comboBox2 );
            this.panel1.Controls.Add( this.comboBox1 );
            this.panel1.Controls.Add( this.label2 );
            this.panel1.Controls.Add( this.label1 );
            this.panel1.Controls.Add( this.textBox1 );
            this.panel1.Controls.Add( this.numericUpDown1 );
            this.panel1.Controls.Add( this.numericUpDown2 );
            this.panel1.Controls.Add( this.button1 );
            this.panel1.Location = new System.Drawing.Point( 133, 3 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 171, 215 );
            this.panel1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 3, 190 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 52, 13 );
            this.label5.TabIndex = 13;
            this.label5.Text = "Evaluator";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange( new object[] {
            "RGB (Fast)",
            "RGB (Smooth)",
            "RGB+Luma (Fast)",
            "RGB+Luma (Smooth)"} );
            this.comboBox3.Location = new System.Drawing.Point( 61, 187 );
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size( 107, 21 );
            this.comboBox3.TabIndex = 12;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler( this.comboBox3_SelectedIndexChanged );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 12, 163 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 43, 13 );
            this.label4.TabIndex = 11;
            this.label4.Text = "Mutator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 8, 136 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 47, 13 );
            this.label3.TabIndex = 10;
            this.label3.Text = "Initializer";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange( new object[] {
            "Harder",
            "Hard",
            "Medium",
            "Soft",
            "Softer"} );
            this.comboBox2.Location = new System.Drawing.Point( 61, 160 );
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size( 107, 21 );
            this.comboBox2.TabIndex = 9;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler( this.comboBox2_SelectedIndexChanged );
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange( new object[] {
            "FullRandom",
            "Segmented"} );
            this.comboBox1.Location = new System.Drawing.Point( 61, 133 );
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size( 107, 21 );
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler( this.comboBox1_SelectedIndexChanged );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 10, 109 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 45, 13 );
            this.label2.TabIndex = 7;
            this.label2.Text = "Vertices";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 5, 82 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 50, 13 );
            this.label1.TabIndex = 6;
            this.label1.Text = "Polygons";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point( 61, 80 );
            this.numericUpDown1.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.numericUpDown1.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size( 43, 20 );
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point( 61, 107 );
            this.numericUpDown2.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.numericUpDown2.Minimum = new decimal( new int[] {
            3,
            0,
            0,
            0} );
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size( 43, 20 );
            this.numericUpDown2.TabIndex = 5;
            this.numericUpDown2.Value = new decimal( new int[] {
            6,
            0,
            0,
            0} );
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 110, 80 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 58, 47 );
            this.button1.TabIndex = 3;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler( this.button1_Click );
            // 
            // mutationStats
            // 
            this.mutationStats.Location = new System.Drawing.Point( 310, 3 );
            this.mutationStats.Multiline = true;
            this.mutationStats.Name = "mutationStats";
            this.mutationStats.ReadOnly = true;
            this.mutationStats.Size = new System.Drawing.Size( 194, 215 );
            this.mutationStats.TabIndex = 7;
            // 
            // diffCanvas1
            // 
            this.diffCanvas1.Location = new System.Drawing.Point( 89, 3 );
            this.diffCanvas1.Name = "diffCanvas1";
            this.diffCanvas1.Size = new System.Drawing.Size( 38, 38 );
            this.diffCanvas1.TabIndex = 8;
            // 
            // canvas1
            // 
            this.canvas1.Location = new System.Drawing.Point( 44, 3 );
            this.canvas1.Name = "canvas1";
            this.canvas1.Size = new System.Drawing.Size( 39, 38 );
            this.canvas1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 618, 336 );
            this.Controls.Add( this.flowLayoutPanel1 );
            this.Name = "MainForm";
            this.Text = "Super Image Evolver";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout( false );
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private Canvas canvas1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox mutationStats;
        private DiffCanvas diffCanvas1;
    }
}

