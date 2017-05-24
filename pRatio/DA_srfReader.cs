using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//using BIOUNIFIEDFILELib;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;

// using System.Data.SQLite; // this was to allow SQLite, but was replaced by the next one
using Community.CsharpSqlite.SQLiteClient; // important to read msf files

namespace pRatio
{
    public class Redundance
    {
        public int FASTAIndex
        {
            get { return FASTAIndexVal; }
            set { FASTAIndexVal = value; }
        }
        public string FASTAProteinDescription
        {
            get { return FASTAProteinDescriptionVal; }
            set { FASTAProteinDescriptionVal = value; }
        }

        private int FASTAIndexVal;
        private string FASTAProteinDescriptionVal;
    }

    public class Modification
    {
        public char aminoacid;
        public char symbol;
        public string name;
        public double deltaMass;
        public bool variable = true;
    }

    //[DebuggerDisplay("{dbType},{rnkXc1},{rnkXcRandom},FDR={FDR}")]
    //[DebuggerDisplay("{dbType},rnkXc1={rnkXc1},pRatio={pRatio}")]
    [DebuggerDisplay("RAW = {RAWFile}, FirstScan = {FirstScan}, z={Charge}")]
    //[DebuggerDisplay("pRatioT={pRatioTarget},pRatioD={pRatioDecoy}")]
    //[DebuggerDisplay("rnkT1={rnkXc1},rnkT2={rnkXcRandom}")]
    public class OutData : IComparable
    {
        public OutData(OutData.databases db)
        {
            dbType = db;
            FileNameVal = "";
            RAWFileVal = "";
            ProteinDescriptionVal = "";
            SequenceVal = "";

        }

        public override string ToString()
        {
            string s = Xcorr1Val.ToString(); //FileNameVal.ToString();
            return s;
        }


        //pI probability function
        public double fpI(float pItoCal, float sigma)
        {
            //Square function (centered in mu and with sigma width) 
            return Math.Abs(pItoCal-pImuVal) > sigma ? 0:1;
        }



        public int FirstScan
        {
            get { return FirstScanVal; }
            set { FirstScanVal = value; }
        }
        public int LastScan
        {
            get { return LastScanVal; }
            set { LastScanVal = value; }
        }
        public short Charge
        {
            get { return ChargeVal; }
            set { ChargeVal = value; }
        }
        public float LowestSpScore
        {
            get { return LowestSpScoreVal; }
            set { LowestSpScoreVal = value; }
        }

        public double PrecursorMass
        {
            get { return PrecursorMassVal; }
            set { PrecursorMassVal = value; }
        }

        public double TheoreticalMass
        {
            get { return TheoreticalMassVal; }
            set { TheoreticalMassVal = value; }
        }

        public float TotalIntensity
        {
            get { return TotalIntensityVal; }
            set { TotalIntensityVal = value; }
        }

        public float Xcorr1Target
        {
            get { return Xcorr1TargetVal; }
            set { Xcorr1TargetVal = value; }
        }

        public float Xcorr1Decoy
        {
            get { return Xcorr1DecoyVal; }
            set { Xcorr1DecoyVal = value; }
        }

        public float Xcorr1Original
        {
            get { return Xcorr1OriginalVal; }
        }

        public float Xcorr1
        {
            get { return Xcorr1Val; }
            set
            {
                Xcorr1SearchVal = value;
                if (value > float.Epsilon * 2)
                    Xcorr1OriginalVal = value;

                switch (xcorrType)
                {
                    case XCorrTypes.regular:
                        Xcorr1Val = value;
                        break;
                    case XCorrTypes.normalized:
                        float R = 1;
                        if (Charge < 3)
                        {
                            R = 1.0F;
                        }
                        else 
                        {
                            R = 1.22F;
                        }

                        Xcorr1Val = (float)(Math.Log10((double)value /(double)R) / Math.Log10(2 * Mass / (double)110));
                        
                        break;
                }

            }
        }
        public float Xcorr2
        {
            get { return Xcorr2Val; }
            set
            {
                Xcorr2SearchVal = value;
                switch (xcorrType)
                {
                    case XCorrTypes.regular:
                        Xcorr2Val = value;
                        break;
                    case XCorrTypes.normalized:
                        float R = 1;
                        if (Charge < 3)
                        {
                            R = 1.0F;
                        }
                        else
                        {
                            R = 1.22F;
                        }

                        Xcorr2Val = (float)(Math.Log10((double)value / (double)R) / Math.Log10(2 * Mass / (double)110));

                        break;
                }

            }
        }
        public float Xcorr3
        {
            get { return Xcorr3Val; }
            set
            {
                Xcorr3SearchVal = value;
                switch (xcorrType)
                {
                    case XCorrTypes.regular:
                        Xcorr3Val = value;
                        break;
                    case XCorrTypes.normalized:
                        float R = 1;
                        if (Charge < 3)
                        {
                            R = 1.0F;
                        }
                        else
                        {
                            R = 1.22F;
                        }

                        Xcorr3Val = (float)(Math.Log10((double)value / (double)R) / Math.Log10(2 * Mass / (double)110));

                        break;
                }

            }
        }
        public float Xcorr4
        {
            get { return Xcorr4Val; }
            set
            {
                Xcorr4SearchVal = value;
                switch (xcorrType)
                {
                    case XCorrTypes.regular:
                        Xcorr4Val = value;
                        break;
                    case XCorrTypes.normalized:
                        float R = 1;
                        if (Charge < 3)
                        {
                            R = 1.0F;
                        }
                        else
                        {
                            R = 1.22F;
                        }

                        Xcorr4Val = (float)(Math.Log10((double)value / (double)R) / Math.Log10(2 * Mass / (double)110));

                        break;
                }

            }
        }

        public float Xcorr1Search 
        {
            get { return Xcorr1SearchVal; }
        }
        public float Xcorr2Search
        {
            get { return Xcorr2SearchVal; }
        }
        public float Xcorr3Search
        {
            get { return Xcorr3SearchVal; }
        }
        public float Xcorr4Search
        {
            get { return Xcorr4SearchVal; }
        }

        public string FileName
        {
            get { return FileNameVal; }
            set
            {
                string fil = value;
                int first = fil.LastIndexOf('\\') + 1;
                int length = fil.Length - first;
                FileNameVal = fil.Substring(first, length).Trim(); ;
            }
        }
        public string RAWFile
        {
            get { return RAWFileVal; }
            set
            {
                string fil = value;
                int first = fil.LastIndexOf('\\') + 1;
                int length = fil.Length - first;
                RAWFileVal = fil.Substring(first, length).Trim();

            }
        }
        public float DeltaCn
        {
            get { return DeltaCnVal; }
            set { DeltaCnVal = value; }
        }
        public int FASTAIndex
        {
            get { return FASTAIndexVal; }
            set { FASTAIndexVal = value; }
        }
        public short IonsCompared
        {
            get { return IonsComparedVal; }
            set { IonsComparedVal = value; }
        }
        public short IonsMatched
        {
            get { return IonsMatchedVal; }
            set { IonsMatchedVal = value; }
        }
        public double Mass
        {
            get { return MassVal; }
            set { MassVal = value; }
        }
        public int MatchRank
        {
            get { return MatchRankVal; }
            set { MatchRankVal = value; }
        }
        public string ProteinDescription
        {
            get { return ProteinDescriptionVal; }
            set { ProteinDescriptionVal = value; }
        }

        public float ProteinMass
        {
            get { return ProteinMassVal; }
            set { ProteinMassVal = value; }
        }

        public short ProteinsWithPeptide
        {
            get { return ProteinsWithPeptideVal; }
            set { ProteinsWithPeptideVal = value; }
        }
        public string Sequence
        {
            get { return SequenceVal; }
            set { SequenceVal = value; }
        }

        public float Sp
        {
            get { return SpVal; }
            set { SpVal = value; }
        }
        public short SpRank
        {
            get { return SpRankVal; }
            set { SpRankVal = value; }
        }
        public Redundance[] Redundances
        {
            get { return RedundancesVal; }
            set { RedundancesVal = value; }
        }

        public float XcorrRandom
        {
            get { return XcorrRandomVal; }
            set { XcorrRandomVal = value; }
        }
        public double rnkXc1
        {
            get { return rnkXc1Val; }
            set { rnkXc1Val = value; }
        }
        public double rnkXcRandom
        {
            get { return rnkXcRandomVal; }
            set { rnkXcRandomVal = value; }
        }
        public double pRatio
        {
            get { return pRatioVal; }
            set { pRatioVal = value; }
        }
        public double pRatioTarget
        {
            get { return pRatioTargetVal; }
            set { pRatioTargetVal = value; }
        }
        public double pRatioDecoy
        {
            get { return pRatioDecoyVal; }
            set { pRatioDecoyVal = value; }
        }

        public float pI
        {
            get { return pIVal; }
            set { pIVal = value; }
        }
        public float pITarget
        {
            get { return pITargetVal; }
            set { pITargetVal = value; }
        }
        public float pIDecoy
        {
            get { return pIDecoyVal; }
            set { pIDecoyVal = value; }
        }

        public float pImu
        {
            get { return pImuVal; }
            set { pImuVal = value; }
        }

        public double pIprob
        {
            get { return pIprobVal; }
            set { pIprobVal = value; }
        }
  
        public double pIprobTarget
        {
            get { return pIprobTargetVal; }
            set { pIprobTargetVal = value; }
        }
        public double pIprobDecoy
        {
            get { return pIprobDecoyVal; }
            set { pIprobDecoyVal = value; }
        }


        public float XCorr1SearchOther
        {
            get { return XCorr1SearchOtherVal; }
            set { XCorr1SearchOtherVal = value; }
        }
        public double FDR
        {
            get { return FDRVal; }
            set { FDRVal = value; }
        }
        public OutData.databases dbType
        {
            get { return dbTypeVal; }
            set { dbTypeVal = value; }
        }
        public OutData.XCorrTypes xcorrType
        {
            get { return xcorrTypeVal; }
            set { xcorrTypeVal = value; }
        }

        public double retentionTime;

        private int FirstScanVal;
        private int LastScanVal;
        private short ChargeVal;
        private float LowestSpScoreVal;
        private double PrecursorMassVal;
        private double TheoreticalMassVal;
        private float TotalIntensityVal;
        private float Xcorr1Val;
        private float Xcorr1TargetVal;
        private float Xcorr1DecoyVal;
        private float Xcorr2Val;
        private float Xcorr3Val;
        private float Xcorr4Val;
        private float Xcorr1SearchVal;
        private float Xcorr1OriginalVal;
        private float Xcorr2SearchVal;
        private float Xcorr3SearchVal;
        private float Xcorr4SearchVal;
        private string FileNameVal;
        private string RAWFileVal;
        private float DeltaCnVal;
        private int FASTAIndexVal;
        private short IonsComparedVal;
        private short IonsMatchedVal;
        private double MassVal;
        private int MatchRankVal;
        private string ProteinDescriptionVal;
        private float ProteinMassVal;
        private short ProteinsWithPeptideVal;
        private string SequenceVal;
        private float SpVal;
        private short SpRankVal;
        private Redundance[] RedundancesVal;

        private float XcorrRandomVal;
        private double rnkXc1Val;
        private double rnkXcRandomVal;
        private double pRatioVal;
        private double pRatioTargetVal;
        private double pRatioDecoyVal;
        private float pIVal;
        private float pITargetVal;
        private float pIDecoyVal;
        private double pIprobVal;
        private double pIprobTargetVal;
        private double pIprobDecoyVal;
                
        private float pImuVal;
       


        private float XCorr1SearchOtherVal;
        private double FDRVal;
        public enum databases
        {
            Decoy,
            Target
        }
        private OutData.databases dbTypeVal;
        public enum XCorrTypes
        {
            normalized,
            regular
        }
        private OutData.XCorrTypes xcorrTypeVal;


        public int CompareTo(Object rhs)
        {
            OutData r = (OutData)rhs;
            return this.Xcorr1.CompareTo(r.Xcorr1);
        }

