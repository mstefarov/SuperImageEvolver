namespace SuperImageEvolver {
    sealed partial class MainForm {
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
            this.picOriginal = new SuperImageEvolver.CustomInterpolationPictureBox();
            this.cmOriginal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmOriginalZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOriginalZoomSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmOriginalZoomSync = new System.Windows.Forms.ToolStripMenuItem();
            this.picBestMatch = new SuperImageEvolver.Canvas();
            this.cmBestMatch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmBestMatchZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchZoomSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmBestMatchZoomSync = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchWireframe = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBestMatchShowLastChange = new System.Windows.Forms.ToolStripMenuItem();
            this.picDiff = new SuperImageEvolver.DiffCanvas();
            this.cmDiff = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmDiffZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffZoomSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmDiffZoomSync = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffShowColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffExaggerate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDiffShowLastChange = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lScale = new System.Windows.Forms.Label();
            this.tbEvalScale = new System.Windows.Forms.TrackBar();
            this.bEditEvaluatorSettings = new System.Windows.Forms.Button();
            this.bEditMutatorSettings = new System.Windows.Forms.Button();
            this.bEditInitializerSetting = new System.Windows.Forms.Button();
            this.lEvaluator = new System.Windows.Forms.Label();
            this.cEvaluator = new System.Windows.Forms.ComboBox();
            this.lMutator = new System.Windows.Forms.Label();
            this.lInitializer = new System.Windows.Forms.Label();
            this.cMutator = new System.Windows.Forms.ComboBox();
            this.cInitializer = new System.Windows.Forms.ComboBox();
            this.graphWindow1 = new SuperImageEvolver.GraphWindow();
            this.lPoints = new System.Windows.Forms.Label();
            this.lShapes = new System.Windows.Forms.Label();
            this.nPolygons = new System.Windows.Forms.NumericUpDown();
            this.nVertices = new System.Windows.Forms.NumericUpDown();
            this.pStatistics = new System.Windows.Forms.Panel();
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
            this.menuOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.bProjectOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.bMatteToAverageColor = new System.Windows.Forms.ToolStripMenuItem();
            this.polygonValueEvaluationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redistributeLeastValuableShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearOutlinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.bHelpListModules = new System.Windows.Forms.ToolStripMenuItem();
            this.ttEvalScale = new System.Windows.Forms.ToolTip(this.components);
            separator1 = new System.Windows.Forms.ToolStripSeparator();
            separator4 = new System.Windows.Forms.ToolStripSeparator();
            separator2 = new System.Windows.Forms.ToolStripSeparator();
            separator3 = new System.Windows.Forms.ToolStripSeparator();
            separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.cmOriginal.SuspendLayout();
            this.cmBestMatch.SuspendLayout();
            this.cmDiff.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbEvalScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).BeginInit();
            this.pStatistics.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // separator1
            // 
            separator1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            separator1.Name = "separator1";
            separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // separator4
            // 
            separator4.Name = "separator4";
            separator4.Size = new System.Drawing.Size(6, 25);
            // 
            // separator2
            // 
            separator2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            separator2.Name = "separator2";
            separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // separator3
            // 
            separator3.Name = "separator3";
            separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // separator5
            // 
            separator5.Name = "separator5";
            separator5.Size = new System.Drawing.Size(6, 25);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.picOriginal);
            this.flowLayoutPanel1.Controls.Add(this.picBestMatch);
            this.flowLayoutPanel1.Controls.Add(this.picDiff);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.pStatistics);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1029, 532);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // picOriginal
            // 
            this.picOriginal.ContextMenuStrip = this.cmOriginal;
            this.picOriginal.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.picOriginal.Location = new System.Drawing.Point(4, 5);
            this.picOriginal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size(93, 35);
            this.picOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOriginal.TabIndex = 10;
            this.picOriginal.TabStop = false;
            // 
            // cmOriginal
            // 
            this.cmOriginal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmOriginalZoom});
            this.cmOriginal.Name = "cmOriginal";
            this.cmOriginal.Size = new System.Drawing.Size(107, 26);
            // 
            // cmOriginalZoom
            // 
            this.cmOriginalZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmOriginalZoom50,
            this.cmOriginalZoom75,
            this.cmOriginalZoom100,
            this.cmOriginalZoom125,
            this.cmOriginalZoom150,
            this.cmOriginalZoom200,
            this.cmOriginalZoomSeparator,
            this.cmOriginalZoomSync});
            this.cmOriginalZoom.Name = "cmOriginalZoom";
            this.cmOriginalZoom.Size = new System.Drawing.Size(106, 22);
            this.cmOriginalZoom.Text = "Zoom";
            this.cmOriginalZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmOriginalZoom_DropDownItemClicked);
            // 
            // cmOriginalZoom50
            // 
            this.cmOriginalZoom50.Name = "cmOriginalZoom50";
            this.cmOriginalZoom50.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom50.Tag = ".5";
            this.cmOriginalZoom50.Text = "50%";
            // 
            // cmOriginalZoom75
            // 
            this.cmOriginalZoom75.Name = "cmOriginalZoom75";
            this.cmOriginalZoom75.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom75.Tag = ".75";
            this.cmOriginalZoom75.Text = "75%";
            // 
            // cmOriginalZoom100
            // 
            this.cmOriginalZoom100.Checked = true;
            this.cmOriginalZoom100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmOriginalZoom100.Name = "cmOriginalZoom100";
            this.cmOriginalZoom100.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom100.Tag = "1";
            this.cmOriginalZoom100.Text = "100%";
            // 
            // cmOriginalZoom125
            // 
            this.cmOriginalZoom125.Name = "cmOriginalZoom125";
            this.cmOriginalZoom125.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom125.Tag = "1.25";
            this.cmOriginalZoom125.Text = "125%";
            // 
            // cmOriginalZoom150
            // 
            this.cmOriginalZoom150.Name = "cmOriginalZoom150";
            this.cmOriginalZoom150.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom150.Tag = "1.5";
            this.cmOriginalZoom150.Text = "150%";
            // 
            // cmOriginalZoom200
            // 
            this.cmOriginalZoom200.Name = "cmOriginalZoom200";
            this.cmOriginalZoom200.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoom200.Tag = "2";
            this.cmOriginalZoom200.Text = "200%";
            // 
            // cmOriginalZoomSeparator
            // 
            this.cmOriginalZoomSeparator.Name = "cmOriginalZoomSeparator";
            this.cmOriginalZoomSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // cmOriginalZoomSync
            // 
            this.cmOriginalZoomSync.Name = "cmOriginalZoomSync";
            this.cmOriginalZoomSync.Size = new System.Drawing.Size(146, 22);
            this.cmOriginalZoomSync.Text = "Sync all views";
            // 
            // picBestMatch
            // 
            this.picBestMatch.ContextMenuStrip = this.cmBestMatch;
            this.picBestMatch.Location = new System.Drawing.Point(105, 5);
            this.picBestMatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picBestMatch.Name = "picBestMatch";
            this.picBestMatch.ShowLastChange = false;
            this.picBestMatch.Size = new System.Drawing.Size(93, 35);
            this.picBestMatch.State = null;
            this.picBestMatch.TabIndex = 0;
            this.picBestMatch.Wireframe = false;
            this.picBestMatch.Zoom = 1F;
            // 
            // cmBestMatch
            // 
            this.cmBestMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmBestMatchZoom,
            this.cmBestMatchWireframe,
            this.cmBestMatchShowLastChange});
            this.cmBestMatch.Name = "cmBestMatch";
            this.cmBestMatch.Size = new System.Drawing.Size(172, 70);
            // 
            // cmBestMatchZoom
            // 
            this.cmBestMatchZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmBestMatchZoom50,
            this.cmBestMatchZoom75,
            this.cmBestMatchZoom100,
            this.cmBestMatchZoom125,
            this.cmBestMatchZoom150,
            this.cmBestMatchZoom200,
            this.cmBestMatchZoomSeparator,
            this.cmBestMatchZoomSync});
            this.cmBestMatchZoom.Name = "cmBestMatchZoom";
            this.cmBestMatchZoom.Size = new System.Drawing.Size(171, 22);
            this.cmBestMatchZoom.Text = "Zoom";
            this.cmBestMatchZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmBestMatchZoom_DropDownItemClicked);
            // 
            // cmBestMatchZoom50
            // 
            this.cmBestMatchZoom50.Name = "cmBestMatchZoom50";
            this.cmBestMatchZoom50.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom50.Tag = ".5";
            this.cmBestMatchZoom50.Text = "50%";
            // 
            // cmBestMatchZoom75
            // 
            this.cmBestMatchZoom75.Name = "cmBestMatchZoom75";
            this.cmBestMatchZoom75.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom75.Tag = ".75";
            this.cmBestMatchZoom75.Text = "75%";
            // 
            // cmBestMatchZoom100
            // 
            this.cmBestMatchZoom100.Checked = true;
            this.cmBestMatchZoom100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmBestMatchZoom100.Name = "cmBestMatchZoom100";
            this.cmBestMatchZoom100.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom100.Tag = "1";
            this.cmBestMatchZoom100.Text = "100%";
            // 
            // cmBestMatchZoom125
            // 
            this.cmBestMatchZoom125.Name = "cmBestMatchZoom125";
            this.cmBestMatchZoom125.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom125.Tag = "1.25";
            this.cmBestMatchZoom125.Text = "125%";
            // 
            // cmBestMatchZoom150
            // 
            this.cmBestMatchZoom150.Name = "cmBestMatchZoom150";
            this.cmBestMatchZoom150.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom150.Tag = "1.5";
            this.cmBestMatchZoom150.Text = "150%";
            // 
            // cmBestMatchZoom200
            // 
            this.cmBestMatchZoom200.Name = "cmBestMatchZoom200";
            this.cmBestMatchZoom200.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoom200.Tag = "2";
            this.cmBestMatchZoom200.Text = "200%";
            // 
            // cmBestMatchZoomSeparator
            // 
            this.cmBestMatchZoomSeparator.Name = "cmBestMatchZoomSeparator";
            this.cmBestMatchZoomSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // cmBestMatchZoomSync
            // 
            this.cmBestMatchZoomSync.Name = "cmBestMatchZoomSync";
            this.cmBestMatchZoomSync.Size = new System.Drawing.Size(146, 22);
            this.cmBestMatchZoomSync.Text = "Sync all views";
            // 
            // cmBestMatchWireframe
            // 
            this.cmBestMatchWireframe.CheckOnClick = true;
            this.cmBestMatchWireframe.Name = "cmBestMatchWireframe";
            this.cmBestMatchWireframe.Size = new System.Drawing.Size(171, 22);
            this.cmBestMatchWireframe.Text = "Show Wireframe";
            this.cmBestMatchWireframe.CheckedChanged += new System.EventHandler(this.showWireframeToolStripMenuItem_CheckedChanged);
            // 
            // cmBestMatchShowLastChange
            // 
            this.cmBestMatchShowLastChange.CheckOnClick = true;
            this.cmBestMatchShowLastChange.Name = "cmBestMatchShowLastChange";
            this.cmBestMatchShowLastChange.Size = new System.Drawing.Size(171, 22);
            this.cmBestMatchShowLastChange.Text = "Show Last Change";
            this.cmBestMatchShowLastChange.CheckedChanged += new System.EventHandler(this.showLastChangeToolStripMenuItem_CheckedChanged);
            // 
            // picDiff
            // 
            this.picDiff.ContextMenuStrip = this.cmDiff;
            this.picDiff.Exaggerate = true;
            this.picDiff.Invert = false;
            this.picDiff.Location = new System.Drawing.Point(206, 5);
            this.picDiff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picDiff.Name = "picDiff";
            this.picDiff.ShowColor = true;
            this.picDiff.ShowLastChange = false;
            this.picDiff.Size = new System.Drawing.Size(93, 35);
            this.picDiff.TabIndex = 8;
            this.picDiff.Zoom = 1F;
            // 
            // cmDiff
            // 
            this.cmDiff.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmDiffZoom,
            this.cmDiffInvert,
            this.cmDiffShowColor,
            this.cmDiffExaggerate,
            this.cmDiffShowLastChange});
            this.cmDiff.Name = "cmBestMatch";
            this.cmDiff.Size = new System.Drawing.Size(172, 114);
            // 
            // cmDiffZoom
            // 
            this.cmDiffZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmDiffZoom50,
            this.cmDiffZoom75,
            this.cmDiffZoom100,
            this.cmDiffZoom125,
            this.cmDiffZoom150,
            this.cmDiffZoom200,
            this.cmDiffZoomSeparator,
            this.cmDiffZoomSync});
            this.cmDiffZoom.Name = "cmDiffZoom";
            this.cmDiffZoom.Size = new System.Drawing.Size(171, 22);
            this.cmDiffZoom.Text = "Zoom";
            this.cmDiffZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmDiffZoom_DropDownItemClicked);
            // 
            // cmDiffZoom50
            // 
            this.cmDiffZoom50.Name = "cmDiffZoom50";
            this.cmDiffZoom50.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom50.Tag = ".5";
            this.cmDiffZoom50.Text = "50%";
            // 
            // cmDiffZoom75
            // 
            this.cmDiffZoom75.Name = "cmDiffZoom75";
            this.cmDiffZoom75.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom75.Tag = ".75";
            this.cmDiffZoom75.Text = "75%";
            // 
            // cmDiffZoom100
            // 
            this.cmDiffZoom100.Checked = true;
            this.cmDiffZoom100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffZoom100.Name = "cmDiffZoom100";
            this.cmDiffZoom100.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom100.Tag = "1";
            this.cmDiffZoom100.Text = "100%";
            // 
            // cmDiffZoom125
            // 
            this.cmDiffZoom125.Name = "cmDiffZoom125";
            this.cmDiffZoom125.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom125.Tag = "1.25";
            this.cmDiffZoom125.Text = "125%";
            // 
            // cmDiffZoom150
            // 
            this.cmDiffZoom150.Name = "cmDiffZoom150";
            this.cmDiffZoom150.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom150.Tag = "1.5";
            this.cmDiffZoom150.Text = "150%";
            // 
            // cmDiffZoom200
            // 
            this.cmDiffZoom200.Name = "cmDiffZoom200";
            this.cmDiffZoom200.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoom200.Tag = "2";
            this.cmDiffZoom200.Text = "200%";
            // 
            // cmDiffZoomSeparator
            // 
            this.cmDiffZoomSeparator.Name = "cmDiffZoomSeparator";
            this.cmDiffZoomSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // cmDiffZoomSync
            // 
            this.cmDiffZoomSync.Name = "cmDiffZoomSync";
            this.cmDiffZoomSync.Size = new System.Drawing.Size(146, 22);
            this.cmDiffZoomSync.Text = "Sync all views";
            // 
            // cmDiffInvert
            // 
            this.cmDiffInvert.CheckOnClick = true;
            this.cmDiffInvert.Name = "cmDiffInvert";
            this.cmDiffInvert.Size = new System.Drawing.Size(171, 22);
            this.cmDiffInvert.Text = "Invert";
            this.cmDiffInvert.CheckedChanged += new System.EventHandler(this.cmDiffInvert_CheckedChanged);
            // 
            // cmDiffShowColor
            // 
            this.cmDiffShowColor.Checked = true;
            this.cmDiffShowColor.CheckOnClick = true;
            this.cmDiffShowColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffShowColor.Name = "cmDiffShowColor";
            this.cmDiffShowColor.Size = new System.Drawing.Size(171, 22);
            this.cmDiffShowColor.Text = "Show Color";
            this.cmDiffShowColor.CheckedChanged += new System.EventHandler(this.cmDiffShowColor_CheckedChanged);
            // 
            // cmDiffExaggerate
            // 
            this.cmDiffExaggerate.Checked = true;
            this.cmDiffExaggerate.CheckOnClick = true;
            this.cmDiffExaggerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmDiffExaggerate.Name = "cmDiffExaggerate";
            this.cmDiffExaggerate.Size = new System.Drawing.Size(171, 22);
            this.cmDiffExaggerate.Text = "Exaggerate";
            this.cmDiffExaggerate.CheckedChanged += new System.EventHandler(this.cmDiffExaggerate_CheckedChanged);
            // 
            // cmDiffShowLastChange
            // 
            this.cmDiffShowLastChange.CheckOnClick = true;
            this.cmDiffShowLastChange.Name = "cmDiffShowLastChange";
            this.cmDiffShowLastChange.Size = new System.Drawing.Size(171, 22);
            this.cmDiffShowLastChange.Text = "Show Last Change";
            this.cmDiffShowLastChange.Click += new System.EventHandler(this.cmDiffShowLastChange_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lScale);
            this.panel1.Controls.Add(this.tbEvalScale);
            this.panel1.Controls.Add(this.bEditEvaluatorSettings);
            this.panel1.Controls.Add(this.bEditMutatorSettings);
            this.panel1.Controls.Add(this.bEditInitializerSetting);
            this.panel1.Controls.Add(this.lEvaluator);
            this.panel1.Controls.Add(this.cEvaluator);
            this.panel1.Controls.Add(this.lMutator);
            this.panel1.Controls.Add(this.lInitializer);
            this.panel1.Controls.Add(this.cMutator);
            this.panel1.Controls.Add(this.cInitializer);
            this.panel1.Controls.Add(this.graphWindow1);
            this.panel1.Controls.Add(this.lPoints);
            this.panel1.Controls.Add(this.lShapes);
            this.panel1.Controls.Add(this.nPolygons);
            this.panel1.Controls.Add(this.nVertices);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(307, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 291);
            this.panel1.TabIndex = 6;
            // 
            // lScale
            // 
            this.lScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lScale.AutoSize = true;
            this.lScale.Location = new System.Drawing.Point(8, 250);
            this.lScale.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lScale.Name = "lScale";
            this.lScale.Size = new System.Drawing.Size(55, 15);
            this.lScale.TabIndex = 18;
            this.lScale.Text = "EvalScale";
            // 
            // tbEvalScale
            // 
            this.tbEvalScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEvalScale.LargeChange = 25;
            this.tbEvalScale.Location = new System.Drawing.Point(71, 245);
            this.tbEvalScale.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbEvalScale.Maximum = 100;
            this.tbEvalScale.Name = "tbEvalScale";
            this.tbEvalScale.Size = new System.Drawing.Size(162, 45);
            this.tbEvalScale.SmallChange = 5;
            this.tbEvalScale.TabIndex = 17;
            this.tbEvalScale.TickFrequency = 10;
            this.tbEvalScale.Value = 100;
            this.tbEvalScale.ValueChanged += new System.EventHandler(this.tbEvalScale_ValueChanged);
            // 
            // bEditEvaluatorSettings
            // 
            this.bEditEvaluatorSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEditEvaluatorSettings.Location = new System.Drawing.Point(211, 216);
            this.bEditEvaluatorSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bEditEvaluatorSettings.Name = "bEditEvaluatorSettings";
            this.bEditEvaluatorSettings.Size = new System.Drawing.Size(22, 24);
            this.bEditEvaluatorSettings.TabIndex = 16;
            this.bEditEvaluatorSettings.UseVisualStyleBackColor = true;
            this.bEditEvaluatorSettings.Click += new System.EventHandler(this.bEditEvaluatorSettings_Click);
            // 
            // bEditMutatorSettings
            // 
            this.bEditMutatorSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEditMutatorSettings.Location = new System.Drawing.Point(211, 185);
            this.bEditMutatorSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bEditMutatorSettings.Name = "bEditMutatorSettings";
            this.bEditMutatorSettings.Size = new System.Drawing.Size(22, 24);
            this.bEditMutatorSettings.TabIndex = 15;
            this.bEditMutatorSettings.UseVisualStyleBackColor = true;
            this.bEditMutatorSettings.Click += new System.EventHandler(this.bEditMutatorSettings_Click);
            // 
            // bEditInitializerSetting
            // 
            this.bEditInitializerSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEditInitializerSetting.Location = new System.Drawing.Point(211, 153);
            this.bEditInitializerSetting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bEditInitializerSetting.Name = "bEditInitializerSetting";
            this.bEditInitializerSetting.Size = new System.Drawing.Size(22, 24);
            this.bEditInitializerSetting.TabIndex = 14;
            this.bEditInitializerSetting.UseVisualStyleBackColor = true;
            this.bEditInitializerSetting.Click += new System.EventHandler(this.bEditInitializerSetting_Click);
            // 
            // lEvaluator
            // 
            this.lEvaluator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lEvaluator.AutoSize = true;
            this.lEvaluator.Location = new System.Drawing.Point(7, 222);
            this.lEvaluator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lEvaluator.Name = "lEvaluator";
            this.lEvaluator.Size = new System.Drawing.Size(56, 15);
            this.lEvaluator.TabIndex = 13;
            this.lEvaluator.Text = "Evaluator";
            // 
            // cEvaluator
            // 
            this.cEvaluator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cEvaluator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cEvaluator.FormattingEnabled = true;
            this.cEvaluator.Items.AddRange(new object[] {
            "RGB (Fast)",
            "RGB (Smooth)",
            "RGB+Luma (Fast)",
            "RGB+Luma (Smooth)",
            "RGB+Emphasis (Sloppy)"});
            this.cEvaluator.Location = new System.Drawing.Point(71, 216);
            this.cEvaluator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cEvaluator.Name = "cEvaluator";
            this.cEvaluator.Size = new System.Drawing.Size(132, 23);
            this.cEvaluator.TabIndex = 12;
            this.cEvaluator.SelectedIndexChanged += new System.EventHandler(this.cEvaluator_SelectedIndexChanged);
            // 
            // lMutator
            // 
            this.lMutator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lMutator.AutoSize = true;
            this.lMutator.Location = new System.Drawing.Point(13, 190);
            this.lMutator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lMutator.Name = "lMutator";
            this.lMutator.Size = new System.Drawing.Size(50, 15);
            this.lMutator.TabIndex = 11;
            this.lMutator.Text = "Mutator";
            // 
            // lInitializer
            // 
            this.lInitializer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lInitializer.AutoSize = true;
            this.lInitializer.Location = new System.Drawing.Point(9, 158);
            this.lInitializer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lInitializer.Name = "lInitializer";
            this.lInitializer.Size = new System.Drawing.Size(54, 15);
            this.lInitializer.TabIndex = 10;
            this.lInitializer.Text = "Initializer";
            // 
            // cMutator
            // 
            this.cMutator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cMutator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMutator.FormattingEnabled = true;
            this.cMutator.Items.AddRange(new object[] {
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
            "Soft Transform",
            "Hardish"});
            this.cMutator.Location = new System.Drawing.Point(71, 185);
            this.cMutator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cMutator.Name = "cMutator";
            this.cMutator.Size = new System.Drawing.Size(132, 23);
            this.cMutator.TabIndex = 9;
            this.cMutator.SelectedIndexChanged += new System.EventHandler(this.cMutator_SelectedIndexChanged);
            // 
            // cInitializer
            // 
            this.cInitializer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cInitializer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cInitializer.FormattingEnabled = true;
            this.cInitializer.Items.AddRange(new object[] {
            "FullRandom",
            "Segmented",
            "Radial"});
            this.cInitializer.Location = new System.Drawing.Point(71, 153);
            this.cInitializer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cInitializer.Name = "cInitializer";
            this.cInitializer.Size = new System.Drawing.Size(132, 23);
            this.cInitializer.TabIndex = 8;
            this.cInitializer.SelectedIndexChanged += new System.EventHandler(this.cInitializer_SelectedIndexChanged);
            // 
            // graphWindow1
            // 
            this.graphWindow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphWindow1.BackColor = System.Drawing.Color.White;
            this.graphWindow1.Location = new System.Drawing.Point(4, 3);
            this.graphWindow1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.graphWindow1.Name = "graphWindow1";
            this.graphWindow1.Size = new System.Drawing.Size(230, 113);
            this.graphWindow1.TabIndex = 9;
            // 
            // lPoints
            // 
            this.lPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lPoints.AutoSize = true;
            this.lPoints.Location = new System.Drawing.Point(23, 126);
            this.lPoints.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPoints.Name = "lPoints";
            this.lPoints.Size = new System.Drawing.Size(40, 15);
            this.lPoints.TabIndex = 7;
            this.lPoints.Text = "Points";
            // 
            // lShapes
            // 
            this.lShapes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lShapes.AutoSize = true;
            this.lShapes.Location = new System.Drawing.Point(130, 126);
            this.lShapes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lShapes.Name = "lShapes";
            this.lShapes.Size = new System.Drawing.Size(44, 15);
            this.lShapes.TabIndex = 6;
            this.lShapes.Text = "Shapes";
            // 
            // nPolygons
            // 
            this.nPolygons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nPolygons.Location = new System.Drawing.Point(182, 123);
            this.nPolygons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nPolygons.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nPolygons.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nPolygons.Name = "nPolygons";
            this.nPolygons.Size = new System.Drawing.Size(51, 23);
            this.nPolygons.TabIndex = 4;
            this.nPolygons.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nVertices
            // 
            this.nVertices.Location = new System.Drawing.Point(71, 123);
            this.nVertices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nVertices.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nVertices.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nVertices.Name = "nVertices";
            this.nVertices.Size = new System.Drawing.Size(47, 23);
            this.nVertices.TabIndex = 5;
            this.nVertices.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // pStatistics
            // 
            this.pStatistics.Controls.Add(this.tMutationStats);
            this.pStatistics.Location = new System.Drawing.Point(552, 5);
            this.pStatistics.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pStatistics.Name = "pStatistics";
            this.pStatistics.Size = new System.Drawing.Size(240, 290);
            this.pStatistics.TabIndex = 9;
            // 
            // tMutationStats
            // 
            this.tMutationStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tMutationStats.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tMutationStats.Location = new System.Drawing.Point(4, 3);
            this.tMutationStats.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tMutationStats.Multiline = true;
            this.tMutationStats.Name = "tMutationStats";
            this.tMutationStats.ReadOnly = true;
            this.tMutationStats.Size = new System.Drawing.Size(231, 284);
            this.tMutationStats.TabIndex = 7;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.menuOptions,
            this.menuHelp});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1029, 25);
            this.toolStrip.TabIndex = 5;
            // 
            // bNewProject
            // 
            this.bNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bNewProject.Image = global::SuperImageEvolver.Properties.Resources.document;
            this.bNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNewProject.Name = "bNewProject";
            this.bNewProject.Size = new System.Drawing.Size(23, 22);
            this.bNewProject.Text = "New Project...";
            this.bNewProject.Click += new System.EventHandler(this.bNewProject_Click);
            // 
            // bOpenProject
            // 
            this.bOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bOpenProject.Image = global::SuperImageEvolver.Properties.Resources.folder_horizontal_open;
            this.bOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenProject.Name = "bOpenProject";
            this.bOpenProject.Size = new System.Drawing.Size(23, 22);
            this.bOpenProject.Text = "Open Project...";
            this.bOpenProject.Click += new System.EventHandler(this.bOpenProject_Click);
            // 
            // bSaveProject
            // 
            this.bSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveProject.Enabled = false;
            this.bSaveProject.Image = global::SuperImageEvolver.Properties.Resources.disk_black;
            this.bSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveProject.Name = "bSaveProject";
            this.bSaveProject.Size = new System.Drawing.Size(23, 22);
            this.bSaveProject.Text = "Save Project";
            this.bSaveProject.Click += new System.EventHandler(this.bSaveProject_Click);
            // 
            // bSaveProjectAs
            // 
            this.bSaveProjectAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveProjectAs.Enabled = false;
            this.bSaveProjectAs.Image = global::SuperImageEvolver.Properties.Resources.disk__pencil;
            this.bSaveProjectAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveProjectAs.Name = "bSaveProjectAs";
            this.bSaveProjectAs.Size = new System.Drawing.Size(23, 22);
            this.bSaveProjectAs.Text = "Save Project As...";
            this.bSaveProjectAs.Click += new System.EventHandler(this.bSaveProjectAs_Click);
            // 
            // bRestart
            // 
            this.bRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRestart.Enabled = false;
            this.bRestart.Image = global::SuperImageEvolver.Properties.Resources.arrow_stop_180;
            this.bRestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRestart.Name = "bRestart";
            this.bRestart.Size = new System.Drawing.Size(23, 22);
            this.bRestart.Text = "Restart";
            this.bRestart.Click += new System.EventHandler(this.bRestart_Click);
            // 
            // bStart
            // 
            this.bStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bStart.Enabled = false;
            this.bStart.Image = global::SuperImageEvolver.Properties.Resources.control;
            this.bStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(23, 22);
            this.bStart.Text = "Start / Resume";
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // bStop
            // 
            this.bStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bStop.Enabled = false;
            this.bStop.Image = global::SuperImageEvolver.Properties.Resources.control_stop_square;
            this.bStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(23, 22);
            this.bStop.Text = "Pause / Stop";
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bViewOriginalImage,
            this.bViewBestMatchImage,
            this.bViewDifferenceImage,
            this.bViewStatistics});
            this.menuView.Image = global::SuperImageEvolver.Properties.Resources.images;
            this.menuView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(61, 22);
            this.menuView.Text = "View";
            // 
            // bViewOriginalImage
            // 
            this.bViewOriginalImage.Checked = true;
            this.bViewOriginalImage.CheckOnClick = true;
            this.bViewOriginalImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewOriginalImage.Name = "bViewOriginalImage";
            this.bViewOriginalImage.Size = new System.Drawing.Size(169, 22);
            this.bViewOriginalImage.Text = "Original Image";
            this.bViewOriginalImage.Click += new System.EventHandler(this.bViewOriginalImage_Click);
            // 
            // bViewBestMatchImage
            // 
            this.bViewBestMatchImage.Checked = true;
            this.bViewBestMatchImage.CheckOnClick = true;
            this.bViewBestMatchImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewBestMatchImage.Name = "bViewBestMatchImage";
            this.bViewBestMatchImage.Size = new System.Drawing.Size(169, 22);
            this.bViewBestMatchImage.Text = "Best Match Image";
            this.bViewBestMatchImage.Click += new System.EventHandler(this.bViewBestMatchImage_Click);
            // 
            // bViewDifferenceImage
            // 
            this.bViewDifferenceImage.Checked = true;
            this.bViewDifferenceImage.CheckOnClick = true;
            this.bViewDifferenceImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewDifferenceImage.Name = "bViewDifferenceImage";
            this.bViewDifferenceImage.Size = new System.Drawing.Size(169, 22);
            this.bViewDifferenceImage.Text = "Difference Image";
            this.bViewDifferenceImage.Click += new System.EventHandler(this.bViewDifferenceImage_Click);
            // 
            // bViewStatistics
            // 
            this.bViewStatistics.Checked = true;
            this.bViewStatistics.CheckOnClick = true;
            this.bViewStatistics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bViewStatistics.Name = "bViewStatistics";
            this.bViewStatistics.Size = new System.Drawing.Size(169, 22);
            this.bViewStatistics.Text = "Statistics";
            this.bViewStatistics.Click += new System.EventHandler(this.bViewStatistics_Click);
            // 
            // menuImport
            // 
            this.menuImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bImportDNA,
            this.bImportSVG});
            this.menuImport.Image = global::SuperImageEvolver.Properties.Resources.document_import;
            this.menuImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuImport.Name = "menuImport";
            this.menuImport.Size = new System.Drawing.Size(72, 22);
            this.menuImport.Text = "Import";
            // 
            // bImportDNA
            // 
            this.bImportDNA.Name = "bImportDNA";
            this.bImportDNA.Size = new System.Drawing.Size(214, 22);
            this.bImportDNA.Text = "DNA from ImageEvolution";
            this.bImportDNA.Click += new System.EventHandler(this.bImportDNA_Click);
            // 
            // bImportSVG
            // 
            this.bImportSVG.Enabled = false;
            this.bImportSVG.Image = global::SuperImageEvolver.Properties.Resources.layer_shape_polygon;
            this.bImportSVG.Name = "bImportSVG";
            this.bImportSVG.Size = new System.Drawing.Size(214, 22);
            this.bImportSVG.Text = "Best Match from SVG";
            // 
            // menuExport
            // 
            this.menuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bExportDNA,
            this.bExportImage,
            this.bExportVectors});
            this.menuExport.Enabled = false;
            this.menuExport.Image = global::SuperImageEvolver.Properties.Resources.document_export;
            this.menuExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuExport.Name = "menuExport";
            this.menuExport.Size = new System.Drawing.Size(70, 22);
            this.menuExport.Text = "Export";
            // 
            // bExportDNA
            // 
            this.bExportDNA.Name = "bExportDNA";
            this.bExportDNA.Size = new System.Drawing.Size(174, 22);
            this.bExportDNA.Text = "DNA";
            this.bExportDNA.Click += new System.EventHandler(this.bExportDNA_Click);
            // 
            // bExportImage
            // 
            this.bExportImage.Image = global::SuperImageEvolver.Properties.Resources.image_export;
            this.bExportImage.Name = "bExportImage";
            this.bExportImage.Size = new System.Drawing.Size(174, 22);
            this.bExportImage.Text = "Best Match Image";
            this.bExportImage.Click += new System.EventHandler(this.bExportImage_Click);
            // 
            // bExportVectors
            // 
            this.bExportVectors.Image = global::SuperImageEvolver.Properties.Resources.layer_shape_polygon;
            this.bExportVectors.Name = "bExportVectors";
            this.bExportVectors.Size = new System.Drawing.Size(174, 22);
            this.bExportVectors.Text = "Best Match Vectors";
            this.bExportVectors.Click += new System.EventHandler(this.bExportVectors_Click);
            // 
            // menuOptions
            // 
            this.menuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bProjectOptions,
            this.bMatteToAverageColor,
            this.polygonValueEvaluationToolStripMenuItem});
            this.menuOptions.Image = global::SuperImageEvolver.Properties.Resources.gear;
            this.menuOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(29, 22);
            this.menuOptions.Text = "toolStripDropDownButton1";
            this.menuOptions.ToolTipText = "Tools";
            // 
            // bProjectOptions
            // 
            this.bProjectOptions.Name = "bProjectOptions";
            this.bProjectOptions.Size = new System.Drawing.Size(216, 22);
            this.bProjectOptions.Text = "Project Options";
            this.bProjectOptions.Click += new System.EventHandler(this.bProjectOptions_Click);
            // 
            // bMatteToAverageColor
            // 
            this.bMatteToAverageColor.Name = "bMatteToAverageColor";
            this.bMatteToAverageColor.Size = new System.Drawing.Size(216, 22);
            this.bMatteToAverageColor.Text = "Set Matte to Average Color";
            this.bMatteToAverageColor.Click += new System.EventHandler(this.bMatteToAverageColor_Click);
            // 
            // polygonValueEvaluationToolStripMenuItem
            // 
            this.polygonValueEvaluationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.evaluateToolStripMenuItem,
            this.redistributeLeastValuableShapesToolStripMenuItem,
            this.clearOutlinesToolStripMenuItem});
            this.polygonValueEvaluationToolStripMenuItem.Name = "polygonValueEvaluationToolStripMenuItem";
            this.polygonValueEvaluationToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.polygonValueEvaluationToolStripMenuItem.Text = "Polygon Value";
            // 
            // evaluateToolStripMenuItem
            // 
            this.evaluateToolStripMenuItem.Name = "evaluateToolStripMenuItem";
            this.evaluateToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.evaluateToolStripMenuItem.Text = "Evaluate";
            this.evaluateToolStripMenuItem.Click += new System.EventHandler(this.evaluatePolygonValueToolStripMenuItem_Click);
            // 
            // redistributeLeastValuableShapesToolStripMenuItem
            // 
            this.redistributeLeastValuableShapesToolStripMenuItem.Name = "redistributeLeastValuableShapesToolStripMenuItem";
            this.redistributeLeastValuableShapesToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.redistributeLeastValuableShapesToolStripMenuItem.Text = "Redistribute least valuable shapes";
            this.redistributeLeastValuableShapesToolStripMenuItem.Click += new System.EventHandler(this.eliminateLVPToolStripMenuItem_Click);
            // 
            // clearOutlinesToolStripMenuItem
            // 
            this.clearOutlinesToolStripMenuItem.Name = "clearOutlinesToolStripMenuItem";
            this.clearOutlinesToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.clearOutlinesToolStripMenuItem.Text = "Clear outlines";
            this.clearOutlinesToolStripMenuItem.Click += new System.EventHandler(this.clearOutlinesToolStripMenuItem_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bHelpListModules});
            this.menuHelp.Image = global::SuperImageEvolver.Properties.Resources.information;
            this.menuHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(29, 22);
            this.menuHelp.ToolTipText = "Help";
            // 
            // bHelpListModules
            // 
            this.bHelpListModules.Name = "bHelpListModules";
            this.bHelpListModules.Size = new System.Drawing.Size(141, 22);
            this.bHelpListModules.Text = "List Modules";
            this.bHelpListModules.Click += new System.EventHandler(this.bHelpListModules_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1029, 560);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "Super Image Evolver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.cmOriginal.ResumeLayout(false);
            this.cmBestMatch.ResumeLayout(false);
            this.cmDiff.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbEvalScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPolygons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nVertices)).EndInit();
            this.pStatistics.ResumeLayout(false);
            this.pStatistics.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SuperImageEvolver.Canvas picBestMatch;
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
        private SuperImageEvolver.DiffCanvas picDiff;
        private SuperImageEvolver.GraphWindow graphWindow1;
        private System.Windows.Forms.Panel pStatistics;
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
        private System.Windows.Forms.ToolStripMenuItem cmDiffShowLastChange;
        private System.Windows.Forms.ToolStripDropDownButton menuOptions;
        private System.Windows.Forms.ToolStripMenuItem bProjectOptions;
        private System.Windows.Forms.ToolStripSeparator cmBestMatchZoomSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmBestMatchZoomSync;
        private System.Windows.Forms.ContextMenuStrip cmOriginal;
        private System.Windows.Forms.ToolStripSeparator cmDiffZoomSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmDiffZoomSync;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom50;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom75;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom100;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom125;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom150;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoom200;
        private System.Windows.Forms.ToolStripSeparator cmOriginalZoomSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmOriginalZoomSync;
        private System.Windows.Forms.ToolStripMenuItem bMatteToAverageColor;
        private CustomInterpolationPictureBox picOriginal;
        private System.Windows.Forms.ToolStripMenuItem polygonValueEvaluationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redistributeLeastValuableShapesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearOutlinesToolStripMenuItem;
        private System.Windows.Forms.TrackBar tbEvalScale;
        private System.Windows.Forms.Label lScale;
        private System.Windows.Forms.ToolTip ttEvalScale;
    }
}

