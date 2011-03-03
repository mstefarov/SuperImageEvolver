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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator separator1;
            System.Windows.Forms.ToolStripSeparator separator4;
            System.Windows.Forms.ToolStripSeparator separator2;
            System.Windows.Forms.ToolStripSeparator separator3;
            System.Windows.Forms.ToolStripSeparator separator5;
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.picOriginal = new System.Windows.Forms.PictureBox();
            this.cmBestMatch = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cmBestMatchZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchWireframe = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchShowLastChange = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiff = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cmDiffZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffShowColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffExaggerate = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bEditEvaluatorSettings = new System.Windows.Forms.Button();
            this.bEditMutatorSettings = new System.Windows.Forms.Button();
            this.bEditInitializerSetting = new System.Windows.Forms.Button();
            this.lEvaluator = new System.Windows.Forms.Label();
            this.cEvaluator = new System.Windows.Forms.ComboBox();
            this.lMutator = new System.Windows.Forms.Label();
            this.lInitializer = new System.Windows.Forms.Label();
            this.cMutator = new System.Windows.Forms.ComboBox();
            this.cInitializer = new System.Windows.Forms.ComboBox();
            this.lPoints = new System.Windows.Forms.Label();
            this.lShapes = new System.Windows.Forms.Label();
            this.nPolygons = new System.Windows.Forms.NumericUpDown();
            this.nVertices = new System.Windows.Forms.NumericUpDown();
            this.pStatistics = new System.Windows.Forms.Panel();
            this.bCopyStats = new System.Windows.Forms.Button();
            this.tMutationStats = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.bNewProject = new System.Windows.Forms.ToolStripButton();
            this.bOpenProject = new System.Windows.Forms.ToolStripButton();
            this.bSaveProject = new System.Windows.Forms.ToolStripButton();
            this.bSaveProjectAs = new System.Windows.Forms.ToolStripButton();
            this.bRestart = new System.Windows.Forms.ToolStripButton();
            this.bStart = new System.Windows.Forms.ToolStripButton();
            this.bStop = new System.Windows.Forms.ToolStripButton();
            this.menuView = new System.Windows.Forms.ToolStripDropDownButton();
            this.bViewOriginalImage = new System.Windows.Forms.ToolStripMenuItem();
            this.bViewBestMatchImage = new System.Windows.Forms.ToolStripMenuItem();
            this.bViewDifferenceImage = new System.Windows.Forms.ToolStripMenuItem();
            this.bViewStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImport = new System.Windows.Forms.ToolStripDropDownButton();
            this.bImportDNA = new System.Windows.Forms.ToolStripMenuItem();
            this.bImportSVG = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.bExportDNA = new System.Windows.Forms.ToolStripMenuItem();
            this.bExportImage = new System.Windows.Forms.ToolStripMenuItem();
            this.bExportVectors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.bHelpListModules = new System.Windows.Forms.ToolStripMenuItem();
            this.picBestMatch = new SuperImageEvolver.Canvas();
            this.picDiff = new SuperImageEvolver.DiffCanvas();
            this.graphWindow1 = new SuperImageEvolver.GraphWindow();
            separator1 = new System.Windows.Forms.ToolStripSeparator();
            separator4 = new System.Windows.Forms.ToolStripSeparator();
            separator2 = new System.Windows.Forms.ToolStripSeparator();
            separator3 = new System.Windows.Forms.ToolStripSeparator();
            separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.cmBestMatch.SuspendLayout();
            this.cmDiff.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).BeginInit();
            this.pStatistics.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // separator1
            // 
            separator1.Margin = new System.Windows.Forms.Padding( 3, 0, 3, 0 );
            separator1.Name = "separator1";
            separator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // separator4
            // 
            separator4.Name = "separator4";
            separator4.Size = new System.Drawing.Size( 6, 25 );
            // 
            // separator2
            // 
            separator2.Margin = new System.Windows.Forms.Padding( 3, 0, 3, 0 );
            separator2.Name = "separator2";
            separator2.Size = new System.Drawing.Size( 6, 25 );
            // 
            // separator3
            // 
            separator3.Name = "separator3";
            separator3.Size = new System.Drawing.Size( 6, 25 );
            // 
            // separator5
            // 
            separator5.Name = "separator5";
            separator5.Size = new System.Drawing.Size( 6, 25 );
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add( this.picOriginal );
            this.flowLayoutPanel1.Controls.Add( this.picBestMatch );
            this.flowLayoutPanel1.Controls.Add( this.picDiff );
            this.flowLayoutPanel1.Controls.Add( this.panel1 );
            this.flowLayoutPanel1.Controls.Add( this.pStatistics );
            this.flowLayoutPanel1.Location = new System.Drawing.Point( 0, 24 );
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding( 0 );
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding( 0, 2, 0, 0 );
            this.flowLayoutPanel1.Size = new System.Drawing.Size( 882, 461 );
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // picOriginal
            // 
            this.picOriginal.Location = new System.Drawing.Point( 3, 5 );
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size( 35, 38 );
            this.picOriginal.TabIndex = 2;
            this.picOriginal.TabStop = false;
            // 
            // cmBestMatch
            // 
            this.cmBestMatch.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmBestMatchZoom,
            this.cmBestMatchWireframe,
            this.cmBestMatchShowLastChange} );
            this.cmBestMatch.Name = "cmBestMatch";
            this.cmBestMatch.Size = new System.Drawing.Size( 172, 70 );
            // 
            // cmBestMatchZoom
            // 
            this.cmBestMatchZoom.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmBestMatchZoom50,
            this.cmBestMatchZoom75,
            this.cmBestMatchZoom100,
            this.cmBestMatchZoom125,
            this.cmBestMatchZoom150,
            this.cmBestMatchZoom200} );
            this.cmBestMatchZoom.Name = "cmBestMatchZoom";
            this.cmBestMatchZoom.Size = new System.Drawing.Size( 171, 22 );
            this.cmBestMatchZoom.Text = "Zoom";
            this.cmBestMatchZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler( this.zoomToolStripMenuItem_DropDownItemClicked );
            // 
            // cmBestMatchZoom50
            // 
            this.cmBestMatchZoom50.CheckOnClick = true;
            this.cmBestMatchZoom50.Name = "cmBestMatchZoom50";
            this.cmBestMatchZoom50.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom50.Tag = ".5";
            this.cmBestMatchZoom50.Text = "50%";
            // 
            // cmBestMatchZoom75
            // 
            this.cmBestMatchZoom75.CheckOnClick = true;
            this.cmBestMatchZoom75.Name = "cmBestMatchZoom75";
            this.cmBestMatchZoom75.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom75.Tag = ".75";
            this.cmBestMatchZoom75.Text = "75%";
            // 
            // cmBestMatchZoom100
            // 
            this.cmBestMatchZoom100.Checked = true;
            this.cmBestMatchZoom100.CheckOnClick = true;
            this.cmBestMatchZoom100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmBestMatchZoom100.Name = "cmBestMatchZoom100";
            this.cmBestMatchZoom100.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom100.Tag = "1";
            this.cmBestMatchZoom100.Text = "100%";
            // 
            // cmBestMatchZoom125
            // 
            this.cmBestMatchZoom125.CheckOnClick = true;
            this.cmBestMatchZoom125.Name = "cmBestMatchZoom125";
            this.cmBestMatchZoom125.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom125.Tag = "1.25";
            this.cmBestMatchZoom125.Text = "125%";
            // 
            // cmBestMatchZoom150
            // 
            this.cmBestMatchZoom150.CheckOnClick = true;
            this.cmBestMatchZoom150.Name = "cmBestMatchZoom150";
            this.cmBestMatchZoom150.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom150.Tag = "1.5";
            this.cmBestMatchZoom150.Text = "150%";
            // 
            // cmBestMatchZoom200
            // 
            this.cmBestMatchZoom200.CheckOnClick = true;
            this.cmBestMatchZoom200.Name = "cmBestMatchZoom200";
            this.cmBestMatchZoom200.Size = new System.Drawing.Size( 102, 22 );
            this.cmBestMatchZoom200.Tag = "2";
            this.cmBestMatchZoom200.Text = "200%";
            // 
            // cmBestMatchWireframe
            // 
            this.cmBestMatchWireframe.CheckOnClick = true;
            this.cmBestMatchWireframe.Name = "cmBestMatchWireframe";
            this.cmBestMatchWireframe.Size = new System.Drawing.Size( 171, 22 );
            this.cmBestMatchWireframe.Text = "Show Wireframe";
            this.cmBestMatchWireframe.CheckedChanged += new System.EventHandler( this.showWireframeToolStripMenuItem_CheckedChanged );
            // 
            // cmBestMatchShowLastChange
            // 
            this.cmBestMatchShowLastChange.CheckOnClick = true;
            this.cmBestMatchShowLastChange.Name = "cmBestMatchShowLastChange";
            this.cmBestMatchShowLastChange.Size = new System.Drawing.Size( 171, 22 );
            this.cmBestMatchShowLastChange.Text = "Show Last Change";
            this.cmBestMatchShowLastChange.CheckedChanged += new System.EventHandler( this.showLastChangeToolStripMenuItem_CheckedChanged );
            // 
            // cmDiff
            // 
            this.cmDiff.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmDiffZoom,
            this.cmDiffInvert,
            this.cmDiffShowColor,
            this.cmDiffExaggerate} );
            this.cmDiff.Name = "cmBestMatch";
            this.cmDiff.Size = new System.Drawing.Size( 136, 92 );
            // 
            // cmDiffZoom
            // 
            this.cmDiffZoom.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmDiffZoom50,
            this.cmDiffZoom75,
            this.cmDiffZoom100,
            this.cmDiffZoom125,
            this.cmDiffZoom150,
            this.cmDiffZoom200} );
            this.cmDiffZoom.Name = "cmDiffZoom";
            this.cmDiffZoom.Size = new System.Drawing.Size( 135, 22 );
            this.cmDiffZoom.Text = "Zoom";
            this.cmDiffZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler( this.cmDiffZoom_DropDownItemClicked );
            // 
            // cmDiffZoom50
            // 
            this.cmDiffZoom50.CheckOnClick = true;
            this.cmDiffZoom50.Name = "cmDiffZoom50";
            this.cmDiffZoom50.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom50.Tag = ".5";
            this.cmDiffZoom50.Text = "50%";
            // 
            // cmDiffZoom75
            // 
            this.cmDiffZoom75.CheckOnClick = true;
            this.cmDiffZoom75.Name = "cmDiffZoom75";
            this.cmDiffZoom75.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom75.Tag = ".75";
            this.cmDiffZoom75.Text = "75%";
            // 
            // cmDiffZoom100
            // 
            this.cmDiffZoom100.Checked = true;
            this.cmDiffZoom100.CheckOnClick = true;
            this.cmDiffZoom100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffZoom100.Name = "cmDiffZoom100";
            this.cmDiffZoom100.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom100.Tag = "1";
            this.cmDiffZoom100.Text = "100%";
            // 
            // cmDiffZoom125
            // 
            this.cmDiffZoom125.CheckOnClick = true;
            this.cmDiffZoom125.Name = "cmDiffZoom125";
            this.cmDiffZoom125.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom125.Tag = "1.25";
            this.cmDiffZoom125.Text = "125%";
            // 
            // cmDiffZoom150
            // 
            this.cmDiffZoom150.CheckOnClick = true;
            this.cmDiffZoom150.Name = "cmDiffZoom150";
            this.cmDiffZoom150.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom150.Tag = "1.5";
            this.cmDiffZoom150.Text = "150%";
            // 
            // cmDiffZoom200
            // 
            this.cmDiffZoom200.CheckOnClick = true;
            this.cmDiffZoom200.Name = "cmDiffZoom200";
            this.cmDiffZoom200.Size = new System.Drawing.Size( 102, 22 );
            this.cmDiffZoom200.Tag = "2";
            this.cmDiffZoom200.Text = "200%";
            // 
            // cmDiffInvert
            // 
            this.cmDiffInvert.Checked = true;
            this.cmDiffInvert.CheckOnClick = true;
            this.cmDiffInvert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffInvert.Name = "cmDiffInvert";
            this.cmDiffInvert.Size = new System.Drawing.Size( 135, 22 );
            this.cmDiffInvert.Text = "Invert";
            this.cmDiffInvert.CheckedChanged += new System.EventHandler( this.cmDiffInvert_CheckedChanged );
            // 
            // cmDiffShowColor
            // 
            this.cmDiffShowColor.Checked = true;
            this.cmDiffShowColor.CheckOnClick = true;
            this.cmDiffShowColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffShowColor.Name = "cmDiffShowColor";
            this.cmDiffShowColor.Size = new System.Drawing.Size( 135, 22 );
            this.cmDiffShowColor.Text = "Show Color";
            this.cmDiffShowColor.CheckedChanged += new System.EventHandler( this.cmDiffShowColor_CheckedChanged );
            // 
            // cmDiffExaggerate
            // 
            this.cmDiffExaggerate.Checked = true;
            this.cmDiffExaggerate.CheckOnClick = true;
            this.cmDiffExaggerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffExaggerate.Name = "cmDiffExaggerate";
            this.cmDiffExaggerate.Size = new System.Drawing.Size( 135, 22 );
            this.cmDiffExaggerate.Text = "Exaggerate";
            this.cmDiffExaggerate.CheckedChanged += new System.EventHandler( this.cmDiffExaggerate_CheckedChanged );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.bEditEvaluatorSettings );
            this.panel1.Controls.Add( this.bEditMutatorSettings );
            this.panel1.Controls.Add( this.bEditInitializerSetting );
            this.panel1.Controls.Add( this.lEvaluator );
            this.panel1.Controls.Add( this.cEvaluator );
            this.panel1.Controls.Add( this.lMutator );
            this.panel1.Controls.Add( this.lInitializer );
            this.panel1.Controls.Add( this.cMutator );
            this.panel1.Controls.Add( this.cInitializer );
            this.panel1.Controls.Add( this.graphWindow1 );
            this.panel1.Controls.Add( this.lPoints );
            this.panel1.Controls.Add( this.lShapes );
            this.panel1.Controls.Add( this.nPolygons );
            this.panel1.Controls.Add( this.nVertices );
            this.panel1.Location = new System.Drawing.Point( 133, 5 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 203, 215 );
            this.panel1.TabIndex = 6;
            // 
            // bEditEvaluatorSettings
            // 
            this.bEditEvaluatorSettings.Location = new System.Drawing.Point( 181, 187 );
            this.bEditEvaluatorSettings.Name = "bEditEvaluatorSettings";
            this.bEditEvaluatorSettings.Size = new System.Drawing.Size( 19, 21 );
            this.bEditEvaluatorSettings.TabIndex = 16;
            this.bEditEvaluatorSettings.UseVisualStyleBackColor = true;
            this.bEditEvaluatorSettings.Click += new System.EventHandler( this.bEditEvaluatorSettings_Click );
            // 
            // bEditMutatorSettings
            // 
            this.bEditMutatorSettings.Location = new System.Drawing.Point( 181, 160 );
            this.bEditMutatorSettings.Name = "bEditMutatorSettings";
            this.bEditMutatorSettings.Size = new System.Drawing.Size( 19, 21 );
            this.bEditMutatorSettings.TabIndex = 15;
            this.bEditMutatorSettings.UseVisualStyleBackColor = true;
            this.bEditMutatorSettings.Click += new System.EventHandler( this.bEditMutatorSettings_Click );
            // 
            // bEditInitializerSetting
            // 
            this.bEditInitializerSetting.Location = new System.Drawing.Point( 181, 133 );
            this.bEditInitializerSetting.Name = "bEditInitializerSetting";
            this.bEditInitializerSetting.Size = new System.Drawing.Size( 19, 21 );
            this.bEditInitializerSetting.TabIndex = 14;
            this.bEditInitializerSetting.UseVisualStyleBackColor = true;
            this.bEditInitializerSetting.Click += new System.EventHandler( this.bEditInitializerSetting_Click );
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
            "RGB (Sloppy)",
            "RGB (Fast)",
            "RGB (Smooth)",
            "RGB+Luma (Fast)",
            "RGB+Luma (Smooth)"} );
            this.cEvaluator.Location = new System.Drawing.Point( 61, 187 );
            this.cEvaluator.Name = "cEvaluator";
            this.cEvaluator.Size = new System.Drawing.Size( 114, 21 );
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
            "Translate",
            "Translate/Stretch",
            "Translate/Rotate",
            "Transform",
            "Soft Translate",
            "Soft Translate/Stretch",
            "Soft Translate/Rotate",
            "Soft Transform"} );
            this.cMutator.Location = new System.Drawing.Point( 61, 160 );
            this.cMutator.Name = "cMutator";
            this.cMutator.Size = new System.Drawing.Size( 114, 21 );
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
            this.cInitializer.Size = new System.Drawing.Size( 114, 21 );
            this.cInitializer.TabIndex = 8;
            this.cInitializer.SelectedIndexChanged += new System.EventHandler( this.cInitializer_SelectedIndexChanged );
            // 
            // lPoints
            // 
            this.lPoints.AutoSize = true;
            this.lPoints.Location = new System.Drawing.Point( 19, 109 );
            this.lPoints.Name = "lPoints";
            this.lPoints.Size = new System.Drawing.Size( 36, 13 );
            this.lPoints.TabIndex = 7;
            this.lPoints.Text = "Points";
            // 
            // lShapes
            // 
            this.lShapes.AutoSize = true;
            this.lShapes.Location = new System.Drawing.Point( 107, 109 );
            this.lShapes.Name = "lShapes";
            this.lShapes.Size = new System.Drawing.Size( 43, 13 );
            this.lShapes.TabIndex = 6;
            this.lShapes.Text = "Shapes";
            // 
            // nPolygons
            // 
            this.nPolygons.Location = new System.Drawing.Point( 156, 107 );
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
            this.nPolygons.Size = new System.Drawing.Size( 44, 20 );
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
            this.nVertices.Size = new System.Drawing.Size( 40, 20 );
            this.nVertices.TabIndex = 5;
            this.nVertices.Value = new decimal( new int[] {
            6,
            0,
            0,
            0} );
            // 
            // pStatistics
            // 
            this.pStatistics.Controls.Add( this.bCopyStats );
            this.pStatistics.Controls.Add( this.tMutationStats );
            this.pStatistics.Location = new System.Drawing.Point( 342, 5 );
            this.pStatistics.Name = "pStatistics";
            this.pStatistics.Size = new System.Drawing.Size( 200, 215 );
            this.pStatistics.TabIndex = 9;
            // 
            // bCopyStats
            // 
            this.bCopyStats.Location = new System.Drawing.Point( 122, 187 );
            this.bCopyStats.Name = "bCopyStats";
            this.bCopyStats.Size = new System.Drawing.Size( 75, 23 );
            this.bCopyStats.TabIndex = 8;
            this.bCopyStats.Text = "Copy Stats";
            this.bCopyStats.UseVisualStyleBackColor = true;
            this.bCopyStats.Click += new System.EventHandler( this.bCopyStats_Click );
            // 
            // tMutationStats
            // 
            this.tMutationStats.Font = new System.Drawing.Font( "Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.tMutationStats.Location = new System.Drawing.Point( 3, 3 );
            this.tMutationStats.Multiline = true;
            this.tMutationStats.Name = "tMutationStats";
            this.tMutationStats.ReadOnly = true;
            this.tMutationStats.Size = new System.Drawing.Size( 194, 178 );
            this.tMutationStats.TabIndex = 7;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bNewProject,
            this.bOpenProject,
            this.bSaveProject,
            this.bSaveProjectAs,
            separator1,
            this.bRestart,
            this.bStart,
            this.bStop,
            separator2,
            this.menuView,
            separator3,
            this.menuImport,
            separator4,
            this.menuExport,
            separator5,
            this.menuHelp} );
            this.toolStrip.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size( 882, 25 );
            this.toolStrip.TabIndex = 5;
            // 
            // bNewProject
            // 
            this.bNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bNewProject.Image = global::SuperImageEvolver.Properties.Resources.document;
            this.bNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNewProject.Name = "bNewProject";
            this.bNewProject.Size = new System.Drawing.Size( 23, 22 );
            this.bNewProject.Text = "New Project...";
            this.bNewProject.Click += new System.EventHandler( this.bNewProject_Click );
            // 
            // bOpenProject
            // 
            this.bOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bOpenProject.Image = global::SuperImageEvolver.Properties.Resources.folder_horizontal_open;
            this.bOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenProject.Name = "bOpenProject";
            this.bOpenProject.Size = new System.Drawing.Size( 23, 22 );
            this.bOpenProject.Text = "Open Project...";
            this.bOpenProject.Click += new System.EventHandler( this.bOpenProject_Click );
            // 
            // bSaveProject
            // 
            this.bSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveProject.Image = global::SuperImageEvolver.Properties.Resources.disk_black;
            this.bSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveProject.Name = "bSaveProject";
            this.bSaveProject.Size = new System.Drawing.Size( 23, 22 );
            this.bSaveProject.Text = "Save Project";
            this.bSaveProject.Click += new System.EventHandler( this.bSaveProject_Click );
            // 
            // bSaveProjectAs
            // 
            this.bSaveProjectAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveProjectAs.Image = global::SuperImageEvolver.Properties.Resources.disk__pencil;
            this.bSaveProjectAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveProjectAs.Name = "bSaveProjectAs";
            this.bSaveProjectAs.Size = new System.Drawing.Size( 23, 22 );
            this.bSaveProjectAs.Text = "Save Project As...";
            this.bSaveProjectAs.Click += new System.EventHandler( this.bSaveProjectAs_Click );
            // 
            // bRestart
            // 
            this.bRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRestart.Image = global::SuperImageEvolver.Properties.Resources.arrow_stop_180;
            this.bRestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRestart.Name = "bRestart";
            this.bRestart.Size = new System.Drawing.Size( 23, 22 );
            this.bRestart.Text = "Restart";
            this.bRestart.Click += new System.EventHandler( this.bRestart_Click );
            // 
            // bStart
            // 
            this.bStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bStart.Image = global::SuperImageEvolver.Properties.Resources.control;
            this.bStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size( 23, 22 );
            this.bStart.Text = "Start / Resume";
            this.bStart.Click += new System.EventHandler( this.bStart_Click );
            // 
            // bStop
            // 
            this.bStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bStop.Enabled = false;
            this.bStop.Image = global::SuperImageEvolver.Properties.Resources.control_stop_square;
            this.bStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size( 23, 22 );
            this.bStop.Text = "Pause / Stop";
            this.bStop.Click += new System.EventHandler( this.bStop_Click );
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bViewOriginalImage,
            this.bViewBestMatchImage,
            this.bViewDifferenceImage,
            this.bViewStatistics} );
            this.menuView.Image = global::SuperImageEvolver.Properties.Resources.images;
            this.menuView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size( 61, 22 );
            this.menuView.Text = "View";
            // 
            // bViewOriginalImage
            // 
            this.bViewOriginalImage.Checked = true;
            this.bViewOriginalImage.CheckOnClick = true;
            this.bViewOriginalImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewOriginalImage.Name = "bViewOriginalImage";
            this.bViewOriginalImage.Size = new System.Drawing.Size( 169, 22 );
            this.bViewOriginalImage.Text = "Original Image";
            this.bViewOriginalImage.Click += new System.EventHandler( this.bViewOriginalImage_Click );
            // 
            // bViewBestMatchImage
            // 
            this.bViewBestMatchImage.Checked = true;
            this.bViewBestMatchImage.CheckOnClick = true;
            this.bViewBestMatchImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewBestMatchImage.Name = "bViewBestMatchImage";
            this.bViewBestMatchImage.Size = new System.Drawing.Size( 169, 22 );
            this.bViewBestMatchImage.Text = "Best Match Image";
            this.bViewBestMatchImage.Click += new System.EventHandler( this.bViewBestMatchImage_Click );
            // 
            // bViewDifferenceImage
            // 
            this.bViewDifferenceImage.Checked = true;
            this.bViewDifferenceImage.CheckOnClick = true;
            this.bViewDifferenceImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewDifferenceImage.Name = "bViewDifferenceImage";
            this.bViewDifferenceImage.Size = new System.Drawing.Size( 169, 22 );
            this.bViewDifferenceImage.Text = "Difference Image";
            this.bViewDifferenceImage.Click += new System.EventHandler( this.bViewDifferenceImage_Click );
            // 
            // bViewStatistics
            // 
            this.bViewStatistics.Checked = true;
            this.bViewStatistics.CheckOnClick = true;
            this.bViewStatistics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewStatistics.Name = "bViewStatistics";
            this.bViewStatistics.Size = new System.Drawing.Size( 169, 22 );
            this.bViewStatistics.Text = "Statistics";
            this.bViewStatistics.Click += new System.EventHandler( this.bViewStatistics_Click );
            // 
            // menuImport
            // 
            this.menuImport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bImportDNA,
            this.bImportSVG} );
            this.menuImport.Image = global::SuperImageEvolver.Properties.Resources.document_import;
            this.menuImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuImport.Name = "menuImport";
            this.menuImport.Size = new System.Drawing.Size( 72, 22 );
            this.menuImport.Text = "Import";
            // 
            // bImportDNA
            // 
            this.bImportDNA.Enabled = false;
            this.bImportDNA.Name = "bImportDNA";
            this.bImportDNA.Size = new System.Drawing.Size( 214, 22 );
            this.bImportDNA.Text = "DNA from ImageEvolution";
            // 
            // bImportSVG
            // 
            this.bImportSVG.Enabled = false;
            this.bImportSVG.Image = global::SuperImageEvolver.Properties.Resources.layer_shape_polygon;
            this.bImportSVG.Name = "bImportSVG";
            this.bImportSVG.Size = new System.Drawing.Size( 214, 22 );
            this.bImportSVG.Text = "Best Match from SVG";
            // 
            // menuExport
            // 
            this.menuExport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bExportDNA,
            this.bExportImage,
            this.bExportVectors} );
            this.menuExport.Image = global::SuperImageEvolver.Properties.Resources.document_export;
            this.menuExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuExport.Name = "menuExport";
            this.menuExport.Size = new System.Drawing.Size( 69, 22 );
            this.menuExport.Text = "Export";
            // 
            // bExportDNA
            // 
            this.bExportDNA.Enabled = false;
            this.bExportDNA.Name = "bExportDNA";
            this.bExportDNA.Size = new System.Drawing.Size( 175, 22 );
            this.bExportDNA.Text = "DNA";
            // 
            // bExportImage
            // 
            this.bExportImage.Image = global::SuperImageEvolver.Properties.Resources.image_export;
            this.bExportImage.Name = "bExportImage";
            this.bExportImage.Size = new System.Drawing.Size( 175, 22 );
            this.bExportImage.Text = "Best Match Image";
            this.bExportImage.Click += new System.EventHandler( this.bExportImage_Click );
            // 
            // bExportVectors
            // 
            this.bExportVectors.Image = global::SuperImageEvolver.Properties.Resources.layer_shape_polygon;
            this.bExportVectors.Name = "bExportVectors";
            this.bExportVectors.Size = new System.Drawing.Size( 175, 22 );
            this.bExportVectors.Text = "Best Match Vectors";
            this.bExportVectors.Click += new System.EventHandler( this.bExportVectors_Click );
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bHelpListModules} );
            this.menuHelp.Image = global::SuperImageEvolver.Properties.Resources.information;
            this.menuHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size( 61, 22 );
            this.menuHelp.Text = "Help";
            // 
            // bHelpListModules
            // 
            this.bHelpListModules.Name = "bHelpListModules";
            this.bHelpListModules.Size = new System.Drawing.Size( 141, 22 );
            this.bHelpListModules.Text = "List Modules";
            this.bHelpListModules.Click += new System.EventHandler( this.bHelpListModules_Click );
            // 
            // picBestMatch
            // 
            this.picBestMatch.ContextMenuStrip = this.cmBestMatch;
            this.picBestMatch.Location = new System.Drawing.Point( 44, 5 );
            this.picBestMatch.Name = "picBestMatch";
            this.picBestMatch.Size = new System.Drawing.Size( 39, 38 );
            this.picBestMatch.State = null;
            this.picBestMatch.TabIndex = 0;
            this.picBestMatch.Zoom = 1F;
            // 
            // picDiff
            // 
            this.picDiff.ContextMenuStrip = this.cmDiff;
            this.picDiff.Location = new System.Drawing.Point( 89, 5 );
            this.picDiff.Name = "picDiff";
            this.picDiff.Size = new System.Drawing.Size( 38, 38 );
            this.picDiff.State = null;
            this.picDiff.TabIndex = 8;
            this.picDiff.Zoom = 1F;
            // 
            // graphWindow1
            // 
            this.graphWindow1.BackColor = System.Drawing.Color.White;
            this.graphWindow1.Location = new System.Drawing.Point( 3, 3 );
            this.graphWindow1.Name = "graphWindow1";
            this.graphWindow1.Size = new System.Drawing.Size( 197, 98 );
            this.graphWindow1.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size( 882, 485 );
            this.Controls.Add( this.toolStrip );
            this.Controls.Add( this.flowLayoutPanel1 );
            this.ForeColor = System.Drawing.Color.Gray;
            this.Name = "MainForm";
            this.Text = "Super Image Evolver";
            this.flowLayoutPanel1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.cmBestMatch.ResumeLayout( false );
            this.cmDiff.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).EndInit();
            this.pStatistics.ResumeLayout( false );
            this.pStatistics.PerformLayout();
            this.toolStrip.ResumeLayout( false );
            this.toolStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private Canvas picBestMatch;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown nPolygons;
        private System.Windows.Forms.NumericUpDown nVertices;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lPoints;
        private System.Windows.Forms.Label lShapes;
        private System.Windows.Forms.Label lMutator;
        private System.Windows.Forms.Label lInitializer;
        private System.Windows.Forms.ComboBox cMutator;
        private System.Windows.Forms.ComboBox cInitializer;
        private System.Windows.Forms.Label lEvaluator;
        private System.Windows.Forms.ComboBox cEvaluator;
        private System.Windows.Forms.TextBox tMutationStats;
        private DiffCanvas picDiff;
        private GraphWindow graphWindow1;
        private System.Windows.Forms.Panel pStatistics;
        private System.Windows.Forms.Button bCopyStats;
        private System.Windows.Forms.Button bEditMutatorSettings;
        private System.Windows.Forms.Button bEditInitializerSetting;
        private System.Windows.Forms.Button bEditEvaluatorSettings;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton bRestart;
        private System.Windows.Forms.ToolStripButton bStart;
        private System.Windows.Forms.ToolStripButton bStop;
        private System.Windows.Forms.ToolStripDropDownButton menuExport;
        private System.Windows.Forms.ToolStripMenuItem bExportImage;
        private System.Windows.Forms.ToolStripMenuItem bExportVectors;
        private System.Windows.Forms.ToolStripButton bOpenProject;
        private System.Windows.Forms.ToolStripButton bSaveProjectAs;
        private System.Windows.Forms.ToolStripMenuItem bExportDNA;
        private System.Windows.Forms.ToolStripButton bNewProject;
        private System.Windows.Forms.ToolStripButton bSaveProject;
        private System.Windows.Forms.ToolStripDropDownButton menuImport;
        private System.Windows.Forms.ToolStripMenuItem bImportSVG;
        private System.Windows.Forms.ToolStripMenuItem bImportDNA;
        private System.Windows.Forms.ToolStripDropDownButton menuView;
        private System.Windows.Forms.ToolStripMenuItem bViewOriginalImage;
        private System.Windows.Forms.ToolStripMenuItem bViewBestMatchImage;
        private System.Windows.Forms.ToolStripMenuItem bViewDifferenceImage;
        private System.Windows.Forms.ToolStripMenuItem bViewStatistics;
        private System.Windows.Forms.ToolStripDropDownButton menuHelp;
        private System.Windows.Forms.ToolStripMenuItem bHelpListModules;
        private System.Windows.Forms.ContextMenuStrip cmBestMatch;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom50;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom100;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom150;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom200;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom75;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoom125;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchWireframe;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchShowLastChange;
        private System.Windows.Forms.ContextMenuStrip cmDiff;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom50;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom75;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom100;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom125;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom150;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoom200;
        private System.Windows.Forms.ToolStripMenuItem cmDiffShowColor;
        private System.Windows.Forms.ToolStripMenuItem cmDiffInvert;
        private System.Windows.Forms.ToolStripMenuItem cmDiffExaggerate;
    }
}