        // Special implementation to be called by custom comparer 
        public int CompareTo(
           OutData rhs,
           OutData.OutsComparer.ComparisonType which)
        {
            switch (which)
            {
                case OutData.OutsComparer.ComparisonType.Xcorr1:
                    return this.Xcorr1.CompareTo(rhs.Xcorr1);
                case OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorr1:
                    return this.Xcorr1.CompareTo(rhs.XcorrRandom);
                case OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorrRandom:
                    return this.XcorrRandom.CompareTo(rhs.XcorrRandom);
                case OutData.OutsComparer.ComparisonType.pRatio:
                    return this.pRatio.CompareTo(rhs.pRatio);
                case OutData.OutsComparer.ComparisonType.pIprob:
                    return this.pIprob.CompareTo(rhs.pIprob);
                case OutData.OutsComparer.ComparisonType.FDR:
                    return this.FDR.CompareTo(rhs.FDR);
                case OutData.OutsComparer.ComparisonType.RAWfile:
                    return this.RAWFile.CompareTo(rhs.RAWFile);
                case OutData.OutsComparer.ComparisonType.key:
                    {
                        int result = 0;
                        result = this.RAWFile.CompareTo(rhs.RAWFile);
                        if (result != 0)
                        {
                            return result;
                        }
                        result = this.FirstScan.CompareTo(rhs.FirstScan);
                        if (result != 0)
                        {
                            return result;
                        }
                        result = this.Charge.CompareTo(rhs.Charge);
                        if (result != 0)
                        {
                            return result;
                        }
                        return result;
                    }
                case OutData.OutsComparer.ComparisonType.RnkXc1:
                    return this.rnkXc1.CompareTo(rhs.rnkXc1);
            }
            return 0;

        }


        // nested class which implements IComparer 
        public class OutsComparer : IComparer
        {
            // enumeration of comparsion types 
            public enum ComparisonType
            {
                Xcorr1,
                XcorrRandomVSXcorr1,
                XcorrRandomVSXcorrRandom,
                RAWfile,
                key,
                pRatio,
                FDR,
                RnkXc1,
                pIprob
            };

            // Tell the OutData objects to compare themselves 
            public int Compare(object lhs, object rhs)
            {
                OutData l = (OutData)lhs;
                OutData r = (OutData)rhs;
                return l.CompareTo(r, WhichComparison);
            }

            public OutData.OutsComparer.ComparisonType WhichComparison
            {
                get
                {
                    return whichComparisonVal;
                }
                set
                {
                    whichComparisonVal = value;
                }
            }

            // private state variable 
            private OutData.OutsComparer.ComparisonType whichComparisonVal;
        }
    }

    public class DA_paramsReader
    {
        public DA_paramsReader()
        { }

        public class paramsData
        {
            public paramsData()
            { }

            public string first_database_name
            {
                get { return first_database_nameVal; }
                set { first_database_nameVal = value; }
            }
            public string second_database_name
            {
                get { return second_database_nameVal; }
                set { second_database_nameVal = value; }
            }
            public float peptide_mass_tolerance
            {
                get { return peptide_mass_toleranceVal; }
                set { peptide_mass_toleranceVal = value; }
            }
            public massUnits peptide_mass_units
            {
                get { return peptide_mass_unitsVal; }
                set { peptide_mass_unitsVal = value; }
            }
            public string ion_series
            {
                get { return ion_seriesVal; }
                set { ion_seriesVal = value; }
            }
            public float fragment_ion_tolerance
            {
                get { return fragment_ion_toleranceVal; }
                set { fragment_ion_toleranceVal = value; }
            }
            public short num_output_lines
            {
                get { return num_output_linesVal; }
                set { num_output_linesVal = value; }
            }
            public int num_results
            {
                get { return num_resultsVal; }
                set { num_resultsVal = value; }
            }
            public short num_description_lines
            {
                get { return num_description_linesVal; }
                set { num_description_linesVal = value; }
            }
            public bool show_fragment_ions
            {
                get { return show_fragment_ionsVal; }
                set { show_fragment_ionsVal = value; }
            }
            public int print_duplicate_references
            {
                get { return print_duplicate_referencesVal; }
                set { print_duplicate_referencesVal = value; }
            }
            public enzyme enzyme_info
            {
                get { return enzyme_infoVal; }
                set { enzyme_infoVal = value; }
            }
            public short max_num_differential_per_peptide
            {
                get { return max_num_differential_per_peptideVal; }
                set { max_num_differential_per_peptideVal = value; }
            }
            public string diff_search_options
            {
                get { return diff_search_optionsVal; }
                set { diff_search_optionsVal = value; }
            }
            public string term_diff_search_options
            {
                get { return term_diff_search_optionsVal; }
                set { term_diff_search_optionsVal = value; }
            }
            public int nucleotide_reading_frame
            {
                get { return nucleotide_reading_frameVal; }
                set { nucleotide_reading_frameVal = value; }
            }
            public massType mass_type_parent
            {
                get { return mass_type_parentVal; }
                set { mass_type_parentVal = value; }
            }
            public massType mass_type_fragment
            {
                get { return mass_type_fragmentVal; }
                set { mass_type_fragmentVal = value; }
            }
            public bool normalize_xcorr
            {
                get { return normalize_xcorrVal; }
                set { normalize_xcorrVal = value; }
            }
            public bool remove_precursor_peak
            {
                get { return remove_precursor_peakVal; }
                set { remove_precursor_peakVal = value; }
            }
            public float ion_cutoff_percentage
            {
                get { return ion_cutoff_percentageVal; }
                set { ion_cutoff_percentageVal = value; }
            }
            public short max_num_internal_cleavage_sites
            {
                get { return max_num_internal_cleavage_sitesVal; }
                set { max_num_internal_cleavage_sitesVal = value; }
            }
            public massRange protein_mass_filter
            {
                get { return protein_mass_filterVal; }
                set { protein_mass_filterVal = value; }
            }
            public short match_peak_count
            {
                get { return match_peak_countVal; }
                set { match_peak_countVal = value; }
            }
            public short match_peak_allowed_error
            {
                get { return match_peak_allowed_errorVal; }
                set { match_peak_allowed_errorVal = value; }
            }
            public float match_peak_tolerance
            {
                get { return match_peak_toleranceVal; }
                set { match_peak_toleranceVal = value; }
            }
            public string partial_sequence
            {
                get { return partial_sequenceVal; }
                set { partial_sequenceVal = value; }
            }
            public string sequence_header_filter
            {
                get { return sequence_header_filterVal; }
                set { sequence_header_filterVal = value; }
            }
            public massRange digest_mass_range
            {
                get { return digest_mass_rangeVal; }
                set { digest_mass_rangeVal = value; }
            }


            private string first_database_nameVal;
            private string second_database_nameVal;
            private float peptide_mass_toleranceVal;
            private massUnits peptide_mass_unitsVal;
            private string ion_seriesVal;
            private float fragment_ion_toleranceVal;
            private short num_output_linesVal;
            private int num_resultsVal;
            private short num_description_linesVal;
            private bool show_fragment_ionsVal;
            private int print_duplicate_referencesVal;
            private enzyme enzyme_infoVal;
            private short max_num_differential_per_peptideVal;
            private string diff_search_optionsVal;
            private string term_diff_search_optionsVal;
            private int nucleotide_reading_frameVal;
            private massType mass_type_parentVal;
            private massType mass_type_fragmentVal;
            private bool normalize_xcorrVal;
            private bool remove_precursor_peakVal;
            private float ion_cutoff_percentageVal;
            private short max_num_internal_cleavage_sitesVal;
            private massRange protein_mass_filterVal;
            private short match_peak_countVal;
            private short match_peak_allowed_errorVal;
            private float match_peak_toleranceVal;
            private string partial_sequenceVal;
            private string sequence_header_filterVal;
            private massRange digest_mass_rangeVal;

            public class massRange
            {
                public float min
                {
                    get { return minVal; }
                    set { minVal = value; }
                }
                public float max
                {
                    get { return maxVal; }
                    set { maxVal = value; }
                }

                private float minVal;
                private float maxVal;
            }
            public enum massUnits
            {
                amu,
                mmu,
                ppm
            }
            public enum massType
            {
                monoisotopic,
                average
            }
            public class enzyme
            {

                public string name
                {
                    get { return nameVal; }
                    set { nameVal = value; }
                }
                public string cut
                {
                    get { return cutVal; }
                    set { cutVal = value; }
                }
                public string noCut
                {
                    get { return noCutVal; }
                    set { noCutVal = value; }
                }
                public senseType sense
                {
                    get { return senseVal; }
                    set { senseVal = value; }
                }


                public enum senseType
                {
                    C,
                    N
                }

                private string nameVal;
                private string cutVal;
                private string noCutVal;
                private senseType senseVal;

            }

        }


        public static paramsData readParams(string paramsFile)
        {
            paramsData pd = new paramsData();

            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(paramsFile));
                ArrayList myAL = new ArrayList();

                try
                {
                    while (sr.Peek() != -1) //equivalente a feof en C
                    {
                        myAL.Add(sr.ReadLine());
                    }
                }
                catch
                {
                    //Console.WriteLine(" Parse error: " + e);
                }

                sr.Close();

                //int r = myAL.Count;


