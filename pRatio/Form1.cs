using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace pRatio
{
    /*
     * PENDING:
     * 1) SpRank, is it important?
     */

    public partial class frmMain : Form
    {
        private CommandLineParser options;
        private DateTime t0;
        public frmMain()
        {
            InitializeComponent();
            cbxAdvanced_CheckedChanged(null, null);

            this.Text += " " + pRatio.Properties.Settings.Default.version;
                       
            this.trackBarFDR.ValueChanged += new EventHandler(trackBarFDR_ValueChanged);

            trackBarFDR.Value = 5;

        }

        public frmMain(bool noCLI)
        {
            InitializeComponent();
            this.t0 = new DateTime();
            this.options = new CommandLineParser();
            this.setParamatersFromCLI();
            
            object sender = null;
            EventArgs e = new EventArgs();
            this.runBtn_Click( sender,e);
        }

        private void setParamatersFromCLI()
        {
            // target & decoy folders
            tbxTargetSearch.Text = (string) this.options.getParameterByName("TargetSearch");
            tbxDecoySearch.Text = (string) this.options.getParameterByName("DecoySearch");

            //use Isoelectric point
            if ( this.options.isDefined("usePI") ) {
                pIcheck.Checked = true;
                if ( this.options.isDefined("FDRPI") ) {
                    double fdrTmp = (double) this.options.getParameterByName("FDRpI");

                    FDRpIcutoffTxt.Text = (string) fdrTmp.ToString();
                }
            }        
            //use pepXML file with params from SEQUEST
            if ( this.options.isDefined("pepXML") ){
                chbPepXML.Checked = true;
                if ( this.options.isDefined("SEQUESTparamsFile") ) {
                    lnkParams.Text = (string) this.options.getParameterByName("SEQUESTparamsFile");
                }
            }

            // FDR Method
            if ( this.options.isDefined("FDRMethod") ) {
                string fdrMethod = (string) this.options.getParameterByName("FDRMethod");
                switch ( fdrMethod ) {
                    case "mixed": 
                        rbnMixed.Checked = true;
                        break;
                    case "concatenated":
                        rbnConcatenated.Checked = true;
                        break;
                    case "separated":
                        rbnSeparated.Checked = true;
                        break;
                }
            }
            // FDR Cut-off
            if ( this.options.isDefined("FDRCutoff") )
            {
                FDRtxt.Text = this.options.getParameterByName("FDRCutoff").ToString();
            }

        }

       
        void trackBarFDR_ValueChanged(object sender, EventArgs e)
        {
            float FDRval=(float)trackBarFDR.Value/100;
            FDRtxt.Text = FDRval.ToString();
        }

        private void btnTSFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selBrowserDlg = new FolderBrowserDialog();

            bool containsFiles = checkFilesInFolder(selBrowserDlg);

            if (selBrowserDlg.SelectedPath != "") //&& hasSrfs)
            {
                this.tbxTargetSearch.Text = selBrowserDlg.SelectedPath;
            }
            else
            {
                this.tbxTargetSearch.Text = "folder not selected";
            }

            if (!containsFiles)
            {
                MessageBox.Show("Error: the selected folder does not contain any srf or msf files.");
                this.tbxTargetSearch.Text = "folder not selected";
            }
        }

       private void btnDSFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selBrowserDlg = new FolderBrowserDialog();

            bool containsFiles = checkFilesInFolder(selBrowserDlg);

            if (selBrowserDlg.SelectedPath != "") //&& hasSrfs)
            {
                this.tbxDecoySearch.Text = selBrowserDlg.SelectedPath;
            }
            else
            {
                this.tbxDecoySearch.Text = "folder not selected";
            }

            if (!containsFiles)
            {
                MessageBox.Show("Error: the selected folder does not contain any srf or msf files.");
                this.tbxDecoySearch.Text = "folder not selected";
            }

        }

       private static bool checkFilesInFolder(FolderBrowserDialog selBrowserDlg)
       {
           selBrowserDlg.ShowNewFolderButton = false;
           selBrowserDlg.ShowDialog();


           bool containsFiles = false;
           if (Directory.Exists(selBrowserDlg.SelectedPath))
           {
               string[] files = Directory.GetFiles(selBrowserDlg.SelectedPath);

               foreach (string fil in files)
               {
                   if (fil.ToLower().EndsWith(".srf") || fil.ToLower().EndsWith(".msf"))
                   {
                       containsFiles = true;
                       break;
                   }
               }
           }
           return containsFiles;
       }

        private void HelpLbl_Click(object sender, EventArgs e)
        {

            // Create form to be owned.
            frmHelp help = new frmHelp();
            // Set the text of the owned form.
            //ownedForm.Text = "Owned Form " + this.OwnedForms.Length;
            string helpTitle = "Help of pRatio " + pRatio.Properties.Settings.Default.version;
            help.Text = helpTitle;


            bool helpIsShown = false;

            for (int i = 0; i <= this.OwnedForms.GetUpperBound(0); i++)
            {
                if (this.OwnedForms[i].Text == helpTitle)
                {
                    helpIsShown = true;
                }
            }


            if (!helpIsShown)
            {
                // Add the form to the array of owned forms.
                this.AddOwnedForm(help);

                help.Show();

            }


        }

        
        
        private void btnLinkParams_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "params files (*.params)|*.params|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            ofd.Title = "Select a SEQUEST parameters file";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                this.lnkParams.Text = ofd.FileName;
            }


        }

        private void chbPepXML_CheckedChanged(object sender, EventArgs e)
        {
            //Size gbSize = new Size();
            //gbSize = this.groupBox4.Size;

            //if (this.chbPepXML.Checked)
            //{
            //    gbSize.Height = 70;
            //}
            //else
            //{
            //    gbSize.Height = 43;
            //}

            //this.groupBox4.Size = gbSize;

            this.btnLinkParams.Visible = this.chbPepXML.Checked;
            this.lnkParams.Visible = this.chbPepXML.Checked;
            this.lblParams.Visible = this.chbPepXML.Checked;
        }



        private void runBtn_Click(object sender, EventArgs e)
        {
            this.statusStrip1.Visible = true;
            this.tStripProgressBar.Value = 0;
            this.tsWhat.Text = "checking";
            this.tsWhat.ToolTipText = "checking";

            float pIWidth = 0;
            double FDRpIcutoff = 0.05;
            int numOfIdsAtFDRpI = 0;

            int errorNumber = 0;
            // specially created for corrupt or wrong msf files
            // in binary: XY,
            //  where Y=0 means no error, Y=1 means error found
            //  and X=0 means continue anyway, X=1 abort pRatio

            FDRmethod FDRmeth = FDRmethod.mixed; // default value

            if (rbnMixed.Checked) FDRmeth = FDRmethod.mixed;
            if (rbnConcatenated.Checked) FDRmeth = FDRmethod.concatenated;
            if (rbnSeparated.Checked) FDRmeth = FDRmethod.separated;

            #region preview checks
            if (!Directory.Exists(tbxTargetSearch.Text))
            {
                string message = "Target search SRF folder not properly specified";
                errorExit(message);
                return;
            }
            if (!Directory.Exists(tbxDecoySearch.Text))
            {
                string message = "Decoy search SRF folder not properly specified";
                errorExit(message);
                return;
            }
            if (!Directory.Exists(tbxResultsFolder.Text))
            {
                string message = "Results folder not properly specified";
                errorExit(message);
                return;
            }

            if (this.chbPepXML.Checked && !File.Exists(lnkParams.Text))
            {
                string message = "You have not selected a params file!";
                errorExit(message);
                return;
            }
       
            double FDRcutOff;
            bool fdrOk = double.TryParse(FDRtxt.Text.Trim(), out FDRcutOff);
            if (!fdrOk)
            {
                string message = "FDR-cutoff not properly specified (not a number)";
                errorExit(message);
                return;
            }
            else 
            {
                if (FDRcutOff < 0 || FDRcutOff > 10)
                {
                    string message = "FDR-cutoff not properly specified  ( 0 <= FDR-cutoff <= 1 )";
                    errorExit(message);
                    return; 
                }
            }

            FDRpIcutoff = FDRcutOff;

            if (pIcheck.Checked && FDRpIcutoffTxt.Text.Trim()!="")
            {
                bool tryFDRpIcutoff = double.TryParse(FDRpIcutoffTxt.Text.Trim(), out FDRpIcutoff);
                if (!tryFDRpIcutoff)
                {
                    string message ="FDR cut-off for pI probability is not a number!";
                    errorExit(message);
                    return;
                }
                else 
                {
                    if (FDRpIcutoff < 0 || FDRpIcutoff > 1)
                    {
                        string message = "FDR cut-off for pI probability is not a correct value!";
                        errorExit(message);
                        return;
                    }
                }
            }

            int totBatches = 1;
            if (cbxAdvanced.Checked && cbxSplit.Checked)
            {
                try
                {
                    totBatches = int.Parse(tbxSplit.Text.Trim());
                }
                catch
                {
                    string message = "Problem getting the number of batches.";
                    errorExit(message);
                    return;
                }
            }

            double deltaMassThreshold = 0; // zero means no threshold
            int deltaMassAreas = 1; // other options: 3 or 5
            double NtermStaticModification = 0; // for example for iTRAQ
            DA_fileReader.PDVersion PDVersionUsed = DA_fileReader.PDVersion.PD20;

            if (rbnPD14.Checked) PDVersionUsed = DA_fileReader.PDVersion.PD14AndPrevious;
            if (rbnPD20.Checked) PDVersionUsed = DA_fileReader.PDVersion.PD20;

            if (cbxAdvanced.Checked)
            {
                // static modification in the Nterminal
                // (or, actually, anywhere, but in every peptide)
                try
                {
                    if (tbxNtermStaticModif.Text.Length > 0)
                        NtermStaticModification = double.Parse(tbxNtermStaticModif.Text.Trim());
                }
                catch
                {
                    string message = "Problem getting the Nterm static modification.";
                    errorExit(message);
                    return;
                }

                if (cbxDeltaMass.Checked)
                {
                    try
                    {
                        deltaMassThreshold = double.Parse(tbxDeltaMass.Text.Trim());
                    }
                    catch
                    {
                        string message = "Problem getting the delta mass threshold.";
                        errorExit(message);
                        return;
                    }

                    if (rbnDMAreaOne.Checked) deltaMassAreas = 1;
                    if (rbnDMAreaThree.Checked) deltaMassAreas = 3;
                    if (rbnDMAreaFive.Checked) deltaMassAreas = 5;

                }
            }

            #endregion

            #region quality selection
            DA_fileReader.Quality qual = new DA_fileReader.Quality();
            bool usepRatio = true;

            if (this.quality2.Checked)
            {
                qual.whichQuality = DA_fileReader.Quality.QualityType.Xc2;
            }
            if (this.quality3.Checked)
            {
                qual.whichQuality = DA_fileReader.Quality.QualityType.Xc3;
            }
            if (this.quality4.Checked)
            {
                qual.whichQuality = DA_fileReader.Quality.QualityType.Xc4;
            }
            if (this.qualityAvg.Checked)
            {
                qual.whichQuality = DA_fileReader.Quality.QualityType.XcAveraged;
            }
            if (this.rbnUseXCorr.Checked)
            {
                usepRatio = false;
                qual.whichQuality = DA_fileReader.Quality.QualityType.FirstOnly;
            }

            #endregion

            #region XCorr selection
            DA_fileReader.XcorrSelection XCselected=new DA_fileReader.XcorrSelection();
            if(this.chbPratioCorrection.Checked)
            {
                XCselected.whichXcorr=DA_fileReader.XcorrSelection.XCorrType.normalized;
            }
            else
            {
                XCselected.whichXcorr=DA_fileReader.XcorrSelection.XCorrType.regular;
            }
            #endregion


            for (int currentBatch = 0; currentBatch < totBatches; currentBatch++)
            {
                //Read the SRFs of the Decoy and the Target searches and organize them in two arraylists.
                #region Read the SRFS, XMLs, or txt data of the decoy and target searches
                GC.Collect();
                ArrayList targetFiles = new ArrayList();
                ArrayList decoyFiles = new ArrayList();
                DA_fileReader.fileType filetype = DA_fileReader.fileType.SRF;

                string[] fileTypes = { ".srf", ".msf" };

                foreach (string extension in fileTypes)
                {
                    targetFiles.AddRange(Directory.GetFiles(tbxTargetSearch.Text, "*" + extension, System.IO.SearchOption.AllDirectories));
                    decoyFiles.AddRange(Directory.GetFiles(tbxDecoySearch.Text, "*" + extension, System.IO.SearchOption.AllDirectories));
                }

                ArrayList splitTargetFiles = splitByFileType(targetFiles, fileTypes);
                ArrayList splitDecoyFiles = splitByFileType(decoyFiles, fileTypes);

                // warning, this is sensible to the position of srf and msf within fileTypes[]

                string[] srfTargetFiles = new string[0];
                string[] msfTargetFiles = new string[0];
                string[] srfDecoyFiles = new string[0];
                string[] msfDecoyFiles = new string[0];

                if (((ArrayList)splitTargetFiles[0]).Count > 0) srfTargetFiles = (string[])((ArrayList)splitTargetFiles[0]).ToArray(typeof(string));
                if (((ArrayList)splitTargetFiles[1]).Count > 0) msfTargetFiles = (string[])((ArrayList)splitTargetFiles[1]).ToArray(typeof(string));
                if (((ArrayList)splitDecoyFiles[0]).Count > 0) srfDecoyFiles = (string[])((ArrayList)splitDecoyFiles[0]).ToArray(typeof(string));
                if (((ArrayList)splitDecoyFiles[1]).Count > 0) msfDecoyFiles = (string[])((ArrayList)splitDecoyFiles[1]).ToArray(typeof(string));

                if (targetFiles.Count == 0)
                {
                    targetFiles.AddRange(Directory.GetFiles(tbxTargetSearch.Text, "*.xml", System.IO.SearchOption.AllDirectories));
                    decoyFiles.AddRange(Directory.GetFiles(tbxDecoySearch.Text, "*.xml", System.IO.SearchOption.AllDirectories));

                    for (int i = 0; i < targetFiles.Count; i++)
                    {
                        if (targetFiles[i].ToString().Contains("_QuiXML"))
                        {
                            string message = "Apparently a quiXML file is in the target folder. Please remove it to perform the pRatio calculation";
                            errorExit(message);
                            return;
                        }
                    }

                    for (int i = 0; i < decoyFiles.Count; i++)
                    {
                        if (decoyFiles[i].ToString().Contains("_QuiXML"))
                        {
                            string message = "Apparently a quiXML file is in the decoy folder. Please remove it to perform the pRatio calculation";
                            errorExit(message);
                            return;
                        }
                    }

                    filetype = DA_fileReader.fileType.XML;
                }


                ArrayList OutsTarget = new ArrayList();
                ArrayList OutsDecoyPre = new ArrayList();
                DA_paramsReader.paramsData SearchParams;
                DA_fileReader.SrfData srfHeaderTarget = new DA_fileReader.SrfData();
                DA_fileReader.SrfData srfHeaderDecoy = new DA_fileReader.SrfData();



                this.tsWhat.Text = "reading data";
                this.tsWhat.ToolTipText = "reading data";

                string modificationsFilePath = "";

                if (msfTargetFiles.Length > 0)
                {
                    string modificationsFileName = "modifications.xml";
                    string msfPath = Path.GetDirectoryName(msfTargetFiles[0]);
                    string originalModificationsFile = Path.Combine(msfPath, modificationsFileName);
                    string finalModificationsFile = Path.Combine(this.tbxResultsFolder.Text, modificationsFileName);

                    if (originalModificationsFile == finalModificationsFile)
                        modificationsFilePath = originalModificationsFile;
                    else
                    {
                        // copies the modifications to the results folder,
                        // and uses that one only, so the original one is not touched

                        if (File.Exists(originalModificationsFile))
                            File.Copy(originalModificationsFile, finalModificationsFile, true);

                        modificationsFilePath = finalModificationsFile;
                    }
                }
                string errorMessage = "";
                if (msfTargetFiles.Length >= 1)
                {
                    ICollection cTarget = (ICollection)DA_fileReader.readOuts(msfTargetFiles,
                                                                qual,
                                                                XCselected,
                                                                OutData.databases.Target,
                                                                DA_fileReader.fileType.MSF,
                                                                modificationsFilePath,
                                                                this.tStripProgressBar,
                                                                this.tsWhat,
                                                                totBatches,
                                                                currentBatch,
                                                                ref errorNumber,
                                                                NtermStaticModification,
                                                                PDVersionUsed,
                                                                ref errorMessage);
                    if (errorMessage.Length > 0)
                    {
                        errorExit(errorMessage);
                        return;
                    }
                    OutsTarget.AddRange(cTarget);

                    string binErrorNumber = Convert.ToString(errorNumber, 2);
                    if (binErrorNumber.Length >= 2)
                        if (binErrorNumber.Substring(binErrorNumber.Length - 2, 1) == "1")
                        {
                            this.tStripProgressBar.Value = 0;
                            this.tsWhat.Text = "Error found, pRatio aborted.";
                            return;
                        }

                    ICollection cDecoy = (ICollection)DA_fileReader.readOuts(msfDecoyFiles,
                                                                qual,
                                                                XCselected,
                                                                OutData.databases.Decoy,
                                                                DA_fileReader.fileType.MSF,
                                                                modificationsFilePath,
                                                                this.tStripProgressBar,
                                                                this.tsWhat,
                                                                totBatches,
                                                                currentBatch,
                                                                ref errorNumber,
                                                                NtermStaticModification,
                                                                PDVersionUsed,
                                                                ref errorMessage);
                    if (errorMessage.Length > 0)
                    {
                        errorExit(errorMessage);
                        return;
                    }
                    OutsDecoyPre.AddRange(cDecoy);

                    binErrorNumber = Convert.ToString(errorNumber, 2);
                    if (binErrorNumber.Length >= 2)
                        if (binErrorNumber.Substring(binErrorNumber.Length - 2, 1) == "1")
                        {
                            this.tStripProgressBar.Value = 0;
                            this.tsWhat.Text = "Error found, pRatio aborted.";
                            return;
                        }
                }

                if (srfTargetFiles.Length >= 1)
                {
                    // not added option for deltaMass filtering
                    switch (filetype)
                    {
                        //case DA_fileReader.fileType.SRF:
                        //    srfHeaderTarget = DA_fileReader.readCommonDataSRF(srfTargetFiles[0], OutData.databases.Target);
                        //    srfHeaderDecoy = DA_fileReader.readCommonDataSRF(srfTargetFiles[0], OutData.databases.Decoy);

                        //    //Check wether the both searches were done with the same parameters
                        //    if (srfHeaderDecoy.CompareTo(srfHeaderTarget, DA_fileReader.SrfData.SrfsComparer.ComparisonType.total) != 0)
                        //    {
                        //        string message = "It seems that target and decoy searches were done with different parameters";
                        //        errorExit(message);
                        //        return;
                        //    }
                        //    break;
                        case DA_fileReader.fileType.XML:
                            break;
                    }

                    ICollection srfTarget = (ICollection)DA_fileReader.readOuts(srfTargetFiles,
                                                                qual,
                                                                XCselected,
                                                                OutData.databases.Target,
                                                                filetype,
                                                                modificationsFilePath,
                                                                this.tStripProgressBar,
                                                                this.tsWhat,
                                                                totBatches,
                                                                currentBatch,
                                                                ref errorNumber,
                                                                NtermStaticModification,
                                                                PDVersionUsed,
                                                                ref errorMessage);
                    if (errorMessage.Length > 0)
                    {
                        errorExit(errorMessage);
                        return;
                    }
                    OutsTarget.AddRange(srfTarget);

                    string binErrorNumber = Convert.ToString(errorNumber, 2);
                    if (binErrorNumber.Length >= 2)
                        if (binErrorNumber.Substring(binErrorNumber.Length - 2, 1) == "1")
                        {
                            this.tStripProgressBar.Value = 0;
                            this.tsWhat.Text = "Error found, pRatio aborted.";
                            return;
                        }

                    //this.tStripProgressBar.Value = 50;

                    ICollection srfDecoy = (ICollection)DA_fileReader.readOuts(srfDecoyFiles,
                                                                qual,
                                                                XCselected,
                                                                OutData.databases.Decoy,
                                                                filetype,
                                                                modificationsFilePath,
                                                                this.tStripProgressBar,
                                                                this.tsWhat,
                                                                totBatches,
                                                                currentBatch,
                                                                ref errorNumber,
                                                                NtermStaticModification,
                                                                PDVersionUsed,
                                                                ref errorMessage);
                    if (errorMessage.Length > 0)
                    {
                        errorExit(errorMessage);
                        return;
                    }
                    OutsDecoyPre.AddRange(srfDecoy);

                    binErrorNumber = Convert.ToString(errorNumber, 2);
                    if (binErrorNumber.Length >= 2)
                        if (binErrorNumber.Substring(binErrorNumber.Length - 2, 1) == "1")
                        {
                            this.tStripProgressBar.Value = 0;
                            this.tsWhat.Text = "Error found, pRatio aborted.";
                            return;
                        }

                    //this.tStripProgressBar.Value = 100;                

                }

                if (targetFiles.Count == 0)
                {
                    targetFiles.AddRange(Directory.GetFiles(tbxTargetSearch.Text, "*.txt", System.IO.SearchOption.AllDirectories));
                    OutsTarget.AddRange(DA_txtReader.readTXT((string[])targetFiles.ToArray(), OutData.databases.Target));
                    //this.tStripProgressBar.Value = 50;
                    decoyFiles.AddRange(Directory.GetFiles(tbxDecoySearch.Text, "*.txt", System.IO.SearchOption.AllDirectories));
                    OutsDecoyPre.AddRange(DA_txtReader.readTXT((string[])decoyFiles.ToArray(), OutData.databases.Decoy));
                    //this.tStripProgressBar.Value = 100;
                }

                #endregion


                #region declare comparers
                //Declare the diverse comparison modes of the OutData structure
                OutData.OutsComparer cmpSrf = new OutData.OutsComparer();
                cmpSrf.WhichComparison = OutData.OutsComparer.ComparisonType.RAWfile;
                OutData.OutsComparer cmpKey = new OutData.OutsComparer();
                cmpKey.WhichComparison = OutData.OutsComparer.ComparisonType.key;
                OutData.OutsComparer cmpXc1 = new OutData.OutsComparer();
                cmpXc1.WhichComparison = OutData.OutsComparer.ComparisonType.Xcorr1;
                OutData.OutsComparer cmpRandomvsInv = new OutData.OutsComparer();
                cmpRandomvsInv.WhichComparison = OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorr1;
                OutData.OutsComparer cmppRatio = new OutData.OutsComparer();
                cmppRatio.WhichComparison = OutData.OutsComparer.ComparisonType.pRatio;
                OutData.OutsComparer cmpFDR = new OutData.OutsComparer();
                cmpFDR.WhichComparison = OutData.OutsComparer.ComparisonType.FDR;

                OutData.OutsComparer cmpXcR = new OutData.OutsComparer();
                cmpXcR.WhichComparison = OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorrRandom;
                #endregion

                //Sort the arraylists by the key (RAWFileName, FirstScan, Charge)
                #region sorting arraylists
                //this.tStripProgressBar.Value = 0;
                this.tsWhat.Text = "sorting";
                this.tsWhat.ToolTipText = "sorting";

                ArrayList outsToDiscard = new ArrayList();
                ArrayList OutsDecoy = new ArrayList();


                OutsTarget.Sort(cmpSrf);
                //this.tStripProgressBar.Value = 50;
                OutsDecoyPre.Sort(cmpKey);
                //this.tStripProgressBar.Value = 100;
                #endregion


                #region calculating pRatios and discarding bad data

                OutData currOut1 = (OutData)OutsTarget[0];

                //Extract the first raw of the decoy outs
                ArrayList currRawDecoy = new ArrayList();
                currRawDecoy = Utilities.extract(OutsDecoyPre, cmpSrf, currOut1, true);
                currRawDecoy.Sort(cmpKey);


                OutData lastOutDir = currOut1;

                //Need an independent ArrayList for the inverted, in order to make the rankings
                ArrayList decoyRanking = (ArrayList)OutsDecoyPre.Clone();
                //That way points to the same array of pointers (Danger!!): 
                //ArrayList decoyRanking = (ArrayList)OutsDecoy.GetRange(0, OutsDecoy.Count - 1);
                decoyRanking.Sort(cmpXc1); //if you need the Hn distribution: decoyRanking.Sort(cmpXcR); 
                decoyRanking.Reverse();

                currRawDecoy = (ArrayList)Utilities.extract(OutsDecoyPre, cmpSrf, currOut1, true).Clone();
                currRawDecoy.Sort(cmpKey);

                //this.tStripProgressBar.Value = 0;
                this.tsWhat.Text = "calculating probability ratios";
                this.tsWhat.ToolTipText = "calculating probability ratios";

                for (int n = 0; n < OutsTarget.Count; n++)
                {

                    //this.tStripProgressBar.Value = (int)(100*n/OutsTarget.Count);

                    OutData sdTarget = (OutData)OutsTarget[n];

                    //If != Raw ==> extract the data of the decoy search arraylist for the corresponding raw
                    int comp = sdTarget.CompareTo(lastOutDir, OutData.OutsComparer.ComparisonType.RAWfile);
                    if (comp != 0)
                    {
                        currRawDecoy = (ArrayList)Utilities.extract(OutsDecoyPre, cmpSrf, sdTarget, true).Clone();
                        lastOutDir = sdTarget;
                        currRawDecoy.Sort(cmpKey);
                    }

                    int search = currRawDecoy.BinarySearch(sdTarget, cmpKey);
                    if (search >= 0)
                    {
                        OutData sdDecoy = new OutData(OutData.databases.Decoy);
                        sdDecoy = (OutData)currRawDecoy[search];

                        sdTarget.rnkXc1 = Utilities.ranking(decoyRanking, cmpXc1, sdTarget, true);
                        sdTarget.rnkXcRandom = Utilities.ranking(decoyRanking, cmpRandomvsInv, sdTarget, true);  //if you need the Hn distribution: sdTarget.rnkXcRandom = Utilities.ranking(decoyRanking, cmpXcR, sdTarget, true); 
                        sdTarget.pRatioTarget = sdTarget.rnkXc1 / Math.Abs(sdTarget.rnkXcRandom);
                        sdTarget.pRatio = sdTarget.pRatioTarget;

                        sdDecoy.rnkXc1 = Utilities.ranking(decoyRanking, cmpXc1, sdDecoy, true);
                        sdDecoy.rnkXcRandom = Utilities.ranking(decoyRanking, cmpRandomvsInv, sdDecoy, true); //if you need the Hn distribution:   sdDecoy.rnkXcRandom = Utilities.ranking(decoyRanking, cmpXcR, sdDecoy, true);
                        sdDecoy.pRatioDecoy = sdDecoy.rnkXc1 / Math.Abs(sdDecoy.rnkXcRandom);
                        sdDecoy.pRatio = sdDecoy.pRatioDecoy;

                        sdTarget.pRatioDecoy = sdDecoy.pRatioDecoy;
                        sdDecoy.pRatioTarget = sdTarget.pRatioTarget;

                        sdTarget.Xcorr1Target = sdTarget.Xcorr1;
                        sdDecoy.Xcorr1Decoy = sdDecoy.Xcorr1;

                        sdTarget.XCorr1SearchOther = sdDecoy.Xcorr1Search;
                        sdDecoy.XCorr1SearchOther = sdTarget.Xcorr1Search;

                        //Adding also pITarget and pIDecoy
                        sdTarget.pITarget = sdTarget.pI;
                        sdTarget.pIDecoy = sdDecoy.pI;
                        sdDecoy.pITarget = sdTarget.pI;
                        sdDecoy.pIDecoy = sdDecoy.pI;

                        OutsTarget[n] = sdTarget;

                        OutsDecoy.Add((OutData)sdDecoy);
                    }
                    else  //The scan is not found at the decoy search ==> discard it!
                        outsToDiscard.Add(sdTarget);
                }

                // filter with deltaMass if it is required
                if (deltaMassThreshold > 0)
                {
                    for (int n = 0; n < OutsTarget.Count; n++)
                    {
                        OutData sdTarget = (OutData)OutsTarget[n];

                        double deltaMassTargetppm = Math.Abs(sdTarget.TheoreticalMass - sdTarget.PrecursorMass)
                            / sdTarget.PrecursorMass * 1e6;

                        double peakDistanceppm = (pRatio.Properties.Settings.Default.C13Mass - 12.0)
                            / sdTarget.PrecursorMass * 1e6;
                        // the algorithm is checking here n peaks, where n = deltaMassAreas 
                        if (deltaMassTargetppm >= deltaMassThreshold)
                        {
                            if (deltaMassAreas <= 1)
                            {
                                sdTarget.pRatio = 2;
                                sdTarget.pRatioTarget = 2;
                                sdTarget.Xcorr1 = float.Epsilon;
                                sdTarget.Xcorr1Target = sdTarget.Xcorr1;
                            }
                            else
                            {
                                deltaMassTargetppm = Math.Abs(deltaMassTargetppm - peakDistanceppm);

                                if (deltaMassTargetppm >= deltaMassThreshold)
                                {
                                    if (deltaMassAreas > 1 && deltaMassAreas <= 3)
                                    {
                                        sdTarget.pRatio = 2;
                                        sdTarget.pRatioTarget = 2;
                                        sdTarget.Xcorr1 = float.Epsilon;
                                        sdTarget.Xcorr1Target = sdTarget.Xcorr1;
                                    }
                                    else
                                    {
                                        deltaMassTargetppm = Math.Abs(deltaMassTargetppm - peakDistanceppm);

                                        if (deltaMassTargetppm >= deltaMassThreshold)
                                        {
                                            sdTarget.pRatio = 2;
                                            sdTarget.pRatioTarget = 2;
                                            sdTarget.Xcorr1 = float.Epsilon;
                                            sdTarget.Xcorr1Target = sdTarget.Xcorr1;
                                        }
                                    }
                                }
                            }
                        }

                    }

                    for (int n = 0; n < OutsDecoy.Count; n++)
                    {
                        OutData sdDecoy = (OutData)OutsDecoy[n];

                        double deltaMassDecoyppm = Math.Abs(sdDecoy.TheoreticalMass - sdDecoy.PrecursorMass)
                            / sdDecoy.PrecursorMass * 1e6;

                        double peakDistanceppm = (pRatio.Properties.Settings.Default.C13Mass - 12.0)
                            / sdDecoy.PrecursorMass * 1e6;

                        if (deltaMassDecoyppm > deltaMassThreshold)
                        {
                            if (deltaMassAreas <= 1)
                            {
                                sdDecoy.pRatio = 2;
                                sdDecoy.pRatioDecoy = 2;
                                sdDecoy.Xcorr1 = float.Epsilon;
                                sdDecoy.Xcorr1Decoy = sdDecoy.Xcorr1;
                            }
                            else
                            {
                                deltaMassDecoyppm = Math.Abs(deltaMassDecoyppm - peakDistanceppm);

                                if (deltaMassDecoyppm >= deltaMassThreshold)
                                {
                                    if (deltaMassAreas > 1 && deltaMassAreas <= 3)
                                    {
                                        sdDecoy.pRatio = 2;
                                        sdDecoy.pRatioDecoy = 2;
                                        sdDecoy.Xcorr1 = float.Epsilon;
                                        sdDecoy.Xcorr1Decoy = sdDecoy.Xcorr1;
                                    }
                                    else
                                    {
                                        deltaMassDecoyppm = Math.Abs(deltaMassDecoyppm - peakDistanceppm);

                                        if (deltaMassDecoyppm >= deltaMassThreshold)
                                        {
                                            sdDecoy.pRatio = 2;
                                            sdDecoy.pRatioDecoy = 2;
                                            sdDecoy.Xcorr1 = float.Epsilon * 2;
                                            sdDecoy.Xcorr1Decoy = sdDecoy.Xcorr1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                OutsTarget.Sort(cmpKey);
                OutsDecoy.Sort(cmpKey);

                //Discard the outs not found at the decoy search arraylist
                for (int n = 0; n < outsToDiscard.Count; n++)
                {
                    OutData co = (OutData)outsToDiscard[n];

                    int idxDiscard = OutsTarget.BinarySearch(co, cmpKey);
                    if (idxDiscard >= 0)
                    {
                        OutsTarget.RemoveAt(idxDiscard);
                    }
                }

                for (int n = 0; n < OutsTarget.Count; n++)
                {
                    OutData sdTarget = (OutData)OutsTarget[n];
                    int idx = OutsDecoy.BinarySearch(sdTarget, cmpKey);
                    ((OutData)OutsDecoy[idx]).Xcorr1Target = sdTarget.Xcorr1Target;
                }

                for (int n = 0; n < OutsDecoy.Count; n++)
                {
                    OutData sdDecoy = (OutData)OutsDecoy[n];
                    int idx = OutsTarget.BinarySearch(sdDecoy, cmpKey);
                    ((OutData)OutsTarget[idx]).Xcorr1Decoy = sdDecoy.Xcorr1Decoy;
                }

                #endregion

                #region Calculating FDRs (calculation without pI probability)

                // ******** Calculating FDRs ***************

                //this.tStripProgressBar.Value = 0;
                this.tsWhat.Text = "calculating FDRs";
                this.tsWhat.ToolTipText = "calculating FDRs";


                //initialize counters
                int targetBest = 0;
                int decoyBest = 0;
                int decoyOnly = 0;
                int targetOnly = 0;

                int falses = 0;


                //Join the both searches in order to sort them by the pRatio
                ArrayList outsJ = new ArrayList(OutsTarget.Count + OutsDecoyPre.Count);

                // previous, a bit longer code doing the same as the next two lines
                //for (int i = 0; i < OutsTarget.Count; i++)
                //{
                //    OutData sd = (OutData)OutsTarget[i];
                //    outsJ.Add((OutData)sd);
                //}
                //for (int i = 0; i < OutsDecoy.Count; i++)
                //{
                //    OutData sd = (OutData)OutsDecoy[i];
                //    outsJ.Add((OutData)sd);
                //}


                /*
                 * INTERESTING TO RETRIEVE pRATIOS (UNCOMMENT TO SAVE THESE DATA)
                 * 
                string folderPath = Path.GetDirectoryName(targetFiles[0].ToString());
                string dirFile = folderPath + @"\pRatios_dir.txt";
                string invFile = folderPath + @"\pRatios_inv.txt";

                write_pRatio_file(OutsTarget, dirFile);
                write_pRatio_file(OutsDecoy, invFile);
                */

                foreach (OutData sd in OutsTarget) outsJ.Add(sd);
                foreach (OutData sd in OutsDecoy) outsJ.Add(sd);

                // next line is important to avoid target = decoy effect
                ArrayList outsJoined = removeEqualSequences(cmpXc1, outsJ);

                if (usepRatio)
                {
                    //outsJoined = removeEqualSequences(cmpXc1, outsJ);
                    outsJoined.Sort(cmppRatio);
                }
                else // use XCorr
                {
                    //ArrayList outsJoined_pre = removeEqualSequences(cmpXc1, outsJ);
                    //outsJoined_pre.Sort(cmpXc1);

                    //outsJoined = new ArrayList();
                    //for (int i = outsJoined_pre.Count - 1; i >= 0; i--)
                    //    outsJoined.Add(outsJoined_pre[i]);

                    outsJoined.Sort(cmpXc1);
                    outsJoined.Reverse();
                }

                OutData.OutsComparer cmpRnk1 = new OutData.OutsComparer();
                cmpRnk1.WhichComparison = OutData.OutsComparer.ComparisonType.RnkXc1;

                for (int i = 0; i < outsJoined.Count; i++)
                {
                    OutData sd = (OutData)outsJoined[i];

                    double targetParametre = 0;
                    double decoyParametre = 0;

                    if (usepRatio)
                    {
                        targetParametre = sd.pRatioTarget;
                        decoyParametre = sd.pRatioDecoy;
                    }
                    else // XCorr1 is used
                    {
                        targetParametre = -sd.Xcorr1Target;
                        decoyParametre = -sd.Xcorr1Decoy;
                    }

                    switch (sd.dbType)
                    {
                        case OutData.databases.Target:
                            {
                                if (targetParametre <= decoyParametre)
                                    targetOnly++;//targetBestTotal++;
                                else
                                {
                                    // this way, do-- and db++ (do = dbTotal - db)
                                    decoyBest++;
                                    decoyOnly--;
                                }

                                break;
                            }
                        case OutData.databases.Decoy:
                            {
                                falses++;
                                if (decoyParametre <= targetParametre)
                                    decoyOnly++;//decoyBestTotal++;
                                else
                                {
                                    // this way, to-- and tb++ (to = tbTotal - tb)
                                    targetBest++;
                                    targetOnly--;
                                }

                                break;
                            }
                    }

                    //decoyOnly = decoyBestTotal - decoyBest; 
                    //targetOnly = targetBestTotal - targetBest;

                    //decoyOnly = decoyBestTotal - targetBest;
                    //targetOnly = targetBestTotal - decoyBest;

                    switch (FDRmeth)
                    {
                        case FDRmethod.separated:
                            sd.FDR = (double)(decoyOnly + targetBest + decoyBest) / (double)(targetBest + decoyBest + targetOnly);
                            break;
                        case FDRmethod.concatenated:
                            sd.FDR = 2 * (double)(decoyOnly + decoyBest) / (double)(decoyOnly + decoyBest + targetOnly + targetBest);
                            break;
                        case FDRmethod.mixed:
                            sd.FDR = (double)(decoyOnly + 2 * decoyBest) / (double)(targetBest + decoyBest + targetOnly);
                            break;
                    }

                    //sd.FDR = (double)(decoyBest + decoyWorse) / (i+1);
                    //sd.FDR = (double)falses / (double)(i + 1);
                    outsJoined[i] = (OutData)sd;

                }

                #endregion


                #region Calculating FDRs (with pI probability)

                ArrayList pIperFraction = new ArrayList();

                if (this.pIcheck.Checked)
                {
                    //Calculate a median for each raw (pI fraction)
                    OutsTarget.Sort(cmpSrf);

                    currOut1 = (OutData)OutsTarget[0];
                    lastOutDir = currOut1;
                    ArrayList currRaw = new ArrayList();

                    //for the first raw
                    currRaw = (ArrayList)Utilities.extract(OutsTarget, cmpSrf, currOut1, true).Clone();

                    //count spectra with FDR < FDRmax
                    double FDRmax = 0.01;
                    int dimOfSpecFDR = 0;
                    for (int n = 0; n < currRaw.Count; n++)
                    {
                        OutData spec = (OutData)currRaw[n];
                        if (spec.FDR <= FDRmax) dimOfSpecFDR++;
                    }

                    //Array with pI values for mu calculation 
                    float[] pIformu1 = new float[dimOfSpecFDR];
                    int counter = 0;
                    for (int n = 0; n < currRaw.Count; n++)
                    {
                        OutData spec = (OutData)currRaw[n];
                        if (spec.FDR <= FDRmax)
                        {
                            pIformu1[counter] = spec.pI;
                            counter++;
                        }
                    }

                    //calculate the median for the first raw and add to an arraylist of raws and medians
                    float median = Utilities.MathAverages.Median(pIformu1);
                    Utilities.pIfraction pif1 = new Utilities.pIfraction();
                    pif1.RAWFileName = currOut1.RAWFile;
                    pif1.mu = median;
                    pIperFraction.Add(pif1);

                    //calculate the median for the the raws and add to an arraylist of raws and medians
                    for (int n = 0; n < OutsTarget.Count; n++)
                    {

                        OutData sdTarget = (OutData)OutsTarget[n];
                        //If != Raw ==> extract the data of the corresponding raw
                        int comp = sdTarget.CompareTo(lastOutDir, OutData.OutsComparer.ComparisonType.RAWfile);
                        if (comp != 0)
                        {
                            currRaw = (ArrayList)Utilities.extract(OutsTarget, cmpSrf, sdTarget, true).Clone();
                            lastOutDir = sdTarget;

                            //count spectra with FDR < FDRmax
                            dimOfSpecFDR = 0;
                            for (int j = 0; j < currRaw.Count; j++)
                            {
                                OutData spec = (OutData)currRaw[j];
                                if (spec.FDR <= FDRmax) dimOfSpecFDR++;
                            }


                            float[] pIformu = new float[dimOfSpecFDR];
                            counter = 0;
                            for (int j = 0; j < currRaw.Count; j++)
                            {
                                OutData spec = (OutData)currRaw[j];
                                if (spec.FDR <= FDRmax)
                                {
                                    pIformu[counter] = spec.pI;
                                    counter++;
                                }
                            }

                            median = Utilities.MathAverages.Median(pIformu);
                            Utilities.pIfraction pif = new Utilities.pIfraction();
                            pif.RAWFileName = lastOutDir.RAWFile;
                            pif.mu = median;
                            pIperFraction.Add(pif);

                        }

                    }


                    //Assign mu to each spectrum (Decoy and target), 
                    outsJoined.Sort(cmpSrf);
                    string currRawFile = currOut1.RAWFile;
                    float currpImu = 0;
                    for (int n = 0; n < pIperFraction.Count; n++)
                    {
                        Utilities.pIfraction pif = (Utilities.pIfraction)pIperFraction[n];
                        if (currRawFile == pif.RAWFileName)
                        {
                            currpImu = pif.mu;
                        }
                    }
                    for (int n = 0; n < outsJoined.Count; n++)
                    {
                        OutData sd = (OutData)outsJoined[n];
                        //If != Raw ==> extract the data of the corresponding raw
                        int comp = sd.CompareTo(lastOutDir, OutData.OutsComparer.ComparisonType.RAWfile);
                        if (comp != 0)
                        {
                            currRawFile = sd.RAWFile;
                            for (int k = 0; k < pIperFraction.Count; k++)
                            {
                                Utilities.pIfraction pif = (Utilities.pIfraction)pIperFraction[k];
                                if (currRawFile == pif.RAWFileName)
                                {
                                    currpImu = pif.mu;
                                }
                            }
                        }

                        sd.pImu = currpImu;

                        outsJoined[n] = (OutData)sd;
                    }




                    //Sweep for finding the best pI width

                    pIWidth = 0;
                    float deltapI = 1;
                    int maxNumofIds = 0;
                    float pIwidthMax = 0;
                    float pIwidthBest = 5.0F;

                    for (int k = 0; k < 2; k++)
                    {
                        for (int step = -4; step < 4; step++)
                        {
                            pIWidth = pIwidthBest + step * deltapI;

                            //assign a pIprob (in function of the pIWidth)                
                            for (int n = 0; n < outsJoined.Count; n++)
                            {
                                OutData sd = (OutData)outsJoined[n];

                                sd.pIprobDecoy = (1 - sd.pRatioDecoy) * Utilities.fpI(sd.pIDecoy, pIWidth, sd.pImu);
                                sd.pIprobTarget = (1 - sd.pRatioTarget) * Utilities.fpI(sd.pITarget, pIWidth, sd.pImu);

                                switch (sd.dbType)
                                {
                                    case OutData.databases.Decoy:
                                        sd.pIprob = sd.pIprobDecoy;
                                        break;
                                    case OutData.databases.Target:
                                        sd.pIprob = sd.pIprobTarget;
                                        break;
                                }

                                outsJoined[n] = (OutData)sd;

                            }



                            //Sort by the pIprob parameter
                            OutData.OutsComparer cmppIprob = new OutData.OutsComparer();
                            cmppIprob.WhichComparison = OutData.OutsComparer.ComparisonType.pIprob;
                            outsJoined.Sort(cmppIprob);
                            outsJoined.Reverse();

                            //Calculating FDR                
                            //initialize counters
                            targetBest = 0;
                            decoyBest = 0;
                            decoyOnly = 0;
                            targetOnly = 0;
                            falses = 0;
                            for (int i = 0; i < outsJoined.Count; i++)
                            {

                                //this.tStripProgressBar.Value = (int)(100 * i / outsJoined.Count);

                                OutData sd = (OutData)outsJoined[i];


                                switch (sd.dbType)
                                {
                                    case OutData.databases.Target:
                                        {
                                            if (sd.pIprob >= sd.pIprobDecoy)
                                                targetOnly++;
                                            else
                                            {
                                                // this way, do-- and db++ (do = dbTotal - db)
                                                decoyBest++;
                                                decoyOnly--;
                                            }

                                            break;
                                        }
                                    case OutData.databases.Decoy:
                                        {
                                            falses++;
                                            if (sd.pIprob >= sd.pIprobTarget)
                                                decoyOnly++;
                                            else
                                            {
                                                // this way, to-- and tb++ (to = tbTotal - tb)
                                                targetBest++;
                                                targetOnly--;
                                            }

                                            break;
                                        }
                                }


                                // previous code lines

                                //switch (sd.dbType)
                                //{
                                //    case OutData.databases.Target:
                                //        {
                                //            if (sd.pIprob >= sd.pIprobDecoy)
                                //            {
                                //                targetBestTotal++;
                                //            }
                                //            else
                                //            {
                                //                decoyBest++;
                                //            }
                                //            break;
                                //        }
                                //    case OutData.databases.Decoy:
                                //        {
                                //            falses++;
                                //            if (sd.pIprob >= sd.pIprobTarget)
                                //            {
                                //                decoyBestTotal++;
                                //            }
                                //            else
                                //            {
                                //                targetBest++;
                                //            }
                                //            break;
                                //        }
                                //}

                                //decoyOnly = decoyBestTotal - targetBest;
                                //targetOnly = targetBestTotal - decoyBest;

                                switch (FDRmeth)
                                {
                                    case FDRmethod.separated:
                                        sd.FDR = (double)(decoyOnly + targetBest + decoyBest) / (double)(targetBest + decoyBest + targetOnly); //separated
                                        break;
                                    case FDRmethod.concatenated:
                                        sd.FDR = 2 * (double)(decoyOnly + decoyBest) / (double)(decoyOnly + decoyBest + targetOnly + targetBest); //concatenated
                                        break;
                                    case FDRmethod.mixed:
                                        sd.FDR = (double)(decoyOnly + 2 * decoyBest) / (double)(targetBest + decoyBest + targetOnly); //mixed
                                        break;
                                }

                                //sd.FDR = (double)(decoyBest + decoyWorse) / (i+1);
                                //sd.FDR = (double)falses / (double)(i + 1);
                                outsJoined[i] = (OutData)sd;

                            }

                            //Sort by FDR and count up the identifications at the FDRpIcutoff
                            outsJoined.Sort(cmpFDR);

                            numOfIdsAtFDRpI = 0;
                            for (int i = 0; i < outsJoined.Count; i++)
                            {
                                OutData sd = (OutData)outsJoined[i];

                                if (sd.FDR > FDRpIcutoff) break;

                                if (sd.dbType == OutData.databases.Target)
                                {
                                    numOfIdsAtFDRpI++;
                                }
                            }

                            //evaluating number of ids at FDRpI to maximize function
                            if (numOfIdsAtFDRpI > maxNumofIds)
                            {
                                maxNumofIds = numOfIdsAtFDRpI;
                                pIwidthMax = pIWidth;
                            }

                        }

                        pIwidthBest = pIwidthMax;
                        deltapI = deltapI / 10;
                    }

                    //Recalculate FDR for the best option (pIwidthBest)
                    //assign a pIprob (in function of the pIWidth)
                    pIWidth = pIwidthBest;
                    for (int n = 0; n < outsJoined.Count; n++)
                    {
                        OutData sd = (OutData)outsJoined[n];

                        sd.pIprobDecoy = (1 - sd.pRatioDecoy) * Utilities.fpI(sd.pIDecoy, pIWidth, sd.pImu);
                        sd.pIprobTarget = (1 - sd.pRatioTarget) * Utilities.fpI(sd.pITarget, pIWidth, sd.pImu);

                        switch (sd.dbType)
                        {
                            case OutData.databases.Decoy:
                                sd.pIprob = sd.pIprobDecoy;
                                break;
                            case OutData.databases.Target:
                                sd.pIprob = sd.pIprobTarget;
                                break;
                        }

                        outsJoined[n] = (OutData)sd;

                    }



                    //Sort by the pIprob parameter
                    OutData.OutsComparer cmppIprob2 = new OutData.OutsComparer();
                    cmppIprob2.WhichComparison = OutData.OutsComparer.ComparisonType.pIprob;
                    outsJoined.Sort(cmppIprob2);
                    outsJoined.Reverse();

                    //Calculating FDR                
                    //initialize counters
                    //targetBestTotal = 0;
                    targetBest = 0;
                    decoyBest = 0;
                    //decoyBestTotal = 0;
                    decoyOnly = 0;
                    targetOnly = 0;
                    falses = 0;
                    for (int i = 0; i < outsJoined.Count; i++)
                    {

                        //this.tStripProgressBar.Value = (int)(100 * i / outsJoined.Count);

                        OutData sd = (OutData)outsJoined[i];

                        switch (sd.dbType)
                        {
                            case OutData.databases.Target:
                                {
                                    if (sd.pRatio <= sd.pRatioDecoy)
                                        targetOnly++;//targetBestTotal++;
                                    else
                                    {
                                        // this way, do-- and db++ (do = dbTotal - db)
                                        decoyBest++;
                                        decoyOnly--;
                                    }

                                    break;
                                }
                            case OutData.databases.Decoy:
                                {
                                    falses++;
                                    if (sd.pRatio <= sd.pRatioTarget)
                                        decoyOnly++;//decoyBestTotal++;
                                    else
                                    {
                                        // this way, to-- and tb++ (to = tbTotal - tb)
                                        targetBest++;
                                        targetOnly--;
                                    }

                                    break;
                                }
                        }

                        /* OLD WAY
                        switch (sd.dbType)
                        {
                            case OutData.databases.Target:
                                {
                                    if (sd.pIprob >= sd.pIprobDecoy)
                                    {
                                        targetBestTotal++;
                                    }
                                    else
                                    {
                                        decoyBest++;
                                    }
                                    break;
                                }
                            case OutData.databases.Decoy:
                                {
                                    falses++;
                                    if (sd.pIprob >= sd.pIprobTarget)
                                    {
                                        decoyBestTotal++;
                                    }
                                    else
                                    {
                                        targetBest++;
                                    }
                                    break;
                                }
                        }
                        */

                        //decoyOnly = decoyBestTotal - targetBest;
                        //targetOnly = targetBestTotal - decoyBest;

                        switch (FDRmeth)
                        {
                            case FDRmethod.separated:
                                sd.FDR = (double)(decoyOnly + targetBest + decoyBest) / (double)(targetBest + decoyBest + targetOnly);
                                break;
                            case FDRmethod.concatenated:
                                sd.FDR = 2 * (double)(decoyOnly + decoyBest) / (double)(decoyOnly + decoyBest + targetOnly + targetBest);
                                break;
                            case FDRmethod.mixed:
                                sd.FDR = (double)(decoyOnly + 2 * decoyBest) / (double)(targetBest + decoyBest + targetOnly);
                                break;
                        }

                        //sd.FDR = (double)(decoyBest + decoyWorse) / (i+1);
                        //sd.FDR = (double)falses / (double)(i + 1);
                        outsJoined[i] = (OutData)sd;

                    }

                }


                #endregion



                //Write the results
                #region writing results

                //this.tStripProgressBar.Value = 0;
                this.tsWhat.Text = "writing results";
                this.tsWhat.ToolTipText = "writing results";

                outsJoined.Sort(cmpFDR);

                // important, as previously, for some reason, OutsTarget
                // was getting bad information, such as spectra form the decoy database
                OutsTarget = new ArrayList();
                foreach (OutData sd in outsJoined)
                    if (sd.dbType == OutData.databases.Target) OutsTarget.Add(sd);

                OutsTarget.Sort(cmpFDR);

                Utilities.writeArrayLists(OutsTarget, OutsDecoy, this.tbxResultsFolder.Text, totBatches, currentBatch);
                //this.tStripProgressBar.Value = 25;
                Utilities.writeALJoined(outsJoined, this.tbxResultsFolder.Text, totBatches, currentBatch);
                //this.tStripProgressBar.Value = 50;
                Utilities.writeQuiXML(OutsTarget, FDRcutOff, this.tbxResultsFolder.Text, totBatches, currentBatch);
                //this.tStripProgressBar.Value = 75;
                Utilities.writeCSV(OutsTarget, FDRcutOff, this.tbxResultsFolder.Text, totBatches, currentBatch);

                writepRatioTXT(totBatches, currentBatch, OutsTarget, OutsDecoy);

                if (this.pIcheck.Checked)
                {
                    Utilities.writeWidth(this.tbxResultsFolder.Text, pIWidth, pIperFraction, totBatches, currentBatch);
                }

                if (this.chbPepXML.Checked)
                {
                    SearchParams = DA_paramsReader.readParams(lnkParams.Text);
                    Utilities.writepepXML(OutsTarget,
                                            FDRcutOff,
                                            srfHeaderTarget,
                                            SearchParams,
                                            this.tbxResultsFolder.Text,
                                            modificationsFilePath);
                }
                //this.tStripProgressBar.Value = 100;

                #endregion

            }

            this.statusStrip1.Visible = false;

            if (sender == null)
            {
                DateTime.Now.Subtract(this.t0);
                TimeSpan elapsedTime = DateTime.Now.Subtract(this.t0);

                Console.WriteLine("Total Duration: " + elapsedTime.Hours.ToString("00") 
                                   + ":" + elapsedTime.Minutes.ToString("00") 
                                   + ":" + elapsedTime.Seconds.ToString("00"));

                Environment.Exit(-1);
            }
            else
            {
                MessageBox.Show("FDR calculation has finished!");
            }

        }

        private void errorExit(string message)
        {
            this.statusStrip1.Visible = false;
            MessageBox.Show(message);
            tsWhat.Text = message;
            tStripProgressBar.Value = 0;
        }

        private void writepRatioTXT(int totBatches, int currentBatch, ArrayList OutsTarget, ArrayList OutsDecoy)
        {
            string pRatioTargetFilename = string.Concat(this.tbxResultsFolder.Text, "\\pRatios_target");
            if (totBatches > 1) pRatioTargetFilename += (currentBatch + 1).ToString() + "of" + totBatches.ToString();
            pRatioTargetFilename += ".txt";

            string pRatioDecoyFilename = string.Concat(this.tbxResultsFolder.Text, "\\pRatios_decoy");
            if (totBatches > 1) pRatioDecoyFilename += (currentBatch + 1).ToString() + "of" + totBatches.ToString();
            pRatioDecoyFilename += ".txt";

            write_pRatio_file(OutsTarget, pRatioTargetFilename);
            write_pRatio_file(OutsDecoy, pRatioDecoyFilename);
        }

        private static ArrayList removeEqualSequences(OutData.OutsComparer cmpXc1, ArrayList outsJ)
        {
            ArrayList outsJoinedPrev = (ArrayList)outsJ.Clone();
            outsJoinedPrev.Sort(cmpXc1);
            string prevSeq = "";
            OutData.databases prevDB = OutData.databases.Target;
            string prevRaw = "";
            int prevFirstScan = 0;
            int prevCharge = 0;

            for (int i = 0; i < outsJoinedPrev.Count; i++)
            {
                OutData sd = (OutData)outsJoinedPrev[i];

                if (sd.Sequence == prevSeq
                    && sd.dbType != prevDB
                    && sd.RAWFile == prevRaw
                    && sd.FirstScan == prevFirstScan
                    && sd.Charge == prevCharge
                    && i > 0)
                {
                    outsJoinedPrev.RemoveAt(i);
                    outsJoinedPrev.RemoveAt(i - 1);
                }
                else
                {
                    prevSeq = sd.Sequence;
                    prevDB = sd.dbType;
                    prevRaw = sd.RAWFile;
                    prevFirstScan = sd.FirstScan;
                    prevCharge = sd.Charge;
                }
            }

            ArrayList outsJoined = (ArrayList)outsJoinedPrev.Clone();
            return outsJoined;
        }

        private static void write_pRatio_file(ArrayList list, string concerningFile)
        {
            StreamWriter writer = new StreamWriter(concerningFile);

            string header = "RAWFile\tFisrtScan\tFASTAProteinDescription\tSequence\tCharge\tdeltaCn\tpRatio";
            writer.WriteLine(header);

            foreach (OutData sd in list)
            {
                //string matchId = sd.RAWFile.ToString() + "-" + sd.FirstScan.ToString() + "-" + sd.Charge.ToString();
                string matchId = sd.RAWFile.ToString();
                matchId += "\t" + sd.FirstScan.ToString();
                matchId += "\t" + sd.ProteinDescription.ToString();
                matchId += "\t" + sd.Sequence.ToString();
                matchId += "\t" + sd.Charge.ToString();
                matchId += "\t" + sd.DeltaCn.ToString();
                string line = matchId + "\t" + sd.pRatio.ToString();
                writer.WriteLine(line);
            }
            writer.Close();
        }

        private static ArrayList splitByFileType(ArrayList files, string[] extensions)
        {
            ArrayList splitFiles = new ArrayList();

            for (int i = 0; i < extensions.Length; i++)
            {
                ArrayList groupFiles = new ArrayList();
                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(extensions[i].ToLower()))
                        groupFiles.Add(file);
                }
                splitFiles.Add(groupFiles);
            }

            return splitFiles;
        }


        #region drag & drops and others

        private void targetSearch_DragDrop(object sender, DragEventArgs e)
        {
            string[] folder = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            string[] files = Directory.GetFiles(folder[0].ToString(),"*.*",System.IO.SearchOption.AllDirectories);

                foreach (string fil in files)
                {
                    if (fil.ToLower().EndsWith(".srf")
                        || fil.ToLower().EndsWith(".xml")
                        || fil.ToLower().EndsWith(".msf"))
                    {
                        tbxTargetSearch.Text = folder[0].ToString().Trim();
                        break;
                    }
                }
        }

        private void dragEnter(object sender, DragEventArgs e)
        {
            // make sure they're actually dropping files (not text or anything else)
            

            if (e.Data.GetDataPresent(DataFormats.FileDrop , false) == true)
            {
                // make sure the file is an xml file and is unique.
                // (without this, the cursor stays a "NO" symbol)
                string[] folder = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (folder.GetUpperBound(0) == 0)
                {
                    if (Directory.Exists(folder[0].ToString()))
                    {
                        e.Effect = DragDropEffects.All;
                    }
                }
            }
        }

        private void decoySearch_DragDrop(object sender, DragEventArgs e)
        {
            string[] folder = (string[])e.Data.GetData(DataFormats.FileDrop);

            string[] files = Directory.GetFiles(folder[0].ToString(), "*.*", System.IO.SearchOption.AllDirectories);

            foreach (string fil in files)
            {
                if (fil.ToLower().EndsWith(".srf")
                    || fil.ToLower().EndsWith(".xml")
                    || fil.ToLower().EndsWith(".msf"))
                {
                    tbxDecoySearch.Text = folder[0].ToString().Trim();
                    break;
                }
            }
          

        }

        private void resultsFolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] folder = (string[])e.Data.GetData(DataFormats.FileDrop);

            // the method is different here, as the folder can be empty
            if(Directory.Exists(folder[0]))
                tbxResultsFolder.Text = folder[0].ToString().Trim();
        }

        private void lblParams_DragDrop(object sender, DragEventArgs e)
        {

            string[] folder = (string[])e.Data.GetData(DataFormats.FileDrop);

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Directory.GetFiles(folder[0].ToString(), "*.*", System.IO.SearchOption.AllDirectories);

            foreach (string fil in files)
            {
                if (fil.ToLower().EndsWith(".params"))
                {
                    lnkParams.Text = folder[0].ToString().Trim();
                    break;
                }
            }
        }

        private void pIcheck_CheckedChanged(object sender, EventArgs e)
        {
            this.FDRpIcutOffLbl.Enabled = this.pIcheck.Checked;
            this.FDRpIcutoffTxt.Enabled = this.pIcheck.Checked;
        }
       
        
        #endregion

        enum fileType
        {
            srf,
            msf,
            xml
        }

        enum FDRmethod
        {
            separated,
            concatenated,
            mixed
        }

        private void cbxAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            gbxFDRmethod.Visible = cbxAdvanced.Checked;
            gbxpIbox.Visible = cbxAdvanced.Checked;
            gbxQuality.Visible = cbxAdvanced.Checked;
            gbxSplit.Visible = cbxAdvanced.Checked;
            gbxDeltaMass.Visible = cbxAdvanced.Checked;
            gbxNterminalModifs.Visible = cbxAdvanced.Checked;

            if (cbxAdvanced.Checked)
                this.Size = new Size(this.Width, 446);
            else
                this.Size = new Size(this.Width, 225);
        }

        private void cbxSplit_CheckedChanged(object sender, EventArgs e)
        {
            lblSplit.Enabled = cbxSplit.Checked;
            tbxSplit.Enabled = cbxSplit.Checked;
        }

        private void cbxDeltaMass_CheckedChanged(object sender, EventArgs e)
        {
            lblDeltaMass.Enabled = cbxDeltaMass.Checked;
            tbxDeltaMass.Enabled = cbxDeltaMass.Checked;

            rbnDMAreaOne.Enabled = cbxDeltaMass.Checked;
            rbnDMAreaThree.Enabled = cbxDeltaMass.Checked;
            rbnDMAreaFive.Enabled = cbxDeltaMass.Checked;
        }

        private void tbxTargetSearch_TextChanged(object sender, EventArgs e)
        {
            if (!cbxResultsFolder.Checked)
                tbxResultsFolder.Text = tbxTargetSearch.Text;
        }

        private void cbxResultsFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxResultsFolder.Checked)
            {
                tbxResultsFolder.Enabled = false;
                tbxResultsFolder.Text = tbxTargetSearch.Text;
            }
            else
                tbxResultsFolder.Enabled = true;
        }

        private void rbnPD20_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnPD20.Checked)
            {
                if (quality3.Checked || quality4.Checked || qualityAvg.Checked)
                    quality2.Checked = true;

                quality3.Enabled = false;
                quality4.Enabled = false;
                qualityAvg.Enabled = false;
            }
            else
            {
                quality3.Enabled = true;
                quality4.Enabled = true;
                qualityAvg.Enabled = true;
            }
        }
    }
}
