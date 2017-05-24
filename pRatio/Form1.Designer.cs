namespace pRatio
{
    partial class frmMain
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.runBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsWhat = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cbxResultsFolder = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarFDR = new System.Windows.Forms.TrackBar();
            this.FDRtxt = new System.Windows.Forms.TextBox();
            this.gbxQuality = new System.Windows.Forms.GroupBox();
            this.rbnUseXCorr = new System.Windows.Forms.RadioButton();
            this.qualityAvg = new System.Windows.Forms.RadioButton();
            this.quality4 = new System.Windows.Forms.RadioButton();
            this.quality3 = new System.Windows.Forms.RadioButton();
            this.quality2 = new System.Windows.Forms.RadioButton();
            this.gbxpRatioCorrection = new System.Windows.Forms.GroupBox();
            this.chbPratioCorrection = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lnkParams = new System.Windows.Forms.Label();
            this.btnLinkParams = new System.Windows.Forms.Button();
            this.lblParams = new System.Windows.Forms.Label();
            this.chbPepXML = new System.Windows.Forms.CheckBox();
            this.HelpLbl = new System.Windows.Forms.Label();
            this.gbxpIbox = new System.Windows.Forms.GroupBox();
            this.FDRpIcutoffTxt = new System.Windows.Forms.TextBox();
            this.pIcheck = new System.Windows.Forms.CheckBox();
            this.FDRpIcutOffLbl = new System.Windows.Forms.Label();
            this.rbnMixed = new System.Windows.Forms.RadioButton();
            this.rbnSeparated = new System.Windows.Forms.RadioButton();
            this.rbnConcatenated = new System.Windows.Forms.RadioButton();
            this.gbxFDRmethod = new System.Windows.Forms.GroupBox();
            this.cbxAdvanced = new System.Windows.Forms.CheckBox();
            this.cbxSplit = new System.Windows.Forms.CheckBox();
            this.tbxSplit = new System.Windows.Forms.TextBox();
            this.lblSplit = new System.Windows.Forms.Label();
            this.gbxSplit = new System.Windows.Forms.GroupBox();
            this.gbxDeltaMass = new System.Windows.Forms.GroupBox();
            this.rbnDMAreaFive = new System.Windows.Forms.RadioButton();
            this.rbnDMAreaThree = new System.Windows.Forms.RadioButton();
            this.rbnDMAreaOne = new System.Windows.Forms.RadioButton();
            this.cbxDeltaMass = new System.Windows.Forms.CheckBox();
            this.lblDeltaMass = new System.Windows.Forms.Label();
            this.tbxDeltaMass = new System.Windows.Forms.TextBox();
            this.lblNtermStaticModif = new System.Windows.Forms.Label();
            this.tbxNtermStaticModif = new System.Windows.Forms.TextBox();
            this.gbxNterminalModifs = new System.Windows.Forms.GroupBox();
            this.lblDecoySearch = new System.Windows.Forms.Label();
            this.lblTargetSearch = new System.Windows.Forms.Label();
            this.btnDSFolder = new System.Windows.Forms.Button();
            this.gbxPDVersion = new System.Windows.Forms.GroupBox();
            this.rbnPD20 = new System.Windows.Forms.RadioButton();
            this.rbnPD14 = new System.Windows.Forms.RadioButton();
            this.btnTSFolder = new System.Windows.Forms.Button();
            this.tbxTargetSearch = new System.Windows.Forms.TextBox();
            this.tbxDecoySearch = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxResultsFolder = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFDR)).BeginInit();
            this.gbxQuality.SuspendLayout();
            this.gbxpRatioCorrection.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gbxpIbox.SuspendLayout();
            this.gbxFDRmethod.SuspendLayout();
            this.gbxSplit.SuspendLayout();
            this.gbxDeltaMass.SuspendLayout();
            this.gbxNterminalModifs.SuspendLayout();
            this.gbxPDVersion.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runBtn.Location = new System.Drawing.Point(827, 119);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 34);
            this.runBtn.TabIndex = 6;
            this.runBtn.Text = "Run !";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tStripProgressBar,
            this.tsWhat,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 388);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(890, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(52, 17);
            this.toolStripStatusLabel1.Text = "progress";
            // 
            // tStripProgressBar
            // 
            this.tStripProgressBar.Maximum = 600;
            this.tStripProgressBar.Name = "tStripProgressBar";
            this.tStripProgressBar.Size = new System.Drawing.Size(600, 16);
            this.tStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tsWhat
            // 
            this.tsWhat.AutoToolTip = true;
            this.tsWhat.Name = "tsWhat";
            this.tsWhat.Size = new System.Drawing.Size(88, 17);
            this.tsWhat.Text = "what is it doing";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // cbxResultsFolder
            // 
            this.cbxResultsFolder.Location = new System.Drawing.Point(615, 23);
            this.cbxResultsFolder.Name = "cbxResultsFolder";
            this.cbxResultsFolder.Size = new System.Drawing.Size(177, 27);
            this.cbxResultsFolder.TabIndex = 21;
            this.cbxResultsFolder.Text = "Save results in a different folder";
            this.cbxResultsFolder.UseVisualStyleBackColor = true;
            this.cbxResultsFolder.CheckedChanged += new System.EventHandler(this.cbxResultsFolder_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(606, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "FDR cut-off";
            // 
            // trackBarFDR
            // 
            this.trackBarFDR.Location = new System.Drawing.Point(674, 119);
            this.trackBarFDR.Name = "trackBarFDR";
            this.trackBarFDR.Size = new System.Drawing.Size(66, 45);
            this.trackBarFDR.TabIndex = 4;
            // 
            // FDRtxt
            // 
            this.FDRtxt.Location = new System.Drawing.Point(755, 128);
            this.FDRtxt.Name = "FDRtxt";
            this.FDRtxt.Size = new System.Drawing.Size(49, 20);
            this.FDRtxt.TabIndex = 5;
            // 
            // gbxQuality
            // 
            this.gbxQuality.Controls.Add(this.rbnUseXCorr);
            this.gbxQuality.Controls.Add(this.qualityAvg);
            this.gbxQuality.Controls.Add(this.quality4);
            this.gbxQuality.Controls.Add(this.quality3);
            this.gbxQuality.Controls.Add(this.quality2);
            this.gbxQuality.Location = new System.Drawing.Point(518, 166);
            this.gbxQuality.Name = "gbxQuality";
            this.gbxQuality.Size = new System.Drawing.Size(191, 152);
            this.gbxQuality.TabIndex = 11;
            this.gbxQuality.TabStop = false;
            this.gbxQuality.Text = "quality determination";
            this.gbxQuality.Visible = false;
            // 
            // rbnUseXCorr
            // 
            this.rbnUseXCorr.AutoSize = true;
            this.rbnUseXCorr.Location = new System.Drawing.Point(17, 118);
            this.rbnUseXCorr.Name = "rbnUseXCorr";
            this.rbnUseXCorr.Size = new System.Drawing.Size(154, 17);
            this.rbnUseXCorr.TabIndex = 4;
            this.rbnUseXCorr.Text = "use XCorr instead of pRatio";
            this.rbnUseXCorr.UseVisualStyleBackColor = true;
            // 
            // qualityAvg
            // 
            this.qualityAvg.AutoSize = true;
            this.qualityAvg.Location = new System.Drawing.Point(17, 95);
            this.qualityAvg.Name = "qualityAvg";
            this.qualityAvg.Size = new System.Drawing.Size(103, 17);
            this.qualityAvg.TabIndex = 3;
            this.qualityAvg.Text = "averaged quality";
            this.qualityAvg.UseVisualStyleBackColor = true;
            // 
            // quality4
            // 
            this.quality4.AutoSize = true;
            this.quality4.Location = new System.Drawing.Point(17, 72);
            this.quality4.Name = "quality4";
            this.quality4.Size = new System.Drawing.Size(124, 17);
            this.quality4.TabIndex = 2;
            this.quality4.Text = "score from 4th match";
            this.quality4.UseVisualStyleBackColor = true;
            // 
            // quality3
            // 
            this.quality3.AutoSize = true;
            this.quality3.Location = new System.Drawing.Point(17, 49);
            this.quality3.Name = "quality3";
            this.quality3.Size = new System.Drawing.Size(124, 17);
            this.quality3.TabIndex = 1;
            this.quality3.Text = "score from 3rd match";
            this.quality3.UseVisualStyleBackColor = true;
            // 
            // quality2
            // 
            this.quality2.AutoSize = true;
            this.quality2.Checked = true;
            this.quality2.Location = new System.Drawing.Point(17, 26);
            this.quality2.Name = "quality2";
            this.quality2.Size = new System.Drawing.Size(127, 17);
            this.quality2.TabIndex = 0;
            this.quality2.TabStop = true;
            this.quality2.Text = "score from 2nd match";
            this.quality2.UseVisualStyleBackColor = true;
            // 
            // gbxpRatioCorrection
            // 
            this.gbxpRatioCorrection.Controls.Add(this.chbPratioCorrection);
            this.gbxpRatioCorrection.Enabled = false;
            this.gbxpRatioCorrection.Location = new System.Drawing.Point(518, 324);
            this.gbxpRatioCorrection.Name = "gbxpRatioCorrection";
            this.gbxpRatioCorrection.Size = new System.Drawing.Size(191, 67);
            this.gbxpRatioCorrection.TabIndex = 12;
            this.gbxpRatioCorrection.TabStop = false;
            this.gbxpRatioCorrection.Text = "pRatio correction";
            this.gbxpRatioCorrection.Visible = false;
            // 
            // chbPratioCorrection
            // 
            this.chbPratioCorrection.AutoSize = true;
            this.chbPratioCorrection.Checked = true;
            this.chbPratioCorrection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPratioCorrection.Location = new System.Drawing.Point(55, 28);
            this.chbPratioCorrection.Name = "chbPratioCorrection";
            this.chbPratioCorrection.Size = new System.Drawing.Size(65, 17);
            this.chbPratioCorrection.TabIndex = 2;
            this.chbPratioCorrection.Text = "On / Off";
            this.chbPratioCorrection.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lnkParams);
            this.groupBox4.Controls.Add(this.btnLinkParams);
            this.groupBox4.Controls.Add(this.lblParams);
            this.groupBox4.Controls.Add(this.chbPepXML);
            this.groupBox4.Location = new System.Drawing.Point(121, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(463, 47);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "pepXML";
            // 
            // lnkParams
            // 
            this.lnkParams.AllowDrop = true;
            this.lnkParams.AutoSize = true;
            this.lnkParams.Location = new System.Drawing.Point(332, 20);
            this.lnkParams.Name = "lnkParams";
            this.lnkParams.Size = new System.Drawing.Size(118, 13);
            this.lnkParams.TabIndex = 3;
            this.lnkParams.Text = "params file not selected";
            this.lnkParams.Visible = false;
            this.lnkParams.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblParams_DragDrop);
            // 
            // btnLinkParams
            // 
            this.btnLinkParams.AllowDrop = true;
            this.btnLinkParams.Location = new System.Drawing.Point(299, 16);
            this.btnLinkParams.Name = "btnLinkParams";
            this.btnLinkParams.Size = new System.Drawing.Size(27, 20);
            this.btnLinkParams.TabIndex = 2;
            this.btnLinkParams.Text = "...";
            this.btnLinkParams.UseVisualStyleBackColor = true;
            this.btnLinkParams.Visible = false;
            this.btnLinkParams.Click += new System.EventHandler(this.btnLinkParams_Click);
            this.btnLinkParams.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblParams_DragDrop);
            this.btnLinkParams.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // lblParams
            // 
            this.lblParams.AllowDrop = true;
            this.lblParams.AutoSize = true;
            this.lblParams.Location = new System.Drawing.Point(183, 20);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new System.Drawing.Size(117, 13);
            this.lblParams.TabIndex = 1;
            this.lblParams.Text = "SEQUEST params file :";
            this.lblParams.Visible = false;
            this.lblParams.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblParams_DragDrop);
            this.lblParams.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // chbPepXML
            // 
            this.chbPepXML.AutoSize = true;
            this.chbPepXML.Location = new System.Drawing.Point(21, 19);
            this.chbPepXML.Name = "chbPepXML";
            this.chbPepXML.Size = new System.Drawing.Size(152, 17);
            this.chbPepXML.TabIndex = 0;
            this.chbPepXML.Text = "Write a pepXML results file";
            this.chbPepXML.UseVisualStyleBackColor = true;
            this.chbPepXML.CheckedChanged += new System.EventHandler(this.chbPepXML_CheckedChanged);
            // 
            // HelpLbl
            // 
            this.HelpLbl.AutoSize = true;
            this.HelpLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpLbl.Location = new System.Drawing.Point(830, 9);
            this.HelpLbl.Name = "HelpLbl";
            this.HelpLbl.Size = new System.Drawing.Size(46, 20);
            this.HelpLbl.TabIndex = 14;
            this.HelpLbl.Text = "Help";
            this.HelpLbl.Click += new System.EventHandler(this.HelpLbl_Click);
            // 
            // gbxpIbox
            // 
            this.gbxpIbox.Controls.Add(this.FDRpIcutoffTxt);
            this.gbxpIbox.Controls.Add(this.pIcheck);
            this.gbxpIbox.Controls.Add(this.FDRpIcutOffLbl);
            this.gbxpIbox.Location = new System.Drawing.Point(21, 320);
            this.gbxpIbox.Name = "gbxpIbox";
            this.gbxpIbox.Size = new System.Drawing.Size(264, 71);
            this.gbxpIbox.TabIndex = 1;
            this.gbxpIbox.TabStop = false;
            this.gbxpIbox.Text = "pI options";
            this.gbxpIbox.Visible = false;
            // 
            // FDRpIcutoffTxt
            // 
            this.FDRpIcutoffTxt.Enabled = false;
            this.FDRpIcutoffTxt.Location = new System.Drawing.Point(207, 39);
            this.FDRpIcutoffTxt.Name = "FDRpIcutoffTxt";
            this.FDRpIcutoffTxt.Size = new System.Drawing.Size(49, 20);
            this.FDRpIcutoffTxt.TabIndex = 2;
            this.FDRpIcutoffTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pIcheck
            // 
            this.pIcheck.AutoSize = true;
            this.pIcheck.Location = new System.Drawing.Point(18, 17);
            this.pIcheck.Name = "pIcheck";
            this.pIcheck.Size = new System.Drawing.Size(160, 17);
            this.pIcheck.TabIndex = 0;
            this.pIcheck.Text = "Use pI as a probability factor";
            this.pIcheck.UseVisualStyleBackColor = true;
            this.pIcheck.CheckedChanged += new System.EventHandler(this.pIcheck_CheckedChanged);
            // 
            // FDRpIcutOffLbl
            // 
            this.FDRpIcutOffLbl.Enabled = false;
            this.FDRpIcutOffLbl.Location = new System.Drawing.Point(30, 35);
            this.FDRpIcutOffLbl.Name = "FDRpIcutOffLbl";
            this.FDRpIcutOffLbl.Size = new System.Drawing.Size(165, 32);
            this.FDRpIcutOffLbl.TabIndex = 1;
            this.FDRpIcutOffLbl.Text = "If wanted, use a different FDR for pI calculation :";
            // 
            // rbnMixed
            // 
            this.rbnMixed.AutoSize = true;
            this.rbnMixed.Checked = true;
            this.rbnMixed.Location = new System.Drawing.Point(25, 31);
            this.rbnMixed.Name = "rbnMixed";
            this.rbnMixed.Size = new System.Drawing.Size(52, 17);
            this.rbnMixed.TabIndex = 0;
            this.rbnMixed.TabStop = true;
            this.rbnMixed.Text = "mixed";
            this.rbnMixed.UseVisualStyleBackColor = true;
            // 
            // rbnSeparated
            // 
            this.rbnSeparated.AutoSize = true;
            this.rbnSeparated.Location = new System.Drawing.Point(25, 49);
            this.rbnSeparated.Name = "rbnSeparated";
            this.rbnSeparated.Size = new System.Drawing.Size(72, 17);
            this.rbnSeparated.TabIndex = 1;
            this.rbnSeparated.Text = "separated";
            this.rbnSeparated.UseVisualStyleBackColor = true;
            // 
            // rbnConcatenated
            // 
            this.rbnConcatenated.AutoSize = true;
            this.rbnConcatenated.Location = new System.Drawing.Point(25, 67);
            this.rbnConcatenated.Name = "rbnConcatenated";
            this.rbnConcatenated.Size = new System.Drawing.Size(91, 17);
            this.rbnConcatenated.TabIndex = 2;
            this.rbnConcatenated.Text = "concatenated";
            this.rbnConcatenated.UseVisualStyleBackColor = true;
            // 
            // gbxFDRmethod
            // 
            this.gbxFDRmethod.Controls.Add(this.rbnConcatenated);
            this.gbxFDRmethod.Controls.Add(this.rbnSeparated);
            this.gbxFDRmethod.Controls.Add(this.rbnMixed);
            this.gbxFDRmethod.Location = new System.Drawing.Point(715, 170);
            this.gbxFDRmethod.Name = "gbxFDRmethod";
            this.gbxFDRmethod.Size = new System.Drawing.Size(187, 108);
            this.gbxFDRmethod.TabIndex = 12;
            this.gbxFDRmethod.TabStop = false;
            this.gbxFDRmethod.Text = "FDR method";
            this.gbxFDRmethod.Visible = false;
            // 
            // cbxAdvanced
            // 
            this.cbxAdvanced.Location = new System.Drawing.Point(40, 116);
            this.cbxAdvanced.Name = "cbxAdvanced";
            this.cbxAdvanced.Size = new System.Drawing.Size(75, 43);
            this.cbxAdvanced.TabIndex = 6;
            this.cbxAdvanced.Text = "Show advanced options";
            this.cbxAdvanced.UseVisualStyleBackColor = true;
            this.cbxAdvanced.CheckedChanged += new System.EventHandler(this.cbxAdvanced_CheckedChanged);
            // 
            // cbxSplit
            // 
            this.cbxSplit.Location = new System.Drawing.Point(12, 19);
            this.cbxSplit.Name = "cbxSplit";
            this.cbxSplit.Size = new System.Drawing.Size(169, 56);
            this.cbxSplit.TabIndex = 16;
            this.cbxSplit.Text = "Split into different QuiXML files (important for limited RAM)";
            this.cbxSplit.UseVisualStyleBackColor = true;
            this.cbxSplit.CheckedChanged += new System.EventHandler(this.cbxSplit_CheckedChanged);
            // 
            // tbxSplit
            // 
            this.tbxSplit.Enabled = false;
            this.tbxSplit.Location = new System.Drawing.Point(133, 92);
            this.tbxSplit.Name = "tbxSplit";
            this.tbxSplit.Size = new System.Drawing.Size(74, 20);
            this.tbxSplit.TabIndex = 15;
            this.tbxSplit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSplit
            // 
            this.lblSplit.AutoSize = true;
            this.lblSplit.Enabled = false;
            this.lblSplit.Location = new System.Drawing.Point(50, 95);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(63, 13);
            this.lblSplit.TabIndex = 17;
            this.lblSplit.Text = "How many?";
            // 
            // gbxSplit
            // 
            this.gbxSplit.Controls.Add(this.cbxSplit);
            this.gbxSplit.Controls.Add(this.lblSplit);
            this.gbxSplit.Controls.Add(this.tbxSplit);
            this.gbxSplit.Location = new System.Drawing.Point(291, 261);
            this.gbxSplit.Name = "gbxSplit";
            this.gbxSplit.Size = new System.Drawing.Size(221, 130);
            this.gbxSplit.TabIndex = 4;
            this.gbxSplit.TabStop = false;
            this.gbxSplit.Text = "splitting options";
            // 
            // gbxDeltaMass
            // 
            this.gbxDeltaMass.Controls.Add(this.rbnDMAreaFive);
            this.gbxDeltaMass.Controls.Add(this.rbnDMAreaThree);
            this.gbxDeltaMass.Controls.Add(this.rbnDMAreaOne);
            this.gbxDeltaMass.Controls.Add(this.cbxDeltaMass);
            this.gbxDeltaMass.Controls.Add(this.lblDeltaMass);
            this.gbxDeltaMass.Controls.Add(this.tbxDeltaMass);
            this.gbxDeltaMass.Location = new System.Drawing.Point(21, 165);
            this.gbxDeltaMass.Name = "gbxDeltaMass";
            this.gbxDeltaMass.Size = new System.Drawing.Size(264, 153);
            this.gbxDeltaMass.TabIndex = 18;
            this.gbxDeltaMass.TabStop = false;
            this.gbxDeltaMass.Text = "delta mass discrimination";
            // 
            // rbnDMAreaFive
            // 
            this.rbnDMAreaFive.AutoSize = true;
            this.rbnDMAreaFive.Enabled = false;
            this.rbnDMAreaFive.Location = new System.Drawing.Point(33, 119);
            this.rbnDMAreaFive.Name = "rbnDMAreaFive";
            this.rbnDMAreaFive.Size = new System.Drawing.Size(217, 17);
            this.rbnDMAreaFive.TabIndex = 20;
            this.rbnDMAreaFive.Text = "check also ±1 and ±2 Da away (5 areas)";
            this.rbnDMAreaFive.UseVisualStyleBackColor = true;
            // 
            // rbnDMAreaThree
            // 
            this.rbnDMAreaThree.AutoSize = true;
            this.rbnDMAreaThree.Enabled = false;
            this.rbnDMAreaThree.Location = new System.Drawing.Point(33, 96);
            this.rbnDMAreaThree.Name = "rbnDMAreaThree";
            this.rbnDMAreaThree.Size = new System.Drawing.Size(181, 17);
            this.rbnDMAreaThree.TabIndex = 19;
            this.rbnDMAreaThree.Text = "check also ±1 Da away (3 areas)";
            this.rbnDMAreaThree.UseVisualStyleBackColor = true;
            // 
            // rbnDMAreaOne
            // 
            this.rbnDMAreaOne.AutoSize = true;
            this.rbnDMAreaOne.Checked = true;
            this.rbnDMAreaOne.Enabled = false;
            this.rbnDMAreaOne.Location = new System.Drawing.Point(33, 73);
            this.rbnDMAreaOne.Name = "rbnDMAreaOne";
            this.rbnDMAreaOne.Size = new System.Drawing.Size(205, 17);
            this.rbnDMAreaOne.TabIndex = 18;
            this.rbnDMAreaOne.TabStop = true;
            this.rbnDMAreaOne.Text = "check only around the precursor mass";
            this.rbnDMAreaOne.UseVisualStyleBackColor = true;
            // 
            // cbxDeltaMass
            // 
            this.cbxDeltaMass.AutoSize = true;
            this.cbxDeltaMass.Location = new System.Drawing.Point(21, 20);
            this.cbxDeltaMass.Name = "cbxDeltaMass";
            this.cbxDeltaMass.Size = new System.Drawing.Size(235, 17);
            this.cbxDeltaMass.TabIndex = 16;
            this.cbxDeltaMass.Text = "Use the delta mass to filter out identifications";
            this.cbxDeltaMass.UseVisualStyleBackColor = true;
            this.cbxDeltaMass.CheckedChanged += new System.EventHandler(this.cbxDeltaMass_CheckedChanged);
            // 
            // lblDeltaMass
            // 
            this.lblDeltaMass.AutoSize = true;
            this.lblDeltaMass.Enabled = false;
            this.lblDeltaMass.Location = new System.Drawing.Point(43, 49);
            this.lblDeltaMass.Name = "lblDeltaMass";
            this.lblDeltaMass.Size = new System.Drawing.Size(152, 13);
            this.lblDeltaMass.TabIndex = 17;
            this.lblDeltaMass.Text = "± delta mass threshold (in ppm)";
            // 
            // tbxDeltaMass
            // 
            this.tbxDeltaMass.Enabled = false;
            this.tbxDeltaMass.Location = new System.Drawing.Point(201, 46);
            this.tbxDeltaMass.Name = "tbxDeltaMass";
            this.tbxDeltaMass.Size = new System.Drawing.Size(49, 20);
            this.tbxDeltaMass.TabIndex = 15;
            this.tbxDeltaMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNtermStaticModif
            // 
            this.lblNtermStaticModif.Location = new System.Drawing.Point(14, 16);
            this.lblNtermStaticModif.Name = "lblNtermStaticModif";
            this.lblNtermStaticModif.Size = new System.Drawing.Size(197, 27);
            this.lblNtermStaticModif.TabIndex = 19;
            this.lblNtermStaticModif.Text = "Take into account a static modification, such as iTRAQ in the Nterminal (in Da)";
            // 
            // tbxNtermStaticModif
            // 
            this.tbxNtermStaticModif.Location = new System.Drawing.Point(133, 54);
            this.tbxNtermStaticModif.Name = "tbxNtermStaticModif";
            this.tbxNtermStaticModif.Size = new System.Drawing.Size(78, 20);
            this.tbxNtermStaticModif.TabIndex = 18;
            this.tbxNtermStaticModif.Text = "0";
            this.tbxNtermStaticModif.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbxNterminalModifs
            // 
            this.gbxNterminalModifs.Controls.Add(this.lblNtermStaticModif);
            this.gbxNterminalModifs.Controls.Add(this.tbxNtermStaticModif);
            this.gbxNterminalModifs.Location = new System.Drawing.Point(291, 165);
            this.gbxNterminalModifs.Name = "gbxNterminalModifs";
            this.gbxNterminalModifs.Size = new System.Drawing.Size(221, 90);
            this.gbxNterminalModifs.TabIndex = 20;
            this.gbxNterminalModifs.TabStop = false;
            this.gbxNterminalModifs.Text = "Nterminal modifications";
            this.gbxNterminalModifs.Visible = false;
            // 
            // lblDecoySearch
            // 
            this.lblDecoySearch.AllowDrop = true;
            this.lblDecoySearch.AutoSize = true;
            this.lblDecoySearch.Location = new System.Drawing.Point(19, 57);
            this.lblDecoySearch.Name = "lblDecoySearch";
            this.lblDecoySearch.Size = new System.Drawing.Size(76, 13);
            this.lblDecoySearch.TabIndex = 3;
            this.lblDecoySearch.Text = "Decoy search:";
            this.lblDecoySearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.decoySearch_DragDrop);
            this.lblDecoySearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // lblTargetSearch
            // 
            this.lblTargetSearch.AllowDrop = true;
            this.lblTargetSearch.AutoSize = true;
            this.lblTargetSearch.Location = new System.Drawing.Point(19, 29);
            this.lblTargetSearch.Name = "lblTargetSearch";
            this.lblTargetSearch.Size = new System.Drawing.Size(76, 13);
            this.lblTargetSearch.TabIndex = 0;
            this.lblTargetSearch.Text = "Target search:";
            this.lblTargetSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.targetSearch_DragDrop);
            this.lblTargetSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // btnDSFolder
            // 
            this.btnDSFolder.AllowDrop = true;
            this.btnDSFolder.Location = new System.Drawing.Point(100, 54);
            this.btnDSFolder.Name = "btnDSFolder";
            this.btnDSFolder.Size = new System.Drawing.Size(27, 19);
            this.btnDSFolder.TabIndex = 4;
            this.btnDSFolder.TabStop = false;
            this.btnDSFolder.Text = "...";
            this.btnDSFolder.UseVisualStyleBackColor = true;
            this.btnDSFolder.Visible = false;
            this.btnDSFolder.Click += new System.EventHandler(this.btnDSFolder_Click);
            this.btnDSFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.decoySearch_DragDrop);
            // 
            // gbxPDVersion
            // 
            this.gbxPDVersion.Controls.Add(this.rbnPD20);
            this.gbxPDVersion.Controls.Add(this.rbnPD14);
            this.gbxPDVersion.Location = new System.Drawing.Point(435, 13);
            this.gbxPDVersion.Name = "gbxPDVersion";
            this.gbxPDVersion.Size = new System.Drawing.Size(168, 63);
            this.gbxPDVersion.TabIndex = 12;
            this.gbxPDVersion.TabStop = false;
            this.gbxPDVersion.Text = "Proteome Discoverer version";
            // 
            // rbnPD20
            // 
            this.rbnPD20.AutoSize = true;
            this.rbnPD20.Location = new System.Drawing.Point(15, 36);
            this.rbnPD20.Name = "rbnPD20";
            this.rbnPD20.Size = new System.Drawing.Size(82, 17);
            this.rbnPD20.TabIndex = 1;
            this.rbnPD20.Text = "PD 2.0 - 2.1";
            this.rbnPD20.UseVisualStyleBackColor = true;
            this.rbnPD20.CheckedChanged += new System.EventHandler(this.rbnPD20_CheckedChanged);
            // 
            // rbnPD14
            // 
            this.rbnPD14.AutoSize = true;
            this.rbnPD14.Checked = true;
            this.rbnPD14.Location = new System.Drawing.Point(15, 17);
            this.rbnPD14.Name = "rbnPD14";
            this.rbnPD14.Size = new System.Drawing.Size(122, 17);
            this.rbnPD14.TabIndex = 0;
            this.rbnPD14.TabStop = true;
            this.rbnPD14.Text = "PD 1.4 and previous";
            this.rbnPD14.UseVisualStyleBackColor = true;
            // 
            // btnTSFolder
            // 
            this.btnTSFolder.AllowDrop = true;
            this.btnTSFolder.Enabled = false;
            this.btnTSFolder.Location = new System.Drawing.Point(100, 25);
            this.btnTSFolder.Name = "btnTSFolder";
            this.btnTSFolder.Size = new System.Drawing.Size(27, 20);
            this.btnTSFolder.TabIndex = 1;
            this.btnTSFolder.Text = "...";
            this.btnTSFolder.UseVisualStyleBackColor = true;
            this.btnTSFolder.Visible = false;
            this.btnTSFolder.Click += new System.EventHandler(this.btnTSFolder_Click);
            this.btnTSFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.targetSearch_DragDrop);
            this.btnTSFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // tbxTargetSearch
            // 
            this.tbxTargetSearch.AllowDrop = true;
            this.tbxTargetSearch.Location = new System.Drawing.Point(100, 26);
            this.tbxTargetSearch.Name = "tbxTargetSearch";
            this.tbxTargetSearch.Size = new System.Drawing.Size(310, 20);
            this.tbxTargetSearch.TabIndex = 21;
            this.tbxTargetSearch.TextChanged += new System.EventHandler(this.tbxTargetSearch_TextChanged);
            this.tbxTargetSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.targetSearch_DragDrop);
            this.tbxTargetSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // tbxDecoySearch
            // 
            this.tbxDecoySearch.AllowDrop = true;
            this.tbxDecoySearch.Location = new System.Drawing.Point(100, 54);
            this.tbxDecoySearch.Name = "tbxDecoySearch";
            this.tbxDecoySearch.Size = new System.Drawing.Size(310, 20);
            this.tbxDecoySearch.TabIndex = 22;
            this.tbxDecoySearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.decoySearch_DragDrop);
            this.tbxDecoySearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxResultsFolder);
            this.groupBox1.Controls.Add(this.cbxResultsFolder);
            this.groupBox1.Controls.Add(this.tbxDecoySearch);
            this.groupBox1.Controls.Add(this.tbxTargetSearch);
            this.groupBox1.Controls.Add(this.gbxPDVersion);
            this.groupBox1.Controls.Add(this.btnTSFolder);
            this.groupBox1.Controls.Add(this.btnDSFolder);
            this.groupBox1.Controls.Add(this.lblTargetSearch);
            this.groupBox1.Controls.Add(this.lblDecoySearch);
            this.groupBox1.Location = new System.Drawing.Point(21, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(881, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select folders with SRF or MSF files";
            // 
            // tbxResultsFolder
            // 
            this.tbxResultsFolder.AllowDrop = true;
            this.tbxResultsFolder.Enabled = false;
            this.tbxResultsFolder.Location = new System.Drawing.Point(610, 51);
            this.tbxResultsFolder.Name = "tbxResultsFolder";
            this.tbxResultsFolder.Size = new System.Drawing.Size(265, 20);
            this.tbxResultsFolder.TabIndex = 23;
            this.tbxResultsFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.resultsFolder_DragDrop);
            this.tbxResultsFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // frmMain
            // 
            this.AcceptButton = this.runBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 410);
            this.Controls.Add(this.gbxNterminalModifs);
            this.Controls.Add(this.gbxDeltaMass);
            this.Controls.Add(this.gbxSplit);
            this.Controls.Add(this.gbxpRatioCorrection);
            this.Controls.Add(this.cbxAdvanced);
            this.Controls.Add(this.gbxFDRmethod);
            this.Controls.Add(this.gbxpIbox);
            this.Controls.Add(this.HelpLbl);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbxQuality);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.FDRtxt);
            this.Controls.Add(this.trackBarFDR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "pRatio";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFDR)).EndInit();
            this.gbxQuality.ResumeLayout(false);
            this.gbxQuality.PerformLayout();
            this.gbxpRatioCorrection.ResumeLayout(false);
            this.gbxpRatioCorrection.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gbxpIbox.ResumeLayout(false);
            this.gbxpIbox.PerformLayout();
            this.gbxFDRmethod.ResumeLayout(false);
            this.gbxFDRmethod.PerformLayout();
            this.gbxSplit.ResumeLayout(false);
            this.gbxSplit.PerformLayout();
            this.gbxDeltaMass.ResumeLayout(false);
            this.gbxDeltaMass.PerformLayout();
            this.gbxNterminalModifs.ResumeLayout(false);
            this.gbxNterminalModifs.PerformLayout();
            this.gbxPDVersion.ResumeLayout(false);
            this.gbxPDVersion.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarFDR;
        private System.Windows.Forms.TextBox FDRtxt;
        private System.Windows.Forms.GroupBox gbxQuality;
        private System.Windows.Forms.RadioButton quality2;
        private System.Windows.Forms.RadioButton qualityAvg;
        private System.Windows.Forms.RadioButton quality4;
        private System.Windows.Forms.RadioButton quality3;
        private System.Windows.Forms.GroupBox gbxpRatioCorrection;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblParams;
        private System.Windows.Forms.CheckBox chbPepXML;
        private System.Windows.Forms.ToolStripStatusLabel tsWhat;
        private System.Windows.Forms.Label HelpLbl;
        private System.Windows.Forms.CheckBox chbPratioCorrection;
        private System.Windows.Forms.Label lnkParams;
        private System.Windows.Forms.Button btnLinkParams;
        private System.Windows.Forms.GroupBox gbxpIbox;
        private System.Windows.Forms.Label FDRpIcutOffLbl;
        private System.Windows.Forms.TextBox FDRpIcutoffTxt;
        private System.Windows.Forms.CheckBox pIcheck;
        private System.Windows.Forms.RadioButton rbnMixed;
        private System.Windows.Forms.RadioButton rbnSeparated;
        private System.Windows.Forms.RadioButton rbnConcatenated;
        private System.Windows.Forms.GroupBox gbxFDRmethod;
        private System.Windows.Forms.CheckBox cbxAdvanced;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox cbxSplit;
        private System.Windows.Forms.TextBox tbxSplit;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.GroupBox gbxSplit;
        private System.Windows.Forms.GroupBox gbxDeltaMass;
        private System.Windows.Forms.CheckBox cbxDeltaMass;
        private System.Windows.Forms.Label lblDeltaMass;
        private System.Windows.Forms.TextBox tbxDeltaMass;
        private System.Windows.Forms.Label lblNtermStaticModif;
        private System.Windows.Forms.TextBox tbxNtermStaticModif;
        private System.Windows.Forms.GroupBox gbxNterminalModifs;
        private System.Windows.Forms.RadioButton rbnDMAreaFive;
        private System.Windows.Forms.RadioButton rbnDMAreaThree;
        private System.Windows.Forms.RadioButton rbnDMAreaOne;
        private System.Windows.Forms.CheckBox cbxResultsFolder;
        private System.Windows.Forms.Label lblDecoySearch;
        private System.Windows.Forms.Label lblTargetSearch;
        private System.Windows.Forms.Button btnDSFolder;
        private System.Windows.Forms.GroupBox gbxPDVersion;
        private System.Windows.Forms.RadioButton rbnPD20;
        private System.Windows.Forms.RadioButton rbnPD14;
        private System.Windows.Forms.Button btnTSFolder;
        private System.Windows.Forms.TextBox tbxTargetSearch;
        private System.Windows.Forms.TextBox tbxDecoySearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxResultsFolder;
        private System.Windows.Forms.RadioButton rbnUseXCorr;
    }
}