                foreach (object o in myAL)
                {
                    try
                    {
                        string[] str;
                        string[] str2;
                        str = Regex.Split(o.ToString(), "=");

                        if (str.GetUpperBound(0) > 0)
                        {
                            str2 = Regex.Split(str[1], ";");

                            string myParam = str[0].Trim();
                            string myValue = str2[0].Trim();

                            switch (myParam)
                            {
                                case "first_database_name":
                                    {
                                        pd.first_database_name = myValue;
                                        break;
                                    }
                                case "second_database_name":
                                    {
                                        pd.second_database_name = myValue;
                                        break;
                                    }
                                case "peptide_mass_tolerance":
                                    {
                                        try
                                        {
                                            pd.peptide_mass_tolerance = float.Parse(myValue);
                                        }
                                        catch { }
                                        break;
                                    }
                                case "peptide_mass_units":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            if (iVal == 0)
                                            {
                                                pd.peptide_mass_units = paramsData.massUnits.amu;
                                            }
                                            if (iVal == 1)
                                            {
                                                pd.peptide_mass_units = paramsData.massUnits.mmu;
                                            }
                                            if (iVal == 2)
                                            {
                                                pd.peptide_mass_units = paramsData.massUnits.ppm;
                                            }
                                        }
                                        catch { }
                                        break;
                                    }

                                case "ion_series":
                                    {
                                        pd.ion_series = myValue;
                                        break;
                                    }

                                case "fragment_ion_tolerance":
                                    {
                                        try
                                        {
                                            pd.fragment_ion_tolerance = float.Parse(myValue);
                                        }
                                        catch { }
                                        break;
                                    }
                                case "num_output_lines":
                                    {
                                        try
                                        {
                                            pd.num_output_lines = short.Parse(myValue);
                                        }
                                        catch { }
                                        break;
                                    }
                                case "num_results":
                                    {
                                        try { pd.num_results = int.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "num_description_lines":
                                    {
                                        try { pd.num_description_lines = short.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "show_fragment_ions":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            if (iVal == 0)
                                            {
                                                pd.show_fragment_ions = false;
                                            }
                                            if (iVal == 1)
                                            {
                                                pd.show_fragment_ions = true;
                                            }
                                        }
                                        catch { }
                                        break;
                                    }
                                case "print_duplicate_references":
                                    {
                                        try { pd.print_duplicate_references = int.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "enzyme_info":
                                    {
                                        try
                                        {
                                            paramsData.enzyme myEnzyme = new paramsData.enzyme();
                                            if (Regex.IsMatch(myValue, "trypsin", RegexOptions.IgnoreCase))
                                            {
                                                myEnzyme.name = "trypsin";
                                                myEnzyme.cut = "KR";
                                                myEnzyme.noCut = "P";
                                                myEnzyme.sense = paramsData.enzyme.senseType.C;
                                            }
                                            pd.enzyme_info = myEnzyme;
                                        }
                                        catch { }
                                        break;
                                    }
                                case "max_num_differential_per_peptide":
                                    {
                                        try { pd.max_num_differential_per_peptide = short.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "diff_search_options":
                                    {
                                        try { pd.diff_search_options = myValue; }
                                        catch { }
                                        break;
                                    }
                                case "term_diff_search_options":
                                    {
                                        try { pd.term_diff_search_options = myValue; }
                                        catch { }
                                        break;
                                    }
                                case "nucleotide_reading_frame":
                                    {
                                        try { pd.nucleotide_reading_frame = int.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "mass_type_parent":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            switch (iVal)
                                            {
                                                case 0:
                                                    {
                                                        pd.mass_type_parent = paramsData.massType.average;
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        pd.mass_type_parent = paramsData.massType.monoisotopic;
                                                        break;
                                                    }
                                            }
                                        }
                                        catch { }
                                        break;
                                    }
                                case "mass_type_fragment":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            switch (iVal)
                                            {
                                                case 0:
                                                    {
                                                        pd.mass_type_fragment = paramsData.massType.average;
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        pd.mass_type_fragment = paramsData.massType.monoisotopic;
                                                        break;
                                                    }
                                            }
                                        }
                                        catch { }
                                        break;
                                    }
                                case "normalize_xcorr":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            switch (iVal)
                                            {
                                                case 0:
                                                    {
                                                        pd.normalize_xcorr = false;
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        pd.normalize_xcorr = true;
                                                        break;
                                                    }
                                            }
                                        }
                                        catch { }
                                        break;
                                    }
                                case "remove_precursor_peak":
                                    {
                                        try
                                        {
                                            int iVal = int.Parse(myValue);
                                            switch (iVal)
                                            {
                                                case 0:
                                                    {
                                                        pd.remove_precursor_peak = false;
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        pd.remove_precursor_peak = true;
                                                        break;
                                                    }
                                            }
                                        }
                                        catch { }
                                        break;
                                    }
                                case "ion_cutoff_percentage":
                                    {
                                        try { pd.ion_cutoff_percentage = float.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "max_num_internal_cleavage_sites":
                                    {
                                        try { pd.max_num_internal_cleavage_sites = short.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "protein_mass_filter":
                                    {
                                        try
                                        {
                                            string[] str3 = Regex.Split(myValue, " ");
                                            pd.protein_mass_filter.min = float.Parse(str3[0]);
                                            pd.protein_mass_filter.max = float.Parse(str3[1]);

                                        }
                                        catch { }
                                        break;
                                    }
                                case "match_peak_count":
                                    {
                                        try { pd.match_peak_count = short.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "match_peak_allowed_error":
                                    {
                                        try { pd.match_peak_allowed_error = short.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "match_peak_tolerance":
                                    {
                                        try { pd.match_peak_tolerance = float.Parse(myValue); }
                                        catch { }
                                        break;
                                    }
                                case "partial_sequence":
                                    {
                                        try { pd.partial_sequence = myValue; }
                                        catch { }
                                        break;
                                    }
                                case "sequence_header_filter":
                                    {
                                        try { pd.sequence_header_filter = myValue; }
                                        catch { }
                                        break;
                                    }
                                case "digest_mass_range":
                                    {
                                        try
                                        {
                                            string[] str3 = Regex.Split(myValue, " ");
                                            pd.digest_mass_range.min = float.Parse(str3[0]);
                                            pd.digest_mass_range.max = float.Parse(str3[1]);
                                        }
                                        catch { }
                                        break;
                                    }
                            }

                        }


                    }
                    catch { }
                }



            }
            catch
            {
            }

            return pd;
        }




    }

    public class DA_fileReader
    {

        public class SrfData : IComparable
        {
            public SrfData()
            { }


            public string Comment
            {
                get { return CommentVal; }
                set { CommentVal = value; }
            }
            public string Database
            {
                get { return DatabaseVal; }
                set { DatabaseVal = value; }
            }
            public string Enzyme
            {
                get { return EnzymeVal; }
                set { EnzymeVal = value; }
            }
            public int FirstScan
            {
                get { return FirstScanVal; }
                set { FirstScanVal = value; }
            }
            public string Instrument
            {
                get { return InstrumentVal; }
                set { InstrumentVal = value; }
            }
            public float IntensityThreshold
            {
                get { return IntensityThresholdVal; }
                set { IntensityThresholdVal = value; }
            }
            public int LastScan
            {
                get { return LastScanVal; }
                set { LastScanVal = value; }
            }
            public short MassType
            {
                get { return MassTypeVal; }
                set { MassTypeVal = value; }
            }
            public short MassUnits
            {
                get { return MassUnitsVal; }
                set { MassUnitsVal = value; }
            }
            public float MaxMass
            {
                get { return MaxMassVal; }
                set { MaxMassVal = value; }
            }
            public float MinMass
            {
                get { return MinMassVal; }
                set { MinMassVal = value; }
            }
            public string Modifications
            {
                get { return ModificationsVal; }
                set { ModificationsVal = value; }
            }
            public float MSMSTolerance
            {
                get { return MSMSToleranceVal; }
                set { MSMSToleranceVal = value; }
            }
            public float MSTolerance
            {
                get { return MSToleranceVal; }
                set { MSToleranceVal = value; }
            }
            public int NumberOfSearchEntries
            {
                get { return NumberOfSearchEntriesVal; }
                set { NumberOfSearchEntriesVal = value; }
            }
            public string Operator
            {
                get { return OperatorVal; }
                set { OperatorVal = value; }
            }
            public string ParamsFile
            {
                get { return ParamsFileVal; }
                set { ParamsFileVal = value; }
            }
            public string RAWFile
            {
                get { return RAWFileVal; }
                set { RAWFileVal = value; }
            }
            public float SNThreshold
            {
                get { return SNThresholdVal; }
                set { SNThresholdVal = value; }
            }
            public short SRFVersion
            {
                get { return SRFVersionVal; }
                set { SRFVersionVal = value; }
            }

            private string CommentVal;
            private string DatabaseVal;
            private string EnzymeVal;
            private int FirstScanVal;
            private string InstrumentVal;
            private float IntensityThresholdVal;
            private int LastScanVal;
            private short MassTypeVal;
            private short MassUnitsVal;
            private float MaxMassVal;
            private float MinMassVal;
            private string ModificationsVal;
            private float MSMSToleranceVal;
            private float MSToleranceVal;
            private int NumberOfSearchEntriesVal;
            private string OperatorVal;
            private string ParamsFileVal;
            private string RAWFileVal;
            private float SNThresholdVal;
            private short SRFVersionVal;


            public int CompareTo(Object rhs)
            {
                SrfData r = (SrfData)rhs;
                return this.Enzyme.CompareTo(r.Enzyme);
            }

            // Special implementation to be called by custom comparer 
            public int CompareTo(
               SrfData rhs,
               SrfData.SrfsComparer.ComparisonType which)
            {
                switch (which)
                {
                    case SrfData.SrfsComparer.ComparisonType.enzyme:
                        return this.Enzyme.CompareTo(rhs.Enzyme);
                    case SrfData.SrfsComparer.ComparisonType.modifications:
                        return this.Modifications.CompareTo(rhs.Modifications);
                    case SrfData.SrfsComparer.ComparisonType.total:
                        {

                            int result = 0;
                            result = this.Enzyme.CompareTo(rhs.Enzyme);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.RAWFile.CompareTo(rhs.RAWFile);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.Modifications.CompareTo(rhs.Modifications);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.FirstScan.CompareTo(rhs.FirstScan);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.LastScan.CompareTo(rhs.LastScan);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MSTolerance.CompareTo(rhs.MSTolerance);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MSMSTolerance.CompareTo(rhs.MSMSTolerance);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.IntensityThreshold.CompareTo(rhs.IntensityThreshold);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MassType.CompareTo(rhs.MassType);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MassUnits.CompareTo(rhs.MassUnits);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MaxMass.CompareTo(rhs.MaxMass);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.MinMass.CompareTo(rhs.MinMass);
                            if (result != 0)
                            {
                                return result;
                            }
                            result = this.SNThreshold.CompareTo(rhs.SNThreshold);
                            if (result != 0)
                            {
                                return result;
                            }
                           
                            
                            return result;
                        }                    
                }
                return 0;

            }


            // nested class which implements IComparer 
            public class SrfsComparer : IComparer
            {
                // enumeration of comparsion types 
                public enum ComparisonType
                {
                    total,
                    enzyme,
                    modifications
                    
                };

                // Tell the SrfData objects to compare themselves 
                public int Compare(object lhs, object rhs)
                {
                    SrfData l = (SrfData)lhs;
                    SrfData r = (SrfData)rhs;
                    return l.CompareTo(r, WhichComparison);
                }

                public SrfData.SrfsComparer.ComparisonType WhichComparison
                {
                    get
                    {
                        return whichComparisonVal;
                    }
                    set
                    {
                        whichComparisonVal = value;
                    }
                }

                // private state variable 
                private SrfData.SrfsComparer.ComparisonType whichComparisonVal;
            }

        }

        public static ArrayList readOuts(string[] files,
                                        DA_fileReader.Quality quality,
                                        DA_fileReader.XcorrSelection Xcorr,
                                        OutData.databases whichDB,
                                        fileType filetype,
                                        string modificationsFile,
                                        ToolStripProgressBar progressBar,
                                        ToolStripStatusLabel statusLabel,
                                        int _totBatches,
                                        int _numBatch,
                                        ref int _errorNumber,
                                        double NtermStaticModif,
                                        DA_fileReader.PDVersion PDVersionUsed,
                                        ref string errorMessage)
        {
            ArrayList outsList = new ArrayList();

            DataSet ds = new DataSet("aa");
            ds.ReadXmlSchema(System.AppDomain.CurrentDomain.BaseDirectory + pRatio.Properties.Settings.Default.aaWeight_Schema.ToString());
            ds.ReadXml(System.AppDomain.CurrentDomain.BaseDirectory + pRatio.Properties.Settings.Default.aaWeight_XML.ToString());

            switch (filetype)
            {
                //case fileType.SRF:
                //    outsList = readOutsSRF(files, quality, whichDB, ds); //*** add to modificationsFile
                //    break;
                case fileType.XML:
                    outsList = readOutsXML(files, quality, whichDB, ds);
                    break;
                case fileType.MSF:
                    outsList = readOutsMSF(files,
                                            quality,
                                            whichDB,
                                            ds,
                                            modificationsFile,
                                            progressBar,
                                            statusLabel,
                                            _totBatches,
                                            _numBatch,
                                            ref _errorNumber,
                                            NtermStaticModif,
                                            PDVersionUsed,
                                            ref errorMessage);
                    break;
            }

            if (outsList == null) return new ArrayList();
            return outsList;
        }

        private static ArrayList readOutsMSF(string[] msfFiles,
                                        DA_fileReader.Quality quality, //second, third or fourth Xcorr to be used
                                        OutData.databases whichDB, // target or decoy
                                        DataSet ds, // ds is to get amino acid info, so isoelectric point can be calculated
                                        string modsFile, // important to preserve the same modification code for all mods
                                        ToolStripProgressBar _progressBar,
                                        ToolStripStatusLabel _statusLabel,
                                        int _totBatches,
                                        int _numBatch, // zero based, because it is the rest
                                        ref int errorNumber,
                                        DA_fileReader.PDVersion PDVersionUsed,
                                        ref string errorMessage)
        {
            return readOutsMSF(msfFiles,
                                quality, //second, third or fourth Xcorr to be used
                                whichDB, // target or decoy
                                ds, // ds is to get amino acid info, so isoelectric point can be calculated
                                modsFile, // important to preserve the same modification code for all mods
                                _progressBar,
                                _statusLabel,
                                _totBatches,
                                _numBatch, // zero based, because it is the rest
                                ref errorNumber,
                                0,
                                PDVersionUsed,
                                ref errorMessage);
        }

        private static ArrayList readOutsMSF(string[] msfFiles,
                                        DA_fileReader.Quality quality, //second, third or fourth Xcorr to be used
                                        OutData.databases whichDB, // target or decoy
                                        DataSet ds, // ds is to get amino acid info, so isoelectric point can be calculated
                                        string modsFile, // important to preserve the same modification code for all mods
                                        ToolStripProgressBar _progressBar,
                                        ToolStripStatusLabel _statusLabel,
                                        int _totBatches,
                                        int _numBatch, // zero based, because it is the rest
                                        ref int errorNumber,
                                        double NtermStaticModif,
                                        DA_fileReader.PDVersion PDVersionUsed,
                                        ref string errorMessage) // for example for iTRAQ
        {
            // next query is important to get all the tables
            //string query = "SELECT name FROM sqlite_master WHERE type = 'table';";

            string dbName = "for " + whichDB.ToString().ToLower() + " db";

            string fileTableName = "workFlowInputFiles";
            string rankExtra = "";
            string deltaScoreExtra = "";
            string onlyRank1 = "";

            switch (PDVersionUsed)
            {
                case PDVersion.PD14AndPrevious:
                    {
                        fileTableName = "fileInfos";
                        break;
                    }
                case PDVersion.PD20:
                    {
                        fileTableName = "workFlowInputFiles";
                        rankExtra = ", p.searchenginerank";
                        deltaScoreExtra = ", p.deltascore";
                        onlyRank1 = "and p.searchenginerank = 1 ";
                        break;
                    }
            }

            string queryMain = "select "
                                + "p.peptideid, " // [0] not in XML, but useful for relations with other data
                                + "fi.filename, " // [1] the msf full path
                                + "sh.firstscan, " // [2]
                                + "sh.lastscan, " // [3]
                                + "sh.charge, " // [4]
                                + "p.sequence, " //[5]
                                + "sh.mass, " // [6] precursor mass
                                + "ps.scorevalue, " // [7] Xcorr (first or second, as this has to be sorted out)
                                + "sh.retentiontime" // [8]
                                + rankExtra // [9] peptides.SearchEngineRank, for PD2.0
                                + deltaScoreExtra // [10] peptides.DeltaScore, for PD2.0
                                + " "
                //
                                + "from peptides p, "
                                + "peptideScores ps, "
                                + "spectrumHeaders sh, "
                                + "massPeaks mp, "
                                + fileTableName + " fi, "
                                + "processingNodeScores scoreNames "
                //
                                + "where p.peptideid = ps.peptideid "
                                + "and sh.spectrumid = p.spectrumid "
                                + "and (fi.fileid = mp.fileid or mp.fileid = -1) "
                                + "and mp.masspeakid = sh.masspeakid "
                                + "and scoreNames.scoreid = ps.scoreid "
                                + "and scoreNames.ScoreName = 'Xcorr' " // usually scoreId = 7 stands for Xcorr, but this changes when msf files are imported from srf
                //
                                + "and sh.firstscan % TOTBATCHES = NUMBATCH "
                                + onlyRank1 // empty if not PD2.0

                                + "order by "
                                + "fi.filename desc, " // sorted by raw file
                                + "sh.firstscan asc, " // sorted by scan number
                                + "sh.lastscan asc, " // just in case
                                + "sh.charge asc, " // important, as otherwise candidates of every charge will get mixed!
                                + "ps.scorevalue desc;"; // so the 1st is the Xcorr1 and 2nd the Xcorr2

            string queryModifications = "select "
                                + "p.peptideid, " // [0]
                                + "paam.aminoacidmodificationid, " // [1]
                                + "paam.position, " //[2] WARNING: this is zero-based
                                + "p.sequence, " // [3]
                                + "aam.modificationname, " // [4]
                                + "aam.deltamass " // [5]
                //
                                + "from peptides p, "
                                + "spectrumHeaders sh, "
                                + "peptidesaminoacidmodifications paam, "
                                + "aminoacidmodifications aam "
                //
                                + "where p.peptideid = paam.peptideid "
                                + "and sh.spectrumid = p.spectrumid "
                                + "and sh.firstscan % TOTBATCHES = NUMBATCH "
                                + "and aam.aminoacidmodificationid = paam.aminoacidmodificationid "
                                + onlyRank1 // empty if not PD2.0
                                + "order by p.peptideid ASC, paam.position ASC;";

            string queryProteinInfo = "select "
                                + "pq.peptideid, " // [0]
                                + "p.sequence, " // [1]
                                + "pq.proteinid, " // [2]
                                + "q.description " // [3]
                //
                                + "from peptidesProteins pq, "
                                + "spectrumHeaders sh, "
                                + "peptides p, "
                                + "proteinAnnotations q "
                //
                                + "where pq.peptideid = p.peptideid "
                                + "and pq.proteinid = q.proteinid "
                                + "and sh.spectrumid = p.spectrumid "
                                + "and sh.firstscan % TOTBATCHES = NUMBATCH "
                                + onlyRank1 // empty if not PD2.0
                                + "order by pq.peptideid asc;";

            // next will not be used for PD2.0
            string querySpScore = "select "
                                + "p.peptideid, " // [0]
                                + "ps.scorevalue " // [1] SpScore
                //
                                + "from peptides p, "
                                + "peptideScores ps, "
                                + "spectrumHeaders sh, "
                                + "processingNodeScores scoreNames "
                //
                                + "where p.peptideid = ps.peptideid "
                                + "and sh.spectrumid = p.spectrumid "
                                + "and sh.firstscan % TOTBATCHES = NUMBATCH "
                                + "and scoreNames.scoreid = ps.scoreid "
                                + "and scoreNames.ScoreName = 'SpScore';"; // usually ScoreId = 8 stands for the spScore if msf files are not imported from srf

            // outList is made of currOuts, which are the single lines.
            ArrayList outsList = new ArrayList();

            int msfFileNumber = 0;
            foreach (string msfFile in msfFiles)
            {
                msfFileNumber++;
                // double ddd = askDB_test_community(queryMain, msfFile, 100000);

                string endStatus = dbName + ", msf " + msfFileNumber.ToString() + "/" + msfFiles.Length.ToString() + ")";
                _statusLabel.Text = "performing main query (" + endStatus;
                Application.DoEvents();
                ArrayList mainList = askDBbatches(queryMain, msfFile, _totBatches, _numBatch);
                if (mainList == null && PDVersionUsed == PDVersion.PD20)
                {
                    errorMessage = "No PSMs were found.\nPlease, are you really using MSF files from Proteome Discoverer 2.0";
                    return new ArrayList();
                }

                _statusLabel.Text = "performing modification query (" + endStatus;
                Application.DoEvents();
                ArrayList modificationsList = askDBbatches(queryModifications, msfFile, _totBatches, _numBatch);

                _statusLabel.Text = "performing protein info query (" + endStatus;
                Application.DoEvents();
                ArrayList proteinInfoList = askDBbatches(queryProteinInfo, msfFile, _totBatches, _numBatch);

                ArrayList SpScoreList = new ArrayList();
                if (PDVersionUsed != PDVersion.PD20)
                {
                    _statusLabel.Text = "performing SpScore query (" + endStatus;
                    Application.DoEvents();
                    SpScoreList = askDBbatches(querySpScore, msfFile, _totBatches, _numBatch);
                }

                _statusLabel.Text = "extracting modifications (" + endStatus;
                Application.DoEvents();
                ArrayList modificationsFound = extractModifications(modificationsList, modsFile);

                ArrayList staticModifications = new ArrayList();

                if (PDVersionUsed == PDVersion.PD14AndPrevious)
                {
                    staticModifications = getStaticModifications(msfFile, PDVersionUsed, ref errorMessage);
                    // please, improve error handling if this becomes more serious
                    if (errorMessage == "MSF file error.\nPlease check Proteome Discoverer is 1.4 or previous.")
                    {
                        return new ArrayList();
                    }
                }

                removeStaticModsInVariableList(modificationsFound, staticModifications);
                modificationsFound.AddRange(staticModifications);

                ArrayList peptideIdInProteinInfo = new ArrayList();
                for (int i = 0; i < proteinInfoList.Count; i++)
                {
                    object[] proteinItem = (object[])proteinInfoList[i];
                    int pepId = int.Parse(proteinItem[0].ToString());
                    peptideIdInProteinInfo.Add(pepId);
                }

                int workingScan = 0;
                int workingCharge = 0;
                string workingRaw = "";

                _statusLabel.Text = "adapting information (" + endStatus;
                Application.DoEvents();

                int extraElement = 1;
                if (quality.whichQuality == Quality.QualityType.FirstOnly) extraElement = 0;

                // the forMain
                for (int i = 0; i < mainList.Count - extraElement; i++) // mainList.Count - 1, as we take them in pairs, unless FirstOnly
                {
                    updateProgressBar(whichDB, _progressBar, mainList, i, msfFileNumber, msfFiles.Length);
                    int qualityLevel = 0;

                    object[] item1;
                    object[] item2;
                    object[] item3 = new object[0];
                    object[] item4 = new object[0];
                    object[] itemN = new object[0];
                    int charge1;
                    int charge2;
                    int charge3;
                    int charge4;
                    int chargeN = 0;

                    item1 = (object[])mainList[i];

                    if (PDVersionUsed == PDVersion.PD14AndPrevious)
                        item2 = (object[])mainList[i + extraElement];
                    else
                    {
                        item2 = (object[])item1.Clone();
                        double deltaScore = (double)item2[10];
                        double XCorr1 = (double)item1[7];
                        double XCorr2 = XCorr1 * (1 - deltaScore);
                        item2[5] = "--not retrieved";
                        item2[7] = XCorr2;
                        item2[9] = 2;
                    }

                    charge1 = (int)short.Parse(item1[4].ToString());
                    charge2 = (int)short.Parse(item2[4].ToString());

                    

                    switch (quality.whichQuality)
                    {
                        case Quality.QualityType.Xc2:
                            {
                                if (PDVersionUsed == PDVersion.PD14AndPrevious)
                                    qualityLevel = 2;
                                else
                                    qualityLevel = 1; // because for PD2.0 the query does not even look for the second match

                                chargeN = charge2;
                                itemN = (object[])item2.Clone();
                                break;
                            }
                        case Quality.QualityType.Xc3:
                            {
                                qualityLevel = 3;
                                if (i < mainList.Count - 2)
                                {
                                    item3 = (object[])mainList[i + 2];
                                    charge3 = (int)short.Parse(item3[4].ToString());
                                    chargeN = charge3;
                                    itemN = (object[])item3.Clone();
                                }
                                break;
                            }
                        case Quality.QualityType.Xc4:
                            {
                                qualityLevel = 4;
                                if (i < mainList.Count - 3)
                                {
                                    item4 = (object[])mainList[i + 3];
                                    charge4 = (int)short.Parse(item4[4].ToString());
                                    chargeN = charge4;
                                    itemN = (object[])item4.Clone();
                                }
                                break;
                            }
                        case Quality.QualityType.XcAveraged:
                            {
                                qualityLevel = 4;
                                if (i < mainList.Count - 3)
                                {
                                    item3 = (object[])mainList[i + 2];
                                    charge3 = (int)short.Parse(item3[4].ToString());
                                    item4 = (object[])mainList[i + 3];
                                    charge4 = (int)short.Parse(item4[4].ToString());
                                    chargeN = charge4;
                                    itemN = (object[])item4.Clone();
                                }
                                break;
                            }
                        case Quality.QualityType.FirstOnly:
                            {
                                qualityLevel = 1; // only the first one is used
                                chargeN = charge1;
                                itemN = (object[])item1.Clone();
                                break;
                            }
                    }

                    if (charge1 == chargeN) // otherwise they get mixed, nightmare!
                    {
                        // the ifWhite
                        if ((int)item1[2] != workingScan || charge1 != workingCharge || item1[1].ToString()!= workingRaw) // so, we are not getting 3rd, 4th and other candidates, only 1st and 2nd
                        {
                            // the ifBlack
                            if ((int)item1[2] == (int)itemN[2]) // firstScan1 == firstScan2, then we have both for Xcorr1 and Xcorr2
                            {
                                workingScan = (int)item1[2]; // to make sure we do not continue working with other candidates of this spectrum
                                workingCharge = charge1;
                                workingRaw = item1[1].ToString();
                                OutData currOut = new OutData(whichDB);

                                int peptideId = (int)item1[0];
                                currOut.RAWFile = Path.GetFileName(item1[1].ToString());
                                currOut.FirstScan = (int)item1[2];

                                currOut.LastScan = (int)item1[3];
                                currOut.Charge = short.Parse(item1[4].ToString());
                                currOut.Mass = (double)item1[6];
                                currOut.PrecursorMass = (double)item1[6];
                                currOut.Xcorr1 = (float)(double)item1[7];

                                switch (quality.whichQuality)
                                {
                                    case Quality.QualityType.Xc2:
                                        {
                                            currOut.Xcorr2 = (float)(double)item2[7];
                                            currOut.XcorrRandom = currOut.Xcorr2;
                                            currOut.DeltaCn = (currOut.Xcorr1Search - currOut.Xcorr2Search) / currOut.Xcorr1Search;
                                            break;
                                        }
                                    case Quality.QualityType.Xc3:
                                        {
                                            currOut.Xcorr3 = (float)(double)item3[7];
                                            currOut.XcorrRandom = currOut.Xcorr3;
                                            currOut.DeltaCn = (currOut.Xcorr1Search - currOut.Xcorr3Search) / currOut.Xcorr1Search;
                                            break;
                                        }
                                    case Quality.QualityType.Xc4:
                                        {
                                            currOut.Xcorr4 = (float)(double)item4[7];
                                            currOut.XcorrRandom = currOut.Xcorr4;
                                            currOut.DeltaCn = (currOut.Xcorr1Search - currOut.Xcorr4Search) / currOut.Xcorr1Search;
                                            break;
                                        }
                                    case Quality.QualityType.XcAveraged:
                                        {
                                            currOut.Xcorr2 = (float)(double)item2[7];
                                            currOut.Xcorr3 = (float)(double)item3[7];
                                            currOut.Xcorr4 = (float)(double)item4[7];
                                            currOut.XcorrRandom =
                                                (currOut.Xcorr2 + currOut.Xcorr3 + currOut.Xcorr4) / 3;

                                            // please double check this (should work if it is linear)
                                            currOut.DeltaCn = (currOut.Xcorr1Search -
                                                ((currOut.Xcorr2Search + currOut.Xcorr3Search + currOut.Xcorr4Search) / 3))
                                                / currOut.Xcorr1Search;
                                            break;
                                        }
                                    case Quality.QualityType.FirstOnly:
                                        {
                                            currOut.Xcorr2 = (float)(double)item1[7]; // so, Xcorr2 = Xcorr1 when only Xcorr is used, which is good, as no pRatio is calculated
                                            currOut.XcorrRandom = currOut.Xcorr2;
                                            currOut.DeltaCn = 0; // no deltaCn can be calculated if no second score is given...
                                            break;
                                        }
                                }

                                currOut.retentionTime = (double)item1[8];

                                currOut.FileName = Path.GetFileName(msfFile);
                                currOut.dbType = whichDB;

                                currOut.Sequence = getSequenceWithModifications(modificationsList, // mods from db
                                                        modificationsFound, // the complete list of possible modifications
                                                        peptideId, // # of peptide in modificationsList
                                                        item1[5].ToString()); // sequence with no mods

                                currOut.TheoreticalMass =
                                    (double)Utilities.getNeutralPepMass(currOut.Sequence,
                                    ds, modificationsFound, DA_paramsReader.paramsData.massType.monoisotopic, true)
                                    + pRatio.Properties.Settings.Default.ProtonMass
                                    + NtermStaticModif;

                                double diff = (currOut.TheoreticalMass - currOut.PrecursorMass) / currOut.PrecursorMass * 1e6;
                                string seq = currOut.Sequence;
                                double theor = currOut.TheoreticalMass;
                                double precursor = currOut.PrecursorMass;
                                // if there is a sequence, then get the isoelectric point
                                if (!(currOut.Sequence.Trim() == ""))
                                {
                                    currOut.pI = Isoelectric.calpI(ds, currOut.Sequence);
                                }
                                else { currOut.pI = 0; }

                                currOut.Sp = getSpScore(SpScoreList, peptideId); ;
                                // perhaps also currOut.SpRank is needed! ***

                                Redundance[] allRedundances = new Redundance[0];
                                int FASTAindex = 0;

                                currOut.ProteinDescription = getProteinInfo(proteinInfoList,
                                                                            peptideIdInProteinInfo,
                                                                            peptideId,
                                                                            out FASTAindex,
                                                                            out allRedundances);

                                if (currOut.ProteinDescription == null)
                                {
                                    MessageBox.Show("There was an error while getting a peptide with the following information:\n" +
                                        "sequence: " + currOut.Sequence + ",\n" +
                                        "scan number: " + currOut.FirstScan.ToString() + ",\n" +
                                        "charge: " + currOut.Charge.ToString() + ",\n" +
                                        "rawfile: " + currOut.RAWFile +
                                        "\n\npRatio will stop working now. Please, check your MSF file.");
                                    errorNumber = 1 + 2; // 1 means there was an error, 2 means abort pRatio
                                    return new ArrayList();
                                }

                                currOut.FASTAIndex = FASTAindex;

                                if (allRedundances.Length > 0)
                                {
                                    currOut.Redundances = allRedundances;
                                    currOut.ProteinsWithPeptide = (short)(int)allRedundances.Length;
                                }

                                outsList.Add(currOut);
                                i += qualityLevel - 1; // to skip the next item and then go to the consecutive pair
                                // nothing is skipped if Xcorr is used instead of pRatio, as qualityLevel = 1 -> qualityLevel - 1 = 0
                            }
                        }
                    }
                }
            }

            return outsList;
        }

        private static void removeStaticModsInVariableList(ArrayList modificationsFound, ArrayList staticModifications)
        {
            // remove static modifications which have been included among the variable ones
            for (int i = 0; i < staticModifications.Count; i++)
            {
                Modification statMod = (Modification)staticModifications[i];
                foreach (Modification varMod in modificationsFound)
                {
                    if (statMod.aminoacid == varMod.aminoacid &&
                        statMod.deltaMass == varMod.deltaMass &&
                        !statMod.variable && varMod.variable)
                    {
                        staticModifications.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }

        private static ArrayList getStaticModifications(string msfFile,
                                                    PDVersion PDVersionUsed,
                                                    ref string errorMessages)
        {
            string queryStaticModifications = "select parameterValue "
                            + "from processingNodeParameters "
                            + "where (parameterName like 'StatMod%' or "
                            + "parameterName like 'StaticMod%');";

            string queryAminoAcidModifications = "select modificationName, deltaMass "
                                + "from aminoAcidModifications "
                                + "where aminoacidModificationId = IDENTIFMODIF;";

            string queryGetAminoAcidSymbol = "select oneLetterCode "
                                + "from aminoAcids "
                                + "where aminoAcidId = IDENTIFAA;";
            ArrayList staticModifications = new ArrayList();
            ArrayList staticCodes = askDB(queryStaticModifications, msfFile);

            if (staticCodes == null && PDVersionUsed == PDVersion.PD14AndPrevious)
            {
                errorMessages = "MSF file error.\nPlease check Proteome Discoverer is 1.4 or previous.";
                return null;
            }

            foreach (object[] code in staticCodes)
            {
                Modification staticMod = new Modification();
                string codeString = code[0].ToString();
                string aminoAcidIdString = codeString.Split('#')[0].ToString();

                bool isNumber = true;
                foreach (char aminoChar in aminoAcidIdString)
                {
                    if (aminoChar < '0' || aminoChar > '9')
                    {
                        isNumber = false;
                        break;
                    }
                }

                if (isNumber)
                {
                    int aminoAcidId = int.Parse(aminoAcidIdString);
                    int modificationId = int.Parse(codeString.Split('#')[1].ToString());
                    object[] modificationInfo = (object[])askDB(queryAminoAcidModifications.Replace("IDENTIFMODIF", modificationId.ToString()), msfFile)[0];
                    object[] aminoacidSymbol = (object[])askDB(queryGetAminoAcidSymbol.Replace("IDENTIFAA", aminoAcidId.ToString()), msfFile)[0];
                    staticMod.name = modificationInfo[0].ToString();
                    staticMod.deltaMass = double.Parse(modificationInfo[1].ToString());
                    staticMod.aminoacid = char.Parse(aminoacidSymbol[0].ToString());
                    staticMod.variable = false;
                    staticMod.symbol = (char)0;
                    staticModifications.Add(staticMod);
                }
            }
            return staticModifications;
        }

        private static ArrayList askDBbatches(string query, string msfFile, int totBatches, int numBatch)
        {
            string queryModified = query.Replace("TOTBATCHES", totBatches.ToString()).Replace("NUMBATCH", numBatch.ToString());
            ArrayList mainList = askDB(queryModified, msfFile);
            return mainList;
        }

        private static void updateProgressBar(OutData.databases whichDB,
                                                ToolStripProgressBar _progressBar,
                                                ArrayList mainList,
                                                int i,
                                                int msfFileNumber,
                                                int totMsfFiles)
        {
            int previousProgressBar = _progressBar.Value;
            int isDecoy = 0;
            if (whichDB == OutData.databases.Decoy) isDecoy = 1;
            double progressPart = (double)i / (double)mainList.Count;
            double progressMSFs = (progressPart + (double)msfFileNumber - 1) / (double)totMsfFiles;
            double progressDB = (progressMSFs + isDecoy) / 2;
            int newProgressBarValue = (int)Math.Truncate(progressDB * _progressBar.Maximum);
            if (newProgressBarValue != previousProgressBar)
            {
                _progressBar.Value = newProgressBarValue;
                Application.DoEvents();
            }
        }

        private static string getProteinInfo(ArrayList proteinInfoList,
                                            ArrayList peptideIdInfoInProtein,
                                            int peptideId,
                                            out int FASTAindex,
                                            out Redundance[] allRedundances)
        {
            ArrayList redundancesList = new ArrayList();
            OutData provOut = new OutData(OutData.databases.Decoy); // it does not matter which db is here

            double start = getStartingIndex(peptideIdInfoInProtein, peptideId, true);

            if (start == -1)
            {
                FASTAindex = 0;
                allRedundances = null;
                return null;
            }

            object[] proteinInfoRow = (object[])proteinInfoList[(int)start];
            int pepIdFromProteinInfo = (int)proteinInfoRow[0];
            while (pepIdFromProteinInfo == peptideId)
            {
                if (provOut.FASTAIndex > 0) // this means the first data has already been taken
                {
                    Redundance currRed = new Redundance();
                    currRed.FASTAIndex = (int)proteinInfoRow[2]; // proteinId within the msf
                    currRed.FASTAProteinDescription = proteinInfoRow[3].ToString();
                    redundancesList.Add(currRed);
                }
                else // main protein description is still empty
                {
                    provOut.FASTAIndex = (int)proteinInfoRow[2];
                    provOut.ProteinDescription = proteinInfoRow[3].ToString();
                }

                start++;
                if (start >= proteinInfoList.Count) break;

                proteinInfoRow = (object[])proteinInfoList[(int)start];
                pepIdFromProteinInfo = (int)proteinInfoRow[0];
            }

            allRedundances = new Redundance[0];
            if (redundancesList.Count > 0)
            {
                allRedundances = new Redundance[redundancesList.Count];
                for (int j = 0; j < redundancesList.Count; j++)
                    allRedundances[j] = (Redundance)redundancesList[j];
            }

            FASTAindex = provOut.FASTAIndex;
            return provOut.ProteinDescription;
        }

        private static double getStartingIndex(ArrayList peptideIdInfoInProtein,
                                                int peptideId,
                                                bool exactMatch)
        {
            // if exactMatch = true, the -1 is returned if none of the elements
            // within peptideIdInfoInProtein do no contain peptideId
            double start = (Math.Truncate(((double)peptideIdInfoInProtein.Count) / 2));
            double step = start;
            while (int.Parse(peptideIdInfoInProtein[(int)start].ToString()) != peptideId)
            {
                step = step / 2;
                if (step == 0) break;
                if (int.Parse(peptideIdInfoInProtein[(int)start].ToString()) > peptideId) start -= step;
                else start += step;
                if ((int)start >= peptideIdInfoInProtein.Count)
                {
                    start--;
                    break;
                }
            }

            start = Math.Truncate(start);
            
            // to fix some rare cases when step becomes zero before than Math.Truncate(start) goes to the second last element of the list
            if (step == 0.0)
                if ((int)start == peptideIdInfoInProtein.Count - 2)
                    if (int.Parse(peptideIdInfoInProtein[(int)start + 1].ToString()) == peptideId)
                        start++;

            // if they are repeated, then it gives the first one
            if (int.Parse(peptideIdInfoInProtein[(int)start].ToString()) == peptideId)
            {
                while (start >= 0 && int.Parse(peptideIdInfoInProtein[(int)start].ToString()) == peptideId)
                    start--;

                start++;
            }

            if ((int)start >= peptideIdInfoInProtein.Count) start = peptideIdInfoInProtein.Count - 1;
            if ((int)start < 0) start = 0;

            if (exactMatch && int.Parse(peptideIdInfoInProtein[(int)start].ToString()) != peptideId)
                start = -1;

            return start;
        }

        private static float getSpScore(ArrayList SpScoreList, int peptideId)
        {
            float SpScore = 0;
            foreach (object[] SpScoreRow in SpScoreList)
            {
                if ((int)SpScoreRow[0] == peptideId) // peptideId is the same
                {
                    SpScore = (float)(double)SpScoreRow[1];
                }
            }
            return SpScore;
        }

        private static string getSequenceWithModifications(ArrayList modificationsList,
                                                            ArrayList modificationsFound,
                                                            int peptideId,
                                                            string workingSequence)
        {
            // this method converts sequences to the old Sequest style
            // (preceding and posterior residues for each peptide are not included)
            foreach (object[] modificationsRow in modificationsList)
            {
                if ((int)modificationsRow[0] == peptideId) // peptideId is the same
                {
                    string pepSequence = modificationsRow[3].ToString();

                    // following line is important in case more than one modification is
                    // in the same peptide
                    ArrayList positionsOfPreviousMods = getPositionsOfPreviousModsWithAnyChar(workingSequence);

                    int modPosition = (int)modificationsRow[2];

                    Modification currentModification = getCurrentModificationWithoutSymbol(modificationsRow);

                    bool modificationWasInList = false;

                    foreach (Modification prevModification in modificationsFound)
                    {
                        if (prevModification.variable)
                        {
                            // if the modification is the same
                            // ... then get the modification char assigned to it
                            if (prevModification.aminoacid == currentModification.aminoacid
                                && prevModification.deltaMass == currentModification.deltaMass)
                            {
                                currentModification = prevModification; // copies the stated modification symbol and name
                                modificationWasInList = true;
                                break;
                            }
                        }
                    }

                    string newSequence = workingSequence;
                    if (modificationWasInList)
                    {
                        // inserts the modification char into the sequence
                        int previousMods = getNumberOfPreviousMods(positionsOfPreviousMods, modPosition);

                        newSequence = newSequence.Insert(modPosition + previousMods + 1, currentModification.symbol.ToString());
                    }
                    else
                    {
                        // in this case, an error should be the output
                        newSequence = pepSequence;
                    }

                    workingSequence = newSequence;
                }
            }
            return workingSequence;
        }

        private static int getNumberOfPreviousMods(ArrayList positionsOfPreviousMods, int modPosition)
        {
            int previousMods = 0;
            foreach (int pos in positionsOfPreviousMods)
            {
                if (pos <= modPosition + positionsOfPreviousMods.Count) previousMods++;
            }
            return previousMods;
        }

        private static ArrayList getPositionsOfPreviousModsWithAnyChar(string previousSequence)
        {
            // returns an arraylist including the zero based position of any
            // character that is not a letter or a dot
            ArrayList positionsOfPreviousMods = new ArrayList();

            char[] allChars = previousSequence.ToUpper().ToCharArray();
            for (int i = 0; i < allChars.Length; i++)
            {
                if (allChars[i] != '.' &&
                    (allChars[i] < 'A' || allChars[i] > 'Z'))
                {
                    positionsOfPreviousMods.Add(i);
                }
            }

            return positionsOfPreviousMods;
        }

        private static ArrayList getPosotionsOfPreviousMods(string previousSequence)
        {
            ArrayList positionsOfPreviousMods = new ArrayList();
            char[] possibleMods = charsForModsList();
            string sequenceWithMods = previousSequence;
            foreach (char mod in possibleMods)
            {
                while (sequenceWithMods.IndexOf(mod) > -1)
                {
                    int positionOfMod = sequenceWithMods.IndexOf(mod);

                    if (positionOfMod > -1)
                    {
                        positionsOfPreviousMods.Add(positionOfMod - 1);
                        sequenceWithMods = sequenceWithMods.Remove(positionOfMod, 1);
                    }
                }
            }
            return positionsOfPreviousMods;
        }

        private static ArrayList getModifications(ArrayList modifications, string modifsFile)
        {
            DataSet modis = new DataSet();
            modis.ReadXmlSchema(pRatio.Properties.Settings.Default.modifications_Schema);
            try
            {
                modis.ReadXml(modifsFile);
            }
            catch
            {
                // this means there is no file, so it will be created
            }

            DataView dv = modis.Tables[1].DefaultView;

            foreach (DataRow row in dv.Table.Rows)
            {
                Modification newMod = new Modification();
                newMod.aminoacid = row["aminoacid"].ToString().ToCharArray()[0];
                newMod.deltaMass = double.Parse(row["weight"].ToString());
                newMod.symbol = row["symbol"].ToString().ToCharArray()[0];
                newMod.name = (string)row["name"];
                newMod.variable = true;

                // ***check that symbol for old modifications is replaced by symbol in the schema
                foreach (Modification oldMod in modifications)
                {
                    if (oldMod.variable)
                    {
                        if (oldMod.aminoacid == newMod.aminoacid &&
                            oldMod.deltaMass == newMod.deltaMass)
                        {
                            modifications.Remove(oldMod);
                            break;
                        }
                    }
                }

                modifications.Add(newMod);
            }
            
            return modifications;
        }

        public static ArrayList extractModifications(ArrayList modificationsFromMSF,
                                                        string modificationFile)
        {
            ArrayList previousMods = getModifications(new ArrayList(), modificationFile);

            if (modificationFile.Length > 0)
            {
                char[] charsForMods = charsForModsList();
                int nextCharToUse = getNextUnusedChar(previousMods, charsForMods, -1); ;

                foreach (object[] modification in modificationsFromMSF)
                {
                    //if (nextCharToUse > charsForMods.Length)
                    //{
                    //    return null; // too many modifications
                    //    // this should report an error with a messagebox
                    //}
                    Modification currentModification = getCurrentModificationWithoutSymbol(modification);
                    bool alreadyPresent = modificationIsAlreadyPresent(previousMods, currentModification);

                    if (!alreadyPresent)
                    {
                        currentModification.symbol = charsForMods[nextCharToUse];
                        nextCharToUse = getNextUnusedChar(previousMods, charsForMods, nextCharToUse);

                        previousMods.Add(currentModification);
                        writeNewModificationsXML(modificationFile, previousMods);
                    }
                }
            }

            #region old code (left here just in case)
            //    if (modifications.Count == 0)
            //    {
            //        modifications.Add(currentModification);
            //        nextCharToUse++;
            //    }
            //    else
            //    {
            //        bool modificationAlreadyAdded = false;

            //        foreach (object[] modificationToCheck in modifications)
            //        {
            //            if (modificationToCheck[0].ToString() == currentModification[0].ToString()
            //                && modificationToCheck[1].ToString() == currentModification[1].ToString()
            //                && double.Parse(modificationToCheck[2].ToString()) == double.Parse(currentModification[2].ToString()))
            //            {
            //                modificationAlreadyAdded = true;
            //                break;
            //            }
            //        }

            //        if (!modificationAlreadyAdded)
            //        {
            //            modifications.Add(currentModification);
            //            nextCharToUse++;
            //        }
            //    }
            //}
            #endregion

            return previousMods;
        }

        private static bool modificationIsAlreadyPresent(ArrayList previousMods, Modification currentModification)
        {
            bool alreadyPresent = false;
            foreach (Modification oldModification in previousMods)
            {
                if (oldModification.variable)
                {
                    if (oldModification.aminoacid == currentModification.aminoacid &&
                        oldModification.deltaMass == currentModification.deltaMass)
                    {
                        alreadyPresent = true;
                        break;
                    }
                }
            }
            return alreadyPresent;
        }

        private static int getNextUnusedChar(ArrayList previousMods, char[] charsForMods, int nextCharToUse)
        {
            bool getNext = true;
            while (getNext)
            {
                nextCharToUse++;
                bool alreadyUsed = false;
                foreach (Modification mod in previousMods)
                {
                    if (charsForMods[nextCharToUse] == mod.symbol)
                    {
                        alreadyUsed = true;
                        break;
                    }
                }

                getNext = alreadyUsed;
            }
            return nextCharToUse;
        }

        /* former method getModifications
        private static ArrayList getModifications(ArrayList modificationsList, string modifsFile)
        {
            ArrayList modifications = getPreviousMods(modifsFile);

            // I am not sure about the order SEQUEST uses from the 6th char
            // however, in future releases, this should be customisable using
            // the ModDefs file
            char[] charsForMods = charsForModsList();

            int nextCharToUse = 0;

            foreach (object[] modification in modificationsList)
            {
                //if (nextCharToUse > charsForMods.Length)
                //{
                //    return null; // too many modifications
                //    // this should report an error with a messagebox
                //}
                object[] currentModification = getCurrentModification(modification);
                currentModification[3] = charsForMods[nextCharToUse];

                if (modifications.Count == 0)
                {
                    modifications.Add(currentModification);
                    nextCharToUse++;
                }
                else
                {
                    bool modificationAlreadyAdded = false;

                    foreach (object[] modificationToCheck in modifications)
                    {
                        if (modificationToCheck[0].ToString() == currentModification[0].ToString()
                            && modificationToCheck[1].ToString() == currentModification[1].ToString()
                            && double.Parse(modificationToCheck[2].ToString()) == double.Parse(currentModification[2].ToString()))
                        {
                            modificationAlreadyAdded = true;
                            break;
                        }
                    }

                    if (!modificationAlreadyAdded)
                    {
                        modifications.Add(currentModification);
                        nextCharToUse++;
                    }
                }
            }

            writeNewModificationsXML(modifsFile, modifications);

            return modifications;
        }
        */

        private static void writeNewModificationsXML(string modifsFile, ArrayList modifications)
        {
            DataSet modSet = new DataSet("soso");
            modSet.ReadXmlSchema(pRatio.Properties.Settings.Default.modifications_Schema);
            DataView modView = modSet.Tables["modification"].DefaultView;
            DataView modifSetView = modSet.Tables["modifSet"].DefaultView;
            modifSetView.Table.Rows.Add(0);

            int primKey = 0;
            foreach (Modification modification in modifications)
            {
                modView.AddNew();
                DataRow row = (DataRow)modView[primKey].Row;
                row["aminoacid"] = modification.aminoacid;
                row["weight"] = modification.deltaMass;
                row["symbol"] = modification.symbol;
                row["name"] = modification.name;
                row["modifSet_Id"] = 0;
                
                modView.Table.Rows.Add(row);
                primKey++;
            }

            modSet.WriteXml(modifsFile);
        }


        private static void writeNewModifications(string modifsFile, ArrayList modifications)
        {
            if (modifications.Count > 0)
            {
                StreamWriter writer = new StreamWriter(modifsFile);
                foreach (object[] newModification in modifications)
                {
                    string line = "";
                    for (int i = 0; i < newModification.Length; i++)
                    {
                        line += newModification[i].ToString();
                        if (i < newModification.Length - 1)
                            line += "\t";
                    }
                    writer.WriteLine(line);
                }

                writer.Close();
            }
        }

        private static ArrayList getPreviousMods(string modifsFile)
        {
            ArrayList modifications = new ArrayList();

            if (File.Exists(modifsFile))
            {
                StreamReader reader = new StreamReader(modifsFile);
                while (!reader.EndOfStream)
                {
                    try
                    {
                        object[] savedMod = reader.ReadLine().Split('\t');
                        modifications.Add(savedMod);
                    }
                    catch { }
                }

                reader.Close();
            }

            return modifications;
        }

        private static char[] charsForModsList()
        {
            char[] charsForMods = new char[30];

            charsForMods[0] = '*';
            charsForMods[1] = '#';
            charsForMods[2] = '@';
            charsForMods[3] = '^';
            charsForMods[4] = '-';
            charsForMods[5] = '$';
            charsForMods[6] = '{';
            charsForMods[7] = '_';
            charsForMods[8] = '!';
            charsForMods[9] = '=';
            charsForMods[10] = '|';
            charsForMods[11] = '(';
            charsForMods[12] = ')';
            charsForMods[13] = '{';
            charsForMods[14] = '}';
            charsForMods[15] = ']';
            charsForMods[16] = '[';
            charsForMods[17] = ':';
            charsForMods[18] = ';';
            charsForMods[19] = '+';
            charsForMods[20] = '0';
            charsForMods[21] = '1';
            charsForMods[22] = '2';
            charsForMods[23] = '3';
            charsForMods[24] = '4';
            charsForMods[25] = '5';
            charsForMods[26] = '6';
            charsForMods[27] = '7';
            charsForMods[28] = '8';
            charsForMods[29] = '9';

            return charsForMods;
        }

        private static Modification getCurrentModificationWithoutSymbol(object[] modification)
        {
            // note this does not give back the modification symbol
            // it has to be added later
            Modification currentModification = new Modification();
            string pepSequence = modification[3].ToString();
            int modPosition = (int)modification[2];

            currentModification.name = modification[4].ToString(); // modification name
            currentModification.aminoacid = pepSequence.Substring(modPosition, 1).ToCharArray()[0]; // aa modified
            currentModification.deltaMass = (double)modification[5]; // delta mass
            return currentModification;
        }

        /* obsolete methods
         * they were used with the SQLite library
         * using System.Data.SQLite
         * and downloaded from sqlite.phxsoftware.com
         * 
        private static ArrayList askDB(string query, string msfFile)
        {
            ArrayList dataFromDB = new ArrayList();
            string connexionString = string.Format("Data Source={0}", msfFile);
            SQLiteConnection connexion = new SQLiteConnection(connexionString);
            connexion.Open();

            SQLiteCommand command = new SQLiteCommand(query, connexion);
            SQLiteDataReader data = command.ExecuteReader();
            int columnNumber = data.FieldCount;

            while (data.Read())
            {
                object[] currentRow = new object[columnNumber];
                data.GetValues(currentRow);
                dataFromDB.Add(currentRow);
            }

            connexion.Close();
            return dataFromDB;
        }

         * 
         * obsolete method to check the velocity of the previous SQLite reader
         * 
        private static double askDB_test(string query, string msfFile, int maxItems)
        {
            ArrayList dataFromDB = new ArrayList();
            string connexionString = string.Format("Data Source={0}", msfFile);
            SQLiteConnection connexion = new SQLiteConnection(connexionString);
            connexion.Open();

            SQLiteCommand command = new SQLiteCommand(query, connexion);

            DateTime muchoAntes = DateTime.Now;

            SQLiteDataReader data = command.ExecuteReader();
            int columnNumber = data.FieldCount;

            DateTime antes = DateTime.Now;

            while (data.Read() && dataFromDB.Count < maxItems)
            {
                object[] currentRow = new object[columnNumber];
                data.GetValues(currentRow);
                dataFromDB.Add(currentRow);
            }

            DateTime despues = DateTime.Now;

            connexion.Close();

            TimeSpan tiempoHacerConsulta = antes - muchoAntes;
            TimeSpan tiempoTransferirDatos = despues - antes;
            TimeSpan tiempoTotal = despues - muchoAntes;
            double hacerConsultaPorFila = tiempoHacerConsulta.TotalSeconds / dataFromDB.Count;
            double transferirDatosPorFila = tiempoTransferirDatos.TotalSeconds / dataFromDB.Count;
            double tiempoTotalPorFila = tiempoTotal.TotalSeconds / dataFromDB.Count;

            return tiempoTotalPorFila;
        }

        */

        private static ArrayList askDB(string query, string msfFile)
        {
            ArrayList dataFromDB = new ArrayList();
            string connexionString = string.Format("Version=3,uri=file:{0}", msfFile);
            SqliteConnection connexion = new SqliteConnection(connexionString);
            connexion.Open();

            SqliteCommand command = new SqliteCommand(query, connexion);
            SqliteDataReader data = command.ExecuteReader();
            int columnNumber;

            try
            {
                columnNumber = data.FieldCount;
            }
            catch
            {
                return null;
            }

            while (data.Read())
            {
                object[] currentRow = new object[columnNumber];
                data.GetValues(currentRow);
                dataFromDB.Add(currentRow);
            }

            connexion.Close();

            return dataFromDB;
        }

        // the next method is useful when checking velocity of SQLite db
        //
        private static double askDB_test(string query, string msfFile, int maxItems)
        {
            ArrayList dataFromDB = new ArrayList();
            string connexionString = string.Format("Version=3,uri=file:{0}", msfFile);
            SqliteConnection connexion = new SqliteConnection(connexionString);
            connexion.Open();

            SqliteCommand command = new SqliteCommand(query, connexion);

            DateTime muchoAntes = DateTime.Now;

            SqliteDataReader data = command.ExecuteReader();
            int columnNumber = data.FieldCount;

            DateTime antes = DateTime.Now;

            while (data.Read() && dataFromDB.Count < maxItems)
            {
                object[] currentRow = new object[columnNumber];
                data.GetValues(currentRow);
                dataFromDB.Add(currentRow);
            }

            DateTime despues = DateTime.Now;

            connexion.Close();

            TimeSpan tiempoHacerConsulta = antes - muchoAntes;
            TimeSpan tiempoTransferirDatos = despues - antes;
            TimeSpan tiempoTotal = despues - muchoAntes;
            double hacerConsultaPorFila = tiempoHacerConsulta.TotalSeconds / dataFromDB.Count;
            double transferirDatosPorFila = tiempoTransferirDatos.TotalSeconds / dataFromDB.Count;
            double tiempoTotalPorFila = tiempoTotal.TotalSeconds / dataFromDB.Count;

            return tiempoTotalPorFila;
        }

        //private static ArrayList readOutsSRF(string[] srfFiles, DA_fileReader.Quality quality, OutData.databases whichDB, DataSet ds)
        //{

        //    ArrayList outsList = new ArrayList();


        //    for (int iFile = 0; iFile <= srfFiles.GetUpperBound(0); iFile++)
        //    {

        //        Unified _obUni = new Unified();
        //        Out _obOut;
        //        OutHeader _OutH;
        //        PeptideList _obPeptides;
        //        ProteinList _obProteins;
        //        SRFHeader _obSRFHeader;

        //        _obUni.SourceSRFFilename = srfFiles[iFile].Trim();

        //        _obSRFHeader = _obUni.GetSRFHeader();

        //        string sh_Comment = _obSRFHeader.Comment;
        //        string sh_Database = _obSRFHeader.Database;
        //        string sh_Enzyme = _obSRFHeader.Enzyme;
        //        int sh_FirstScan = _obSRFHeader.FirstScan;
        //        string sh_Instrument = _obSRFHeader.Instrument;
        //        float sh_IntensityThreshold = _obSRFHeader.IntensityThreshold;
        //        int sh_LastScan = _obSRFHeader.LastScan;
        //        short sh_MassType = _obSRFHeader.MassType;
        //        short sh_MassUnits = _obSRFHeader.MassUnits;
        //        float sh_MaxMass = _obSRFHeader.MaxMass;
        //        float sh_MinMass = _obSRFHeader.MinMass;
        //        string sh_Modifications = _obSRFHeader.Modifications;
        //        float sh_MSMSTolerance = _obSRFHeader.MSMSTolerance;
        //        float sh_MSTolerance = _obSRFHeader.MSTolerance;
        //        int sh_NumberOfSearchEntries = _obSRFHeader.NumberOfSearchEntries;
        //        string sh_Operator = _obSRFHeader.Operator;
        //        string sh_ParamsFile = _obSRFHeader.ParamsFile;
        //        string sh_RAWFile = _obSRFHeader.RAWFile;
        //        float sh_SNThreshold = _obSRFHeader.SNThreshold;
        //        short sh_SRFVersion = _obSRFHeader.SRFVersion;

        //        // fixed bug here: the las candidate of each file was not being considered
        //        // previous line was:
        //        // for (int i = 0; i <= _obUni.NumberOfDtaEntries - 2; i++)
        //        for (int i = 0; i <= _obUni.NumberOfDtaEntries - 1; i++)
        //        {
        //            Application.DoEvents();
        //            _obOut = (Out)_obUni.GetOutData(i);

        //            _obOut = _obUni.GetOutData(i);
        //            _OutH = _obOut.GetOutHeader();
        //            _obPeptides = _obOut.GetPeptideList();
        //            _obProteins = _obOut.GetProteinList();

        //            if (_obPeptides.Count > 3)
        //            {
        //                OutData currOut = new OutData(whichDB);

        //                currOut.FirstScan = _OutH.FirstScan;
        //                currOut.LastScan = _OutH.LastScan;
        //                currOut.Charge = _OutH.Charge;
        //                currOut.LowestSpScore = _OutH.LowestSpScore;
        //                currOut.PrecursorMass = _OutH.PrecursorMass;
        //                currOut.TotalIntensity = _OutH.TotalIntensity;

        //                int uni_NumberOfDtaEntries = _obUni.NumberOfDtaEntries;

        //                currOut.FileName = _obUni.SourceSRFFilename;
        //                currOut.RAWFile = _obSRFHeader.RAWFile;
        //                currOut.Sequence = _obPeptides[1].Sequence;
        //                if (!(currOut.Sequence.Trim() == ""))
        //                {
        //                    currOut.pI = Isoelectric.calpI(ds, currOut.Sequence);
        //                }
        //                else { currOut.pI = 0; }

        //                currOut.FASTAIndex = _obPeptides[1].FASTAIndex;
        //                currOut.DeltaCn = _obPeptides[2].DeltaCn;
        //                currOut.IonsCompared = _obPeptides[1].IonsCompared;
        //                currOut.IonsMatched = _obPeptides[1].IonsMatched;
        //                currOut.Mass = _obPeptides[1].Mass;
        //                currOut.MatchRank = _obPeptides[1].MatchRank;
        //                currOut.ProteinDescription = _obPeptides[1].ProteinDescription;
        //                currOut.ProteinMass = _obPeptides[1].ProteinMass;
        //                currOut.ProteinsWithPeptide = _obPeptides[1].ProteinsWithPeptide;
        //                currOut.Sp = _obPeptides[1].Sp;
        //                currOut.SpRank = _obPeptides[1].SpRank;
        //                currOut.Xcorr1 = _obPeptides[1].XCorr;
        //                currOut.Xcorr2 = _obPeptides[2].XCorr;
        //                currOut.Xcorr3 = _obPeptides[3].XCorr;
        //                currOut.Xcorr4 = _obPeptides[4].XCorr;

        //                switch (quality.whichQuality)
        //                {
        //                    case Quality.QualityType.Xc2:
        //                        {
        //                            currOut.XcorrRandom = currOut.Xcorr2;
        //                            break;
        //                        }
        //                    case Quality.QualityType.Xc3:
        //                        {
        //                            currOut.XcorrRandom = currOut.Xcorr3;
        //                            break;
        //                        }
        //                    case Quality.QualityType.Xc4:
        //                        {
        //                            currOut.XcorrRandom = currOut.Xcorr4;
        //                            break;
        //                        }
        //                    case Quality.QualityType.XcAveraged:
        //                        {
        //                            currOut.XcorrRandom = (currOut.Xcorr2 + currOut.Xcorr3 + currOut.Xcorr4) / 3;
        //                            break;
        //                        }
        //                }

        //                if (_obProteins.Count > 0 & _obPeptides[1].ProteinsWithPeptide > 0)
        //                {
        //                    Redundance[] red = new Redundance[_obPeptides[1].ProteinsWithPeptide];

        //                    int k = 0;
        //                    for (int j = 1; j < _obProteins.Count; j++)
        //                    {
        //                        Redundance r1 = new Redundance();

        //                        if (_obProteins[j].MatchRank == 1)
        //                        {
        //                            r1.FASTAIndex = _obProteins[j].FASTAIndex;
        //                            r1.FASTAProteinDescription = _obProteins[j].FASTAProteinDescription;
        //                            red[k] = r1;
        //                            k++;
        //                        }
        //                    }
        //                    currOut.Redundances = (Redundance[])red.Clone();
        //                    red = null;
        //                }
        //                else
        //                {
        //                    currOut.Redundances = null;
        //                }

        //                outsList.Add(currOut);
        //            }

        //        }


        //        _obProteins = null;
        //        _obPeptides = null;
        //        _OutH = null;
        //        _obOut = null;
        //        _obUni = null;


        //    }

        //    return outsList;
        //}

        private static ArrayList readOutsXML(string[] xmlFiles, DA_fileReader.Quality quality, OutData.databases whichDB, DataSet ds)
        {
            ArrayList outsList = new ArrayList();

            

            for (int iFile = 0; iFile <= xmlFiles.GetUpperBound(0); iFile++)
            {

                DataSet _currXML = new DataSet();

                _currXML.ReadXmlSchema(System.AppDomain.CurrentDomain.BaseDirectory + pRatio.Properties.Settings.Default.bioW33_Schema);
                _currXML.ReadXml(xmlFiles[iFile]);

                
                DataView _peptideMatches = _currXML.Tables["peptide_match"].DefaultView;
                DataView _fileHeaderdv = _currXML.Tables["sequestresults"].DefaultView;
                DataRow _fileHeader = _fileHeaderdv[0].Row;

                int numOfMatches = _peptideMatches.Count;
                for (int i = 0; i < numOfMatches; i++)
                {
                    Application.DoEvents();

                        
                    OutData currOut = new OutData(whichDB);

                    DataRow pepMatch = _peptideMatches[i].Row;


                    currOut.FileName = xmlFiles[iFile];
                    //scans
                    string scans = pepMatch["scan_range"].ToString();
                    string[] scanRange = scans.Split('-');
                    if (scanRange.Length > 0)
                    {
                        currOut.FirstScan = int.Parse(scanRange[0].ToString());                        
                    }
                    if (scanRange.Length > 1)
                    {
                        currOut.LastScan = int.Parse(scanRange[1].ToString());
                    }
                    currOut.Charge = short.Parse(pepMatch["charge"].ToString());
                    currOut.PrecursorMass = (double)pepMatch["input_mass"];
                    currOut.TotalIntensity = (float)pepMatch["intensity"];
                    currOut.ProteinDescription = pepMatch["reference"].ToString();
                    currOut.Sequence = pepMatch["peptide"].ToString();
                    if (!(currOut.Sequence.Trim() == ""))
                    {
                        currOut.pI = Isoelectric.calpI(ds, currOut.Sequence);
                    }
                    else { currOut.pI = 0; }

                    currOut.DeltaCn = (float)pepMatch["deltacn"];
                    currOut.Mass = (double)pepMatch["actual_mass"];
                    //Ions matched/Ions compared
                    string ionsMatched_Compared = pepMatch["ions"].ToString();
                    string[] ionsMC = ionsMatched_Compared.Split('/');
                    if (ionsMC.Length == 2)
                    {
                        currOut.IonsCompared = short.Parse(ionsMC[1]);
                        currOut.IonsMatched = short.Parse(ionsMC[0]);
                    }

                    currOut.ProteinMass = 0.0F;
                    currOut.Sp = (float)pepMatch["sp"];
                    currOut.SpRank = (short)pepMatch["rsp"];
                    currOut.ProteinsWithPeptide = 0;
                    currOut.FASTAIndex = 0;
                    currOut.MatchRank = 1;
                    currOut.LowestSpScore = 0.0F;


                    //RAW file 
                    //WARNING: the raw file is not reported at the BioWorks 3.3.1 SP1 xml files! 
                    //         It must be written manually in the xml file, in the field <origfilename/> 

                    string orFileName = _fileHeader["origfilepath"].ToString();
                    string[] orFileName_sp = orFileName.Split('\\');
                    currOut.RAWFile = orFileName_sp[orFileName_sp.Length - 1];
                   
            
                    currOut.Xcorr1 = (float)pepMatch["xcorr"];
                    currOut.Xcorr2 = currOut.Xcorr1Search * (1 - currOut.DeltaCn);
                    currOut.Xcorr3 = 0.0F;  //WARNING:  xml files only report Xcorr1 and deltac2
                    currOut.Xcorr4 = 0.0F;  //WARNING:  xml files only report Xcorr1 and deltac2
                

                    switch (quality.whichQuality)
                    {
                        case Quality.QualityType.Xc2:
                            {
                                currOut.XcorrRandom = currOut.Xcorr2;
                                break;
                            }
                        case Quality.QualityType.Xc3:
                            {
                                currOut.XcorrRandom = currOut.Xcorr3;
                                break;
                            }
                        case Quality.QualityType.Xc4:
                            {
                                currOut.XcorrRandom = currOut.Xcorr4;
                                break;
                            }
                        case Quality.QualityType.XcAveraged:
                            {
                                currOut.XcorrRandom = (currOut.Xcorr2 + currOut.Xcorr3 + currOut.Xcorr4) / 3;
                                break;
                            }
                    }

                 
                    currOut.Redundances = null;
                   
                    outsList.Add(currOut);
                    

                }

                _currXML = null;

            }

            return outsList;

 

        }
        
        //public static SrfData readCommonDataSRF(string srfFile,
        //                                        OutData.databases whichDB)
        //{

        //    SrfData sd = new SrfData();

        //    Unified _obUni = new Unified();
        //    SRFHeader _obSRFHeader;

        //    _obUni.SourceSRFFilename = srfFile.Trim();
        //    _obSRFHeader = _obUni.GetSRFHeader();

        //    sd.Comment = _obSRFHeader.Comment;
        //    sd.Database = _obSRFHeader.Database;
        //    sd.Enzyme = _obSRFHeader.Enzyme;
        //    sd.FirstScan = _obSRFHeader.FirstScan;                
        //    sd.Instrument = _obSRFHeader.Instrument;               
        //    sd.IntensityThreshold = _obSRFHeader.IntensityThreshold;       
        //    sd.LastScan = _obSRFHeader.LastScan;                 
        //    sd.MassType = _obSRFHeader.MassType;                 
        //    sd.MassUnits = _obSRFHeader.MassUnits;                
        //    sd.MaxMass = _obSRFHeader.MaxMass;                  
        //    sd.MinMass = _obSRFHeader.MinMass;                  
        //    sd.Modifications = _obSRFHeader.Modifications;            
        //    sd.MSMSTolerance = _obSRFHeader.MSMSTolerance;            
        //    sd.MSTolerance = _obSRFHeader.MSTolerance;              
        //    sd.NumberOfSearchEntries = _obSRFHeader.NumberOfSearchEntries;    
        //    sd.Operator = _obSRFHeader.Operator;                 
        //    sd.ParamsFile = _obSRFHeader.ParamsFile;               
        //    sd.RAWFile = _obSRFHeader.RAWFile;                  
        //    sd.SNThreshold = _obSRFHeader.SNThreshold;              
        //    sd.SRFVersion = _obSRFHeader.SRFVersion;               

        //    return sd;
        //}



        //public static SrfData readCommonDataXML(string xmlFile,
        //                                        OutData.databases whichDB)
        //{
        //    SrfData sd = new SrfData();

        //    Unified _obUni = new Unified();
        //    SRFHeader _obSRFHeader;


        //    _obUni.SourceSRFFilename = xmlFile.Trim();
        //    _obSRFHeader = _obUni.GetSRFHeader();

        //    sd.Comment = _obSRFHeader.Comment;
        //    sd.Database = _obSRFHeader.Database;
        //    sd.Enzyme = _obSRFHeader.Enzyme;
        //    sd.FirstScan = _obSRFHeader.FirstScan;
        //    sd.Instrument = _obSRFHeader.Instrument;
        //    sd.IntensityThreshold = _obSRFHeader.IntensityThreshold;
        //    sd.LastScan = _obSRFHeader.LastScan;
        //    sd.MassType = _obSRFHeader.MassType;
        //    sd.MassUnits = _obSRFHeader.MassUnits;
        //    sd.MaxMass = _obSRFHeader.MaxMass;
        //    sd.MinMass = _obSRFHeader.MinMass;
        //    sd.Modifications = _obSRFHeader.Modifications;
        //    sd.MSMSTolerance = _obSRFHeader.MSMSTolerance;
        //    sd.MSTolerance = _obSRFHeader.MSTolerance;
        //    sd.NumberOfSearchEntries = _obSRFHeader.NumberOfSearchEntries;
        //    sd.Operator = _obSRFHeader.Operator;
        //    sd.ParamsFile = _obSRFHeader.ParamsFile;
        //    sd.RAWFile = _obSRFHeader.RAWFile;
        //    sd.SNThreshold = _obSRFHeader.SNThreshold;
        //    sd.SRFVersion = _obSRFHeader.SRFVersion;




        //    return sd;

 
        //}

        public class Quality
        {
            public enum QualityType
            {
                Xc2,
                Xc3,
                Xc4,
                XcAveraged,
                FirstOnly
            }

            public DA_fileReader.Quality.QualityType whichQuality
            {
                get { return whichQualityVal; }
                set { whichQualityVal = value; }
            }

            private DA_fileReader.Quality.QualityType whichQualityVal;
        }

        public class XcorrSelection
        {
            public enum XCorrType
            {
                regular,
                normalized
            }

            public DA_fileReader.XcorrSelection.XCorrType whichXcorr
            {
                get { return whichXcorrVal; }
                set { whichXcorrVal = value; }
            }

            private DA_fileReader.XcorrSelection.XCorrType whichXcorrVal;

        }

        public enum fileType
        {
            SRF,
            MSF,
            XML
        }

        public enum PDVersion
        {
            PD14AndPrevious,
            PD20
        }

    }

    public class DA_txtReader
    {


        public static ArrayList readTXT(string[] txtFiles,
                                        OutData.databases whichDB)
        {

            ArrayList outsList = new ArrayList();


            foreach (string file in txtFiles)
            {
                try
                {
                    StreamReader sr = new StreamReader(File.OpenRead(file));
                    ArrayList myAL = new ArrayList();

                    try
                    {
                        while (sr.Peek() != -1) //equivalente a feof en C
                        {
                            myAL.Add(sr.ReadLine());
                        }
                    }
                    catch
                    {
                        //Console.WriteLine(" Parse error: " + e);
                    }

                    //int r = myAL.Count;


                    foreach (object o in myAL)
                    {
                        try
                        {
                            OutData sd = new OutData(whichDB);

                            string[] str;

                            str = Regex.Split(o.ToString(), "\t");
                            System.Collections.IEnumerator myEnumerator = str.GetEnumerator();

                            myEnumerator.MoveNext();
                            sd.Charge = short.Parse((string)myEnumerator.Current);
                            myEnumerator.MoveNext();
                            sd.Mass = float.Parse((string)myEnumerator.Current);
                            myEnumerator.MoveNext();
                            sd.Xcorr1 = float.Parse((string)myEnumerator.Current);
                            myEnumerator.MoveNext();
                            sd.Xcorr2 = float.Parse((string)myEnumerator.Current);
                            sd.XcorrRandom = sd.Xcorr2;
                            myEnumerator.MoveNext();
                            sd.RAWFile = myEnumerator.Current.ToString();
                            sd.FileName = myEnumerator.Current.ToString();
                            myEnumerator.MoveNext();
                            sd.FirstScan = int.Parse((string)myEnumerator.Current);
                            
                            sd.dbType = whichDB;

                            outsList.Add(sd);
                        }
                        catch { }
                    }



                }
                catch
                {
                }
            }

            return outsList;
        }

        public class Quality
        {
            public enum QualityType
            {
                Xc2,
                Xc3,
                Xc4,
                XcAveraged
            }

            public DA_txtReader.Quality.QualityType whichQuality
            {
                get { return whichQualityVal; }
                set { whichQualityVal = value; }
            }

            private DA_txtReader.Quality.QualityType whichQualityVal;
        }

    }


}
