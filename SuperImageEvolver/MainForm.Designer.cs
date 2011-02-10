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
            this.graphWindow1 = new SuperImageEvolver.GraphWindow();
            this.lVertices = new System.Windows.Forms.Label();
            this.lPolygons = new System.Windows.Forms.Label();
            this.nPolygons = new System.Windows.Forms.NumericUpDown();
            this.nVertices = new System.Windows.Forms.NumericUpDown();
            this.bStartStop = new System.Windows.Forms.Button();
            this.tMutationStats = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuTask = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewTask = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenTask = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveTask = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExportImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportSVG = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOriginalImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBestMatchImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDifferenceImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listModulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add( this.picOriginal );
            this.flowLayoutPanel1.Controls.Add( this.picBestMatch );
            this.flowLayoutPanel1.Controls.Add( this.picDiff );
            this.flowLayoutPanel1.Controls.Add( this.panel1 );
            this.flowLayoutPanel1.Controls.Add( this.tMutationStats );
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point( 0, 24 );
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size( 882, 461 );
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // picBestMatch
            // 
            this.picBestMatch.Location = new System.Drawing.Point( 44, 3 );
            this.picBestMatch.Name = "picBestMatch";
            this.picBestMatch.ShowLastChange = false;
            this.picBestMatch.Size = new System.Drawing.Size( 39, 38 );
            this.picBestMatch.TabIndex = 0;
            this.picBestMatch.Wireframe = false;
            // 
            // picDiff
            // 
            this.picDiff.Inverse = true;
            this.picDiff.Location = new System.Drawing.Point( 89, 3 );
            this.picDiff.Monochrome = false;
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
            this.panel1.Controls.Add( this.graphWindow1 );
            this.panel1.Controls.Add( this.lVertices );
            this.panel1.Controls.Add( this.lPolygons );
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
            "Softer",
            "SubPixel",
            "Translate",
            "Translate/Stretch",
            "Translate/Rotate",
            "Transform"} );
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
            "Segmented",
            "Radial"} );
            this.cInitializer.Location = new System.Drawing.Point( 61, 133 );
            this.cInitializer.Name = "cInitializer";
            this.cInitializer.Size = new System.Drawing.Size( 107, 21 );
            this.cInitializer.TabIndex = 8;
            this.cInitializer.SelectedIndexChanged += new System.EventHandler( this.cInitializer_SelectedIndexChanged );
            // 
            // graphWindow1
            // 
            this.graphWindow1.Location = new System.Drawing.Point( 3, 3 );
            this.graphWindow1.Name = "graphWindow1";
            this.graphWindow1.Size = new System.Drawing.Size( 165, 71 );
            this.graphWindow1.TabIndex = 9;
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
            this.bStartStop.Click += new System.EventHandler( this.bStartStop_Click );
            // 
            // tMutationStats
            // 
            this.tMutationStats.Font = new System.Drawing.Font( "Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.tMutationStats.Location = new System.Drawing.Point( 310, 3 );
            this.tMutationStats.Multiline = true;
            this.tMutationStats.Name = "tMutationStats";
            this.tMutationStats.ReadOnly = true;
            this.tMutationStats.Size = new System.Drawing.Size( 199, 215 );
            this.tMutationStats.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuTask,
            this.menuView,
            this.helpToolStripMenuItem} );
            this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size( 882, 24 );
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuTask
            // 
            this.menuTask.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuNewTask,
            this.menuOpenTask,
            this.menuSaveTask,
            this.toolStripSeparator1,
            this.menuExportImage,
            this.menuExportSVG,
            this.toolStripSeparator2,
            this.menuExit} );
            this.menuTask.Name = "menuTask";
            this.menuTask.Size = new System.Drawing.Size( 43, 20 );
            this.menuTask.Text = "Task";
            // 
            // menuNewTask
            // 
            this.menuNewTask.Name = "menuNewTask";
            this.menuNewTask.Size = new System.Drawing.Size( 228, 22 );
            this.menuNewTask.Text = "New Task";
            this.menuNewTask.Click += new System.EventHandler( this.menuNewTask_Click );
            // 
            // menuOpenTask
            // 
            this.menuOpenTask.Name = "menuOpenTask";
            this.menuOpenTask.Size = new System.Drawing.Size( 228, 22 );
            this.menuOpenTask.Text = "Open Task...";
            this.menuOpenTask.Click += new System.EventHandler( this.menuOpenTask_Click );
            // 
            // menuSaveTask
            // 
            this.menuSaveTask.Name = "menuSaveTask";
            this.menuSaveTask.Size = new System.Drawing.Size( 228, 22 );
            this.menuSaveTask.Text = "Save Task...";
            this.menuSaveTask.Click += new System.EventHandler( this.menuSaveTask_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 225, 6 );
            // 
            // menuExportImage
            // 
            this.menuExportImage.Name = "menuExportImage";
            this.menuExportImage.Size = new System.Drawing.Size( 228, 22 );
            this.menuExportImage.Text = "Export Best Match as Image...";
            this.menuExportImage.Click += new System.EventHandler( this.menuExportImage_Click );
            // 
            // menuExportSVG
            // 
            this.menuExportSVG.Name = "menuExportSVG";
            this.menuExportSVG.Size = new System.Drawing.Size( 228, 22 );
            this.menuExportSVG.Text = "Export Best Match as SVG...";
            this.menuExportSVG.Click += new System.EventHandler( this.menuExportSVG_Click );
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 225, 6 );
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size( 228, 22 );
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler( this.menuExit_Click );
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuOriginalImage,
            this.menuBestMatchImage,
            this.menuDifferenceImage,
            this.menuStatistics} );
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size( 44, 20 );
            this.menuView.Text = "View";
            // 
            // menuOriginalImage
            // 
            this.menuOriginalImage.Checked = true;
            this.menuOriginalImage.CheckOnClick = true;
            this.menuOriginalImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuOriginalImage.Name = "menuOriginalImage";
            this.menuOriginalImage.Size = new System.Drawing.Size( 169, 22 );
            this.menuOriginalImage.Text = "Original Image";
            this.menuOriginalImage.Click += new System.EventHandler( this.menuOriginalImage_Click );
            // 
            // menuBestMatchImage
            // 
            this.menuBestMatchImage.Checked = true;
            this.menuBestMatchImage.CheckOnClick = true;
            this.menuBestMatchImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuBestMatchImage.Name = "menuBestMatchImage";
            this.menuBestMatchImage.Size = new System.Drawing.Size( 169, 22 );
            this.menuBestMatchImage.Text = "Best Match Image";
            this.menuBestMatchImage.Click += new System.EventHandler( this.menuBestMatchImage_Click );
            // 
            // menuDifferenceImage
            // 
            this.menuDifferenceImage.Checked = true;
            this.menuDifferenceImage.CheckOnClick = true;
            this.menuDifferenceImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuDifferenceImage.Name = "menuDifferenceImage";
            this.menuDifferenceImage.Size = new System.Drawing.Size( 169, 22 );
            this.menuDifferenceImage.Text = "Difference Image";
            this.menuDifferenceImage.Click += new System.EventHandler( this.menuDifferenceImage_Click );
            // 
            // menuStatistics
            // 
            this.menuStatistics.Checked = true;
            this.menuStatistics.CheckOnClick = true;
            this.menuStatistics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStatistics.Name = "menuStatistics";
            this.menuStatistics.Size = new System.Drawing.Size( 169, 22 );
            this.menuStatistics.Text = "Statistics";
            this.menuStatistics.Click += new System.EventHandler( this.menuStatistics_Click );
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.listModulesToolStripMenuItem} );
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // listModulesToolStripMenuItem
            // 
            this.listModulesToolStripMenuItem.Name = "listModulesToolStripMenuItem";
            this.listModulesToolStripMenuItem.Size = new System.Drawing.Size( 141, 22 );
            this.listModulesToolStripMenuItem.Text = "List Modules";
            this.listModulesToolStripMenuItem.Click += new System.EventHandler( this.menuListModules_Click );
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size( 882, 485 );
            this.Controls.Add( this.flowLayoutPanel1 );
            this.Controls.Add( this.menuStrip1 );
            this.ForeColor = System.Drawing.Color.Gray;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Super Image Evolver";
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.flowLayoutPanel1.ResumeLayout( false );
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).EndInit();
            this.menuStrip1.ResumeLayout( false );
            this.menuStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private Canvas picBestMatch;
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
        private GraphWindow graphWindow1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuTask;
        private System.Windows.Forms.ToolStripMenuItem menuNewTask;
        private System.Windows.Forms.ToolStripMenuItem menuOpenTask;
        private System.Windows.Forms.ToolStripMenuItem menuSaveTask;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuOriginalImage;
        private System.Windows.Forms.ToolStripMenuItem menuBestMatchImage;
        private System.Windows.Forms.ToolStripMenuItem menuDifferenceImage;
        private System.Windows.Forms.ToolStripMenuItem menuStatistics;
        private System.Windows.Forms.ToolStripMenuItem menuExportImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listModulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuExportSVG;
    }
}

