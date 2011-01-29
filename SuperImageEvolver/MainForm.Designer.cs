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
            this.tTaskStats = new System.Windows.Forms.TextBox();
            this.picOriginal = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.picBestMatch = new SuperImageEvolver.Canvas();
            this.picDiff = new SuperImageEvolver.DiffCanvas();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lEvaluator = new System.Windows.Forms.Label();
            this.cEvaluator = new System.Windows.Forms.ComboBox();
            this.lMutator = new System.Windows.Forms.Label();
            this.lInitializer = new System.Windows.Forms.Label();
            this.cMutator = new System.Windows.Forms.ComboBox();
            this.cInitializer = new System.Windows.Forms.ComboBox();
            this.lVertices = new System.Windows.Forms.Label();
            this.lPolygons = new System.Windows.Forms.Label();
            this.nPolygons = new System.Windows.Forms.NumericUpDown();
            this.nVertices = new System.Windows.Forms.NumericUpDown();
            this.bStartStop = new System.Windows.Forms.Button();
            this.tMutationStats = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).BeginInit();
            this.SuspendLayout();
            // 
            // tTaskStats
            // 
            this.tTaskStats.Location = new System.Drawing.Point( 3, 3 );
            this.tTaskStats.Multiline = true;
            this.tTaskStats.Name = "tTaskStats";
            this.tTaskStats.ReadOnly = true;
            this.tTaskStats.Size = new System.Drawing.Size( 165, 71 );
            this.tTaskStats.TabIndex = 1;
            // 
            // picOriginal
            // 
            this.picOriginal.Location = new System.Drawing.Point( 3, 3 );
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size( 35, 38 );
            this.picOriginal.TabIndex = 2;
            this.picOriginal.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add( this.picOriginal );
            this.flowLayoutPanel1.Controls.Add( this.picBestMatch );
            this.flowLayoutPanel1.Controls.Add( this.picDiff );
            this.flowLayoutPanel1.Controls.Add( this.panel1 );
            this.flowLayoutPanel1.Controls.Add( this.tMutationStats );
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size( 595, 336 );
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // picBestMatch
            // 
            this.picBestMatch.Location = new System.Drawing.Point( 44, 3 );
            this.picBestMatch.Name = "picBestMatch";
            this.picBestMatch.Size = new System.Drawing.Size( 39, 38 );
            this.picBestMatch.TabIndex = 0;
            // 
            // picDiff
            // 
            this.picDiff.Location = new System.Drawing.Point( 89, 3 );
            this.picDiff.Name = "picDiff";
            this.picDiff.Size = new System.Drawing.Size( 38, 38 );
            this.picDiff.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.lEvaluator );
            this.panel1.Controls.Add( this.cEvaluator );
            this.panel1.Controls.Add( this.lMutator );
            this.panel1.Controls.Add( this.lInitializer );
            this.panel1.Controls.Add( this.cMutator );
            this.panel1.Controls.Add( this.cInitializer );
            this.panel1.Controls.Add( this.lVertices );
            this.panel1.Controls.Add( this.lPolygons );
            this.panel1.Controls.Add( this.tTaskStats );
            this.panel1.Controls.Add( this.nPolygons );
            this.panel1.Controls.Add( this.nVertices );
            this.panel1.Controls.Add( this.bStartStop );
            this.panel1.Location = new System.Drawing.Point( 133, 3 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 171, 215 );
            this.panel1.TabIndex = 6;
            // 
            // lEvaluator
            // 
            this.lEvaluator.AutoSize = true;
            this.lEvaluator.Location = new System.Drawing.Point( 3, 190 );
            this.lEvaluator.Name = "lEvaluator";
            this.lEvaluator.Size = new System.Drawing.Size( 52, 13 );
            this.lEvaluator.TabIndex = 13;
            this.lEvaluator.Text = "Evaluator";
            // 
            // cEvaluator
            // 
            this.cEvaluator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cEvaluator.FormattingEnabled = true;
            this.cEvaluator.Items.AddRange( new object[] {
            "RGB (Fast)",
            "RGB (Smooth)",
            "RGB+Luma (Fast)",
            "RGB+Luma (Smooth)"} );
            this.cEvaluator.Location = new System.Drawing.Point( 61, 187 );
            this.cEvaluator.Name = "cEvaluator";
            this.cEvaluator.Size = new System.Drawing.Size( 107, 21 );
            this.cEvaluator.TabIndex = 12;
            this.cEvaluator.SelectedIndexChanged += new System.EventHandler( this.cEvaluator_SelectedIndexChanged );
            // 
            // lMutator
            // 
            this.lMutator.AutoSize = true;
            this.lMutator.Location = new System.Drawing.Point( 12, 163 );
            this.lMutator.Name = "lMutator";
            this.lMutator.Size = new System.Drawing.Size( 43, 13 );
            this.lMutator.TabIndex = 11;
            this.lMutator.Text = "Mutator";
            // 
            // lInitializer
            // 
            this.lInitializer.AutoSize = true;
            this.lInitializer.Location = new System.Drawing.Point( 8, 136 );
            this.lInitializer.Name = "lInitializer";
            this.lInitializer.Size = new System.Drawing.Size( 47, 13 );
            this.lInitializer.TabIndex = 10;
            this.lInitializer.Text = "Initializer";
            // 
            // cMutator
            // 
            this.cMutator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMutator.FormattingEnabled = true;
            this.cMutator.Items.AddRange( new object[] {
            "Harder",
            "Hard",
            "Medium",
            "Soft",
            "Softer"} );
            this.cMutator.Location = new System.Drawing.Point( 61, 160 );
            this.cMutator.Name = "cMutator";
            this.cMutator.Size = new System.Drawing.Size( 107, 21 );
            this.cMutator.TabIndex = 9;
            this.cMutator.SelectedIndexChanged += new System.EventHandler( this.cMutator_SelectedIndexChanged );
            // 
            // cInitializer
            // 
            this.cInitializer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cInitializer.FormattingEnabled = true;
            this.cInitializer.Items.AddRange( new object[] {
            "FullRandom",
            "Segmented"} );
            this.cInitializer.Location = new System.Drawing.Point( 61, 133 );
            this.cInitializer.Name = "cInitializer";
            this.cInitializer.Size = new System.Drawing.Size( 107, 21 );
            this.cInitializer.TabIndex = 8;
            this.cInitializer.SelectedIndexChanged += new System.EventHandler( this.cInitializer_SelectedIndexChanged );
            // 
            // lVertices
            // 
            this.lVertices.AutoSize = true;
            this.lVertices.Location = new System.Drawing.Point( 10, 109 );
            this.lVertices.Name = "lVertices";
            this.lVertices.Size = new System.Drawing.Size( 45, 13 );
            this.lVertices.TabIndex = 7;
            this.lVertices.Text = "Vertices";
            // 
            // lPolygons
            // 
            this.lPolygons.AutoSize = true;
            this.lPolygons.Location = new System.Drawing.Point( 5, 82 );
            this.lPolygons.Name = "lPolygons";
            this.lPolygons.Size = new System.Drawing.Size( 50, 13 );
            this.lPolygons.TabIndex = 6;
            this.lPolygons.Text = "Polygons";
            // 
            // nPolygons
            // 
            this.nPolygons.Location = new System.Drawing.Point( 61, 80 );
            this.nPolygons.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.nPolygons.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.nPolygons.Name = "nPolygons";
            this.nPolygons.Size = new System.Drawing.Size( 43, 20 );
            this.nPolygons.TabIndex = 4;
            this.nPolygons.Value = new decimal( new int[] {
            50,
            0,
            0,
            0} );
            // 
            // nVertices
            // 
            this.nVertices.Location = new System.Drawing.Point( 61, 107 );
            this.nVertices.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.nVertices.Minimum = new decimal( new int[] {
            3,
            0,
            0,
            0} );
            this.nVertices.Name = "nVertices";
            this.nVertices.Size = new System.Drawing.Size( 43, 20 );
            this.nVertices.TabIndex = 5;
            this.nVertices.Value = new decimal( new int[] {
            6,
            0,
            0,
            0} );
            // 
            // bStartStop
            // 
            this.bStartStop.Location = new System.Drawing.Point( 110, 80 );
            this.bStartStop.Name = "bStartStop";
            this.bStartStop.Size = new System.Drawing.Size( 58, 47 );
            this.bStartStop.TabIndex = 3;
            this.bStartStop.Text = "Start";
            this.bStartStop.UseVisualStyleBackColor = true;
            this.bStartStop.Click += new System.EventHandler( this.button1_Click );
            // 
            // tMutationStats
            // 
            this.tMutationStats.Location = new System.Drawing.Point( 310, 3 );
            this.tMutationStats.Multiline = true;
            this.tMutationStats.Name = "tMutationStats";
            this.tMutationStats.ReadOnly = true;
            this.tMutationStats.Size = new System.Drawing.Size( 194, 215 );
            this.tMutationStats.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 595, 336 );
            this.Controls.Add( this.flowLayoutPanel1 );
            this.Name = "MainForm";
            this.Text = "Super Image Evolver";
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.flowLayoutPanel1.ResumeLayout( false );
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private Canvas picBestMatch;
        private System.Windows.Forms.TextBox tTaskStats;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown nPolygons;
        private System.Windows.Forms.NumericUpDown nVertices;
        private System.Windows.Forms.Button bStartStop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lVertices;
        private System.Windows.Forms.Label lPolygons;
        private System.Windows.Forms.Label lMutator;
        private System.Windows.Forms.Label lInitializer;
        private System.Windows.Forms.ComboBox cMutator;
        private System.Windows.Forms.ComboBox cInitializer;
        private System.Windows.Forms.Label lEvaluator;
        private System.Windows.Forms.ComboBox cEvaluator;
        private System.Windows.Forms.TextBox tMutationStats;
        private DiffCanvas picDiff;
    }
}

