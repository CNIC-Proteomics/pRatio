using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using pRatio.Properties;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
//using System.Collections;

namespace pRatio
{
  
    class Utilities
    {

        public static float fpI(float pI,float sigma, float mu)
        {
            //squared function
            return Math.Abs(pI - mu) < sigma ? 1 : 0;
        }

        public class MathAverages
        {
            // Computes the median x~ of a set of values X. O(1) implementation..
            public static float Median(float[] values)
            {
                ArrayList vals = new ArrayList();
                float result;

                for (int i = 0; i <= values.GetUpperBound(0); i++)
                {
                    vals.Add(values[i]);
                }

                vals.Sort();
                
                // Median = (n + 1) ÷ 2th value

                int halfsize = 0;

                int oddeven = values.Length % 2;

                try
                {
                    switch (oddeven)
                    {
                        case 1:
                            halfsize = (values.Length - 1) / 2;
                            result = ((float)vals[halfsize]);
                            break;
                        default:
                            halfsize = (values.Length) / 2;
                            result = ((float)vals[halfsize - 1] + (float)vals[halfsize]) / 2;
                            break;
                    }
                }
                catch
                {
                    result = 7;
                }
 
                return result;
            }

            // Computes the arithmetic mean x-bar of a set of values X. O(n)implementation.
            public static float Mean(float[] values)
            {
                float sum = 0;

                foreach (float value in values)
                {
                    sum += value;
                }

                return sum/(values.Length);
            }
        }


        public class pIfraction
        {

            private string RAWFileNameVal;
            private float muVal;
            private float stDevVal;

            public string RAWFileName
            {
                get { return RAWFileNameVal; }
                set { RAWFileNameVal = value; }
            }

            public float mu
            {
                get { return muVal; }
                set { muVal = value; }
            }

            public float stDev
            {
                get { return stDevVal; }
                set { stDevVal = value; }
            }
        }

        private class Modifications
        {
            public string aminoacid
            { 
                get { return aminoacidVal; } 
                set { aminoacidVal = value; } 
            }
            public bool variable
            { 
                get { return variableVal; } 
                set { variableVal = value; } 
            }
            public float deltaMass
            {
                get { return deltaMassVal; }
                set { deltaMassVal = value; }
            }
            public float finalMass 
            { 
                get{ return finalMassVal; }
                set { finalMassVal = value; }
            }
            public char symbol
            { 
                get { return symbolVal; }
                set { symbolVal = value; } 
            }
                                                                                     
            private string aminoacidVal;
            private bool variableVal;
            private float deltaMassVal;
            private float finalMassVal;
            private char symbolVal;                                             

        }


        public static float getNeutralPepMass(string peptide,
                                                DataSet ds,
                                                ArrayList mods,
                                                DA_paramsReader.paramsData.massType massType,
                                                bool fromMSF)
        {
            float neutralMass = 0.0F;
            
            DataView dv=new DataView(ds.Tables["aminoacid"]);
            string pep = (string)peptide.Clone();

            string[] occ = new string[dv.Count];
            float[] miw = new float[dv.Count];
            float[] aiw = new float[dv.Count];

            for (int i = 0; i < dv.Count; i++)
            {
                occ[i] = dv[i].Row["one_char_code"].ToString();
                miw[i] = float.Parse(dv[i].Row["monoisotopic_incremental_weight"].ToString());
                aiw[i] = float.Parse(dv[i].Row["average_incremental_weight"].ToString());
            }

            //Search variable modifications
            for (int i = 0; i < mods.Count; i++)
            {
                Modifications mod = new Modifications();
                if (fromMSF)
                {
                    Modification modNew = (Modification)mods[i];
                    mod.aminoacid = modNew.aminoacid.ToString();
                    mod.deltaMass = (float)modNew.deltaMass;
                    mod.symbol = modNew.symbol;
                    mod.variable = modNew.variable;
                }
                else
                {
                    mod = (Modifications)mods[i];
                }

                char chMod = mod.symbol;

                string peptideCopy = pep;
                int iSearch = pep.IndexOf(chMod);
                while (iSearch != -1)
                {
                    neutralMass += mod.deltaMass;
                    pep = pep.Remove(iSearch, 1);
                    iSearch = pep.IndexOf(chMod);
                }

                if (chMod == ' ' || chMod == 0)
                {
                    int iFMod = peptideCopy.IndexOf(mod.aminoacid);
                    while (iFMod != -1)
                    {
                        neutralMass += mod.deltaMass;
                        peptideCopy = peptideCopy.Remove(iFMod, 1);
                        iFMod = peptideCopy.IndexOf(mod.aminoacid); 
                    }
                }
            }

            

            for (int i = 0; i <= occ.GetUpperBound(0); i++)
            {
                int iSearch = pep.IndexOf(occ[i]);
                while (iSearch != -1)
                {
                    switch (massType)
                    {
                        case DA_paramsReader.paramsData.massType.average:
                            {
                                neutralMass += aiw[i];
                                break;
                            }
                        case DA_paramsReader.paramsData.massType.monoisotopic:
                            {
                                neutralMass += miw[i];
                                break;
                            }
                    }
                    pep = pep.Remove(iSearch, 1);
                    iSearch = pep.IndexOf(occ[i]);
                }
            }
            
            //Add H2O
            neutralMass += Settings.Default.HydrogenMass * 2 + Settings.Default.OxygenMass;

            return neutralMass;
        }
                                                
        
        private static string getProtein(string proteinDescription)
        {
            string protein = "";

            int firstSep = proteinDescription.IndexOf('|');

            if(firstSep==-1)
            {
                int maxValue = 10 > proteinDescription.Length ? 10 : proteinDescription.Length;
                protein = proteinDescription.Substring(1, maxValue);
            }
            if (firstSep <= 5)
            { 
                string prot=proteinDescription.Substring(firstSep+1,proteinDescription.Length-firstSep-1);
                int secondSep=prot.IndexOf('|');

                if (secondSep == -1)
                {
                    protein = prot;
                }
                else 
                {
                    protein = prot.Substring(0, secondSep);
                }

            }
            if (firstSep > 5)
            {
                protein = proteinDescription.Substring(0, firstSep - 1);
            }
            
            return protein;
        }

        private static int getNumOfMissedCleaveages(string sequence,
                                                      DA_paramsReader.paramsData.enzyme enzyme)
        {
            int missedCleav = 0;

            char[] cutPoints=enzyme.cut.ToCharArray();
            char[] nocutPoints = enzyme.noCut.ToCharArray();

            string seq="";
            switch (enzyme.sense)
            {
                case DA_paramsReader.paramsData.enzyme.senseType.C:
                    {
                        seq = sequence.Substring(0, sequence.Length - 1);
                        break; 
                    }
                case DA_paramsReader.paramsData.enzyme.senseType.N:
                    {
                        seq = sequence.Substring(1, sequence.Length - 1);
                        break;
                    }
            }

            //search for missed cleaveages (not taken into account the "NO CUT" points!!)
            int mcCut = seq.Split(cutPoints).GetUpperBound(0);

            //NO CUT combinations
            string[] noCutCombs = new string[cutPoints.Length * nocutPoints.Length];
            int counter = 0;
            for (int i = 0; i <= cutPoints.GetUpperBound(0); i++)
            {
                for (int k = 0; k <= nocutPoints.GetUpperBound(0); k++)
                {
                    switch (enzyme.sense)
                    {
                        case DA_paramsReader.paramsData.enzyme.senseType.C:
                            {
                                noCutCombs[counter] = string.Concat(cutPoints[i] , nocutPoints[k]);
                                break;
                            }
                        case DA_paramsReader.paramsData.enzyme.senseType.N:
                            {
                                noCutCombs[counter] = string.Concat(nocutPoints[k] , cutPoints[i]);
                                break;
                            }
                    }

                    counter++;
 
                }
            }

            //search for NO CUT points
            int mcNoCut = seq.Split(noCutCombs,StringSplitOptions.RemoveEmptyEntries).GetUpperBound(0);
            
            missedCleav = mcCut - mcNoCut;

            return missedCleav;
        }

        private static int numTolTerm(string sequence,
                                        DA_paramsReader.paramsData.enzyme enzyme)
        {
            int nCorrectCleav = 0;

            char[] cutPoints = enzyme.cut.ToCharArray();
            char[] nocutPoints = enzyme.noCut.ToCharArray();

            switch (enzyme.sense)
            {
                case DA_paramsReader.paramsData.enzyme.senseType.C:
                    {
                        if (sequence.IndexOfAny(cutPoints) == sequence.Length - 1)
                        {
                            nCorrectCleav++;
                        }
                        break; 
                    }
                case DA_paramsReader.paramsData.enzyme.senseType.N:
                    {
                        if (sequence.LastIndexOfAny(cutPoints) == 0)
                        {
                            nCorrectCleav++;
                        }
                        break; 
                    }
            }

            return nCorrectCleav;
        }

        private static ArrayList getModsFromSrf(string modifications,
                                        DA_paramsReader.paramsData.massType massType,
                                        DataSet ds)
        {
            ArrayList modsList = new ArrayList();

            string[] occ;
            float[] miw;
            float[] aiw;
            getAAmassesFromDataSet(ds, out occ, out miw, out aiw);
     

            string[] separators = new string[2];
            separators[0] = ")";
            separators[1] = "(";
            string[] shModsSp = modifications.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            
            
            for (int ffd = 0; ffd <= shModsSp.GetUpperBound(0); ffd++)
            {
                if (shModsSp[ffd].Trim() != "")
                {
                    int bsMod = shModsSp[ffd].IndexOf('=');
                    if (bsMod != -1)
                    {
                        //stable modifications
                        char[] stabSeps = new char[2];
                        stabSeps[0] = ' ';
                       
                        string[] stabMods = shModsSp[ffd].Split(stabSeps, StringSplitOptions.RemoveEmptyEntries);
                        
                        for (int numstMods = 0; numstMods <= stabMods.GetUpperBound(0); numstMods++)
                        {
                            Utilities.Modifications newMod = new Utilities.Modifications();
                            newMod.aminoacid = stabMods[numstMods].Substring(0, 1);
                            newMod.finalMass = float.Parse(stabMods[numstMods].Substring(2, stabMods[numstMods].Length - 2));
                       
                            for (int k = 0; k <= occ.GetUpperBound(0); k++)
                            {
                                if (newMod.aminoacid == occ[k])
                                {
                                    switch (massType)
                                    {
                                        case DA_paramsReader.paramsData.massType.average:
                                            {
                                                newMod.deltaMass =newMod.finalMass - aiw[k];
                                                break; 
                                            }
                                        case DA_paramsReader.paramsData.massType.monoisotopic:
                                            {
                                                newMod.deltaMass = newMod.finalMass - miw[k];
                                                break; 
                                            }
                                        
                                    }

                                    break;                                    
                                }
                            }
                            
                            newMod.symbol = ' ';
                            newMod.variable = false;

                            modsList.Add(newMod);

                        }
                    }
                    else
                    {
                        //variable modification
                        Utilities.Modifications newMod = new Utilities.Modifications();
                        newMod.aminoacid = shModsSp[ffd].Substring(0, 1);
                        newMod.symbol = shModsSp[ffd].Substring(1, 1).ToCharArray()[0];
                        newMod.deltaMass = float.Parse(shModsSp[ffd].Substring(2, shModsSp[ffd].Length - 2));
                        newMod.variable = true;

                        for (int k = 0; k <= occ.GetUpperBound(0); k++)
                        {
                            if (newMod.aminoacid == occ[k])
                            {
                                switch (massType)
                                {
                                    case DA_paramsReader.paramsData.massType.average:
                                        {
                                            newMod.finalMass = aiw[k]+newMod.deltaMass;
                                            break;
                                        }
                                    case DA_paramsReader.paramsData.massType.monoisotopic:
                                        {
                                            newMod.finalMass = miw[k]+newMod.deltaMass;
                                            break;
                                        }
                                }

                                break;
                            }
                        }


                        modsList.Add(newMod);

                    }
                }
            }

            return modsList;
            
        }

        private static void getAAmassesFromDataSet(DataSet ds, out string[] occ, out float[] miw, out float[] aiw)
        {
            DataView dv = new DataView(ds.Tables["aminoacid"]);
            occ = new string[dv.Count];
            miw = new float[dv.Count];
            aiw = new float[dv.Count];

            for (int i = 0; i < dv.Count; i++)
            {
                occ[i] = dv[i].Row["one_char_code"].ToString();
                miw[i] = float.Parse(dv[i].Row["monoisotopic_incremental_weight"].ToString());
                aiw[i] = float.Parse(dv[i].Row["average_incremental_weight"].ToString());
            }
        }

        private static string completeDateTime(string dateOrTime,
                                                int digits)
        {

            while (dateOrTime.Length < digits)
            {
                dateOrTime = "0" + dateOrTime; 
            }

            return dateOrTime;
        }

        /// <summary>
        /// extract data from a sorted ArrayList 
        /// </summary>
        /// <param name="al">arraylist which contains the data to be extracted</param>
        /// <param name="comparer">criterium to compare</param>
        /// <param name="comparison">Element to extract</param>
        /// <returns>an arraylist with the desired elements</returns>
        public static ArrayList extract(ArrayList al, 
                                        OutData.OutsComparer comparer,
                                        OutData comparison,
                                        bool sorted)
        {

           
            if (!sorted)
            {
                al.Sort(comparer);
            }
            
            int iMin = 0;
            int iMax = al.Count - 1;
            int maximumFound=0;

            int diff = iMax - iMin;

            int iFirst = -1;
            int iLast = al.Count;
            int iPos = diff / 2;

            //Check wether the data is the first
            OutData sData = (OutData)al[0];
            int iCmp = sData.CompareTo(comparison, comparer.WhichComparison);
            if (iCmp != 0)
            {
                while (diff > 1)
                {
                    sData = (OutData)al[iPos];
                    int iCm = sData.CompareTo(comparison, comparer.WhichComparison);
                    switch (iCm)
                    {
                        case 0:
                        {
                            if (iCm > maximumFound)
                            {
                                maximumFound = iCm;
                            }

                            OutData sContig = (OutData)al[iPos - 1];
                            int iContig = sContig.CompareTo(comparison, comparer.WhichComparison);
                            if(iContig==-1)
                            {
                                iFirst = iPos;
                                diff = 0;
                                break;
                            }
                            if(iContig==0)
                            {
                                iMax = iPos;
                                diff = iMax - iMin;
                                iPos = iPos - diff / 2;
                            }
                            if(iContig==1)
                            {
                                //Not sorted!!!
                                return extract(al, comparer, comparison, false);                                            
                            }
                            break;
                            
                        }
                        case -1:
                        {
                            iMin = iPos;
                            diff = iMax - iMin;
                            iPos = iPos + diff / 2;
                            break;
                        }
                        case 1:
                        {
                            iMax = iPos;
                            diff = iMax - iMin;
                            iPos = iPos - diff / 2;
                            break;
                        }
                    }
 
                }
                
            }
            else 
            {
                iFirst = 0; 
            }


            //Check wether the data is the last
            iMin = maximumFound;
            iMax = al.Count - 1;
            diff = iMax - iMin;
            iPos = diff / 2;
            
            sData = (OutData)al[al.Count-1];
            iCmp = sData.CompareTo(comparison, comparer.WhichComparison);
            if (iCmp != 0)
            {
                while (diff > 1)
                {
                    sData = (OutData)al[iPos];
                    int iCm = sData.CompareTo(comparison, comparer.WhichComparison);
                    switch (iCm)
                    {
                        case 0:
                            {
                                OutData sContig = (OutData)al[iPos + 1];
                                int iContig = sContig.CompareTo(comparison, comparer.WhichComparison);
                                if (iContig == +1)
                                {
                                    iLast = iPos;
                                    diff = 0;
                                    break;
                                }
                                if (iContig == 0)
                                {
                                    iMin = iPos;
                                    diff = iMax - iMin;
                                    iPos += diff / 2;
                                }
                                if (iContig == -1)
                                {
                                    //Not sorted!!!
                                    return extract(al, comparer, comparison, false);
                                }
                                break;

                            }
                        case -1:
                            {
                                iMin = iPos;
                                diff = iMax - iMin;
                                iPos += diff / 2;
                                break;
                            }
                        case 1:
                            {
                                iMax = iPos;
                                diff = iMax - iMin;
                                iPos -= diff / 2;
                                break;
                            }
                    }

                }
                
            }
            else
            {
                iLast = al.Count-1;
            }

            ArrayList alTmp = new ArrayList();
            try
            {
                for (int i = 0; i <= iLast - iFirst; i++)
                {
                    alTmp.Add(al[iFirst + i]);
                }
            }
            catch { }

            return alTmp;
        }


        public static double ranking(ArrayList rankingList, 
                                    OutData.OutsComparer comparer,
                                    OutData toRank,
                                    bool sortedDesc)
        {

            if (!sortedDesc)
            {
                rankingList.Sort(comparer);
                rankingList.Reverse();
            }

            int iMin=0;
            int iMax=rankingList.Count-1;
            int diff=iMax-iMin;
            int iPos = diff / 2;
            int rank = 0;

            //check wether the data to rank is over the entire ranking list
            OutData sdataFirst = (OutData)rankingList[0];
            OutData sdataLast = (OutData)rankingList[rankingList.Count - 1];

            int ic = sdataFirst.CompareTo(toRank, comparer.WhichComparison);
            if (ic < 0)
            {
                rank = -1;
                return rank;
            }
            if (ic == 0)
            {
                rank = 1;
                return rank;
            }

            ic = sdataLast.CompareTo(toRank, comparer.WhichComparison);
            if (ic > 1)
            {
                rank = rankingList.Count + 1;
                return rank;
            }
            
            


            while (diff > 1)
            {
                OutData sd = (OutData)rankingList[iPos];

                int iCmp = sd.CompareTo(toRank, comparer.WhichComparison);

                switch (iCmp)
                {
                    case -1:
                        {
                            rank = iPos-1;
                            iMax = iPos;
                            diff = iMax - iMin;
                            iPos -= diff / 2;
                            break;
                        }
                    case 0:
                        {
                            rank = iPos;
                            diff = 0;
                            break;
                        }
                    case 1:
                        {
                            rank = iPos;
                            iMin = iPos;
                            diff = iMax - iMin;
                            iPos += diff / 2;
                            break;
                        }
 
                }
 
            }


            // following lines are to convert to decimal the ranks
            // which is important for the fine calculation of pRatio
            int rankTop = rank;
            int rankBottom = rank + 1;
            double decimalRank = 0;
            double compare1Top = 0;
            double compare1Bottom = 0;
            double compare2 = 0;

            switch(comparer.WhichComparison)
            {
                case pRatio.OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorr1:
                    {
                        compare1Top = ((OutData)rankingList[rankTop]).Xcorr1;
                        compare1Bottom = ((OutData)rankingList[rankBottom]).Xcorr1;
                        compare2 = toRank.XcorrRandom;
                        break;
                    }
                case pRatio.OutData.OutsComparer.ComparisonType.XcorrRandomVSXcorrRandom:
                    {
                        compare1Top = ((OutData)rankingList[rankTop]).XcorrRandom;
                        compare1Bottom = ((OutData)rankingList[rankBottom]).XcorrRandom;
                        compare2 = toRank.XcorrRandom;
                        break;
                    }
                case pRatio.OutData.OutsComparer.ComparisonType.Xcorr1:
                    {
                        compare1Top = ((OutData)rankingList[rankTop]).Xcorr1;
                        compare1Bottom = ((OutData)rankingList[rankBottom]).Xcorr1;
                        compare2 = toRank.Xcorr1;
                        break;
                    }
                default:
                    {
                        // error
                        decimalRank = (double)rank + 1;
                        break;
                    }
            }

            if (compare1Bottom == compare1Top)
                decimalRank = (double)(rankBottom + rankTop) / 2;
            else
            {
                double slope = (double)(rankBottom - rankTop) / (compare1Bottom - compare1Top);
                double y_intercept = (double)rankTop - slope * compare1Top;

                decimalRank = slope * compare2 + y_intercept;
            }

            return decimalRank;
        }


        public static void writeQuiXML(ArrayList targetList,
                                        double FDR,
                                        string foldername,
                                        int totBatches,
                                        int numBatch)
        {

            System.IO.DirectoryInfo di = System.IO.Directory.GetParent(foldername);
            
            XmlDocument doc = new XmlDocument();

            XmlNode IdentificationArchive = doc.CreateElement("IdentificationArchive");

            XmlNode Identifications = doc.CreateElement("Identifications");
            
            doc.AppendChild(IdentificationArchive);
            IdentificationArchive.AppendChild(Identifications);
         

            foreach (OutData sd in targetList)
            {
                if (sd.FDR <= FDR)
                {
                    XmlNode peptide_match = doc.CreateElement("peptide_match");

                    XmlNode RAWFileName = doc.CreateElement("RAWFileName");
                    RAWFileName.InnerText = sd.RAWFile;
                    peptide_match.AppendChild(RAWFileName);

                    XmlNode FileName = doc.CreateElement("FileName");
                    FileName.InnerText = sd.FileName;
                    peptide_match.AppendChild(FileName);

                    XmlNode FirstScan = doc.CreateElement("FirstScan");
                    FirstScan.InnerText = sd.FirstScan.ToString();
                    peptide_match.AppendChild(FirstScan);

                    XmlNode LastScan = doc.CreateElement("LastScan");
                    LastScan.InnerText = sd.LastScan.ToString();
                    peptide_match.AppendChild(LastScan);

                    XmlNode Charge = doc.CreateElement("Charge");
                    Charge.InnerText = sd.Charge.ToString();
                    peptide_match.AppendChild(Charge);

                    XmlNode FDRx = doc.CreateElement("FDR");
                    FDRx.InnerText = sd.FDR.ToString();
                    peptide_match.AppendChild(FDRx);

                    XmlNode FASTAProteinDescription = doc.CreateElement("FASTAProteinDescription");
                    try
                    {
                        FASTAProteinDescription.InnerText = sd.ProteinDescription.ToString();
                    }
                    catch { }
                    peptide_match.AppendChild(FASTAProteinDescription);

                    XmlNode Sequence = doc.CreateElement("Sequence");
                    if (sd.Sequence != null)
                    {
                        Sequence.InnerText = sd.Sequence.ToString();
                    }
                    peptide_match.AppendChild(Sequence);

                    XmlNode pI = doc.CreateElement("pI");
                    pI.InnerText = sd.pI.ToString();
                    peptide_match.AppendChild(pI);

                    XmlNode PrecursorMass = doc.CreateElement("PrecursorMass");
                    PrecursorMass.InnerText = sd.PrecursorMass.ToString();
                    peptide_match.AppendChild(PrecursorMass);
                    
                    XmlNode TheoreticalMass = doc.CreateElement("q_peptide_Mass");
                    TheoreticalMass.InnerText = sd.TheoreticalMass.ToString();
                    peptide_match.AppendChild(TheoreticalMass);

                    XmlNode XC1D = doc.CreateElement("XC1D");
                    XC1D.InnerText = sd.Xcorr1Original.ToString();
                    peptide_match.AppendChild(XC1D);

                    XmlNode XC2D = doc.CreateElement("XC2D");
                    XC2D.InnerText = sd.Xcorr2Search.ToString();
                    peptide_match.AppendChild(XC2D);

                    XmlNode deltaCn = doc.CreateElement("deltaCn");
                    deltaCn.InnerText = sd.DeltaCn.ToString();
                    peptide_match.AppendChild(deltaCn);

                    XmlNode Sp = doc.CreateElement("Sp");
                    Sp.InnerText = sd.Sp.ToString();
                    peptide_match.AppendChild(Sp);

                    XmlNode SpRank = doc.CreateElement("SpRank");
                    SpRank.InnerText = sd.SpRank.ToString();
                    peptide_match.AppendChild(SpRank);

                    XmlNode Proteinswithpeptide = doc.CreateElement("Proteinswithpeptide");
                    Proteinswithpeptide.InnerText = sd.ProteinsWithPeptide.ToString();
                    peptide_match.AppendChild(Proteinswithpeptide);

                    XmlNode rankings = doc.CreateElement("rankings");
                    XmlNode rnkXc1D = doc.CreateElement("rnkXc1D");
                    XmlNode rnkXc2D = doc.CreateElement("rnkXc2D");
                    XmlNode rnkXc1I = doc.CreateElement("rnkXc1I");
                    XmlNode rnkXc2I = doc.CreateElement("rnkXc2I");
                    rnkXc1D.InnerText = sd.rnkXc1.ToString();
                    rnkXc2D.InnerText = sd.rnkXcRandom.ToString();
                    rnkXc1I.InnerText = "0";
                    rnkXc2I.InnerText = "0";
                    rankings.AppendChild(rnkXc1D);
                    rankings.AppendChild(rnkXc2D);
                    rankings.AppendChild(rnkXc1I);
                    rankings.AppendChild(rnkXc2I);

                    peptide_match.AppendChild(rankings);

                    XmlNode Redundances = doc.CreateElement("Redundances");

                    if (sd.ProteinsWithPeptide > 0)
                    {
                     try
                     {
                        foreach (pRatio.Redundance myRed in sd.Redundances)
                        {
                            
                                XmlNode Red = doc.CreateElement("Red");
                                XmlNode FASTAIndex_red = doc.CreateElement("FASTAIndex");
                                XmlNode FASTAProteinDescription_red = doc.CreateElement("FASTAProteinDescription");
                                FASTAIndex_red.InnerText = myRed.FASTAIndex.ToString();
                                FASTAProteinDescription_red.InnerText = myRed.FASTAProteinDescription.ToString();

                                Red.AppendChild(FASTAIndex_red);
                                Red.AppendChild(FASTAProteinDescription_red);

                                Redundances.AppendChild(Red);
                            
                        }
                     }
                     catch { }

                    }
                    peptide_match.AppendChild(Redundances);
                
                    Identifications.AppendChild(peptide_match);

                 

                }
            }

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_QuiXML";
            if (totBatches > 1) filename += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filename += ".xml";
            
            doc.Save(filename);
            

        }


        public static void writepepXML(ArrayList targetList,
                                        double FDR,
                                        DA_fileReader.SrfData srfHeader,
                                        DA_paramsReader.paramsData searchParams,
                                        string foldername,
                                        string modificationsFile)
        {
            System.IO.DirectoryInfo di = System.IO.Directory.GetParent(foldername);

            string folderFullName=di.FullName;

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_pepXML.xml";
            string str_base_name = foldername.Trim();

            DataSet ds = new DataSet("aa");
            ds.ReadXmlSchema(Settings.Default.aaWeight_Schema.ToString());
            ds.ReadXml(Settings.Default.aaWeight_XML.ToString());

            

            XmlDocument doc = new XmlDocument();

            

            XmlNode xmlDeclaration = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            doc.AppendChild(xmlDeclaration);

            XmlNode msms_pipeline_analysis = doc.CreateElement("msms_pipeline_analysis");


            XmlAttribute date = doc.CreateAttribute("date");
            //"2007-07-16T16:31:56"

            string year =completeDateTime(System.DateTime.Now.Year.ToString(),4);
            string month =completeDateTime(System.DateTime.Now.Month.ToString(),2);
            string day = completeDateTime(System.DateTime.Now.Day.ToString(),2);
            string hour =completeDateTime(System.DateTime.Now.Hour.ToString(),2);
            string minute = completeDateTime(System.DateTime.Now.Minute.ToString(), 2);
            string second = completeDateTime(System.DateTime.Now.Second.ToString(), 2);

            date.Value = year;
            date.Value += "-" + month;
            date.Value += "-" + day;
            date.Value += "T" + hour;
            date.Value += ":" + minute;
            date.Value += ":" + second;

            
            msms_pipeline_analysis.Attributes.Append(date);

            
            XmlAttribute summary_xml = doc.CreateAttribute("summary_xml");
            summary_xml.Value = filename;

            msms_pipeline_analysis.Attributes.Append(summary_xml);

    
            doc.AppendChild(msms_pipeline_analysis);
      


            string xsiNS = "http://www.w3.org/2001/XMLSchema-instance";
            string xmlNS = "http://regis-web.systemsbiology.net/pepXML";
            string xmlSchemaLocation = "http://regis-web.systemsbiology.net/pepXML file:///C:/Documents%20and%20Settings/mesontau/Escritorio/pepXML/pepXML_v18.xsd";

            //xmlns="http://regis-web.systemsbiology.net/pepXML" 
            //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
            //xsi:schemaLocation="http://regis-web.systemsbiology.net/pepXML file:///C:/Documents%20and%20Settings/mesontau/Escritorio/pepXML/pepXML_v18.xsd">


            //xmlns="http://regis-web.systemsbiology.net/pepXML" 
            //xsi:pepXML_v18.xsd="conf\pepXML_v18.xsd" 
            //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"


            XmlAttribute attributeNode = doc.CreateAttribute("xmlns");
            attributeNode.Value = xmlNS;
            doc.DocumentElement.SetAttributeNode(attributeNode);
            
            attributeNode = doc.CreateAttribute("xsi","schemaLocation",xsiNS);
            attributeNode.Value = xmlSchemaLocation;
            doc.DocumentElement.SetAttributeNode(attributeNode);

          
            XmlNode analysis_summary = doc.CreateElement("analysis_summary");

            XmlAttribute ansum_analysis = doc.CreateAttribute("analysis");
            XmlAttribute ansum_time = doc.CreateAttribute("time");

            ansum_analysis.Value = "probabilityRatio";
            ansum_time.Value = date.Value;

            analysis_summary.Attributes.Append(ansum_analysis);
            analysis_summary.Attributes.Append(ansum_time);

            msms_pipeline_analysis.AppendChild(analysis_summary);

            XmlNode msms_run_summary = doc.CreateElement("msms_run_summary");
            
            XmlAttribute base_name = doc.CreateAttribute("base_name");
            XmlAttribute msManufacturer = doc.CreateAttribute("msManufacturer");
            XmlAttribute msModel = doc.CreateAttribute("msModel");
            XmlAttribute msIonization = doc.CreateAttribute("msIonization");
            XmlAttribute msMassAnalyzer = doc.CreateAttribute("msMassAnalyzer");
            XmlAttribute msDetector = doc.CreateAttribute("msDetector");
            XmlAttribute raw_data_type = doc.CreateAttribute("raw_data_type");
            XmlAttribute raw_data = doc.CreateAttribute("raw_data");

            base_name.Value = str_base_name;
            msManufacturer.Value = "Thermo-Fisher";
            msModel.Value = srfHeader.Instrument;
            msIonization.Value = "";
            msMassAnalyzer.Value = "Ion Trap";
            msDetector.Value = "";
            raw_data_type.Value = "raw";
            raw_data.Value = ".raw";
            
            msms_run_summary.Attributes.Append(base_name);
            msms_run_summary.Attributes.Append(msManufacturer);
            msms_run_summary.Attributes.Append(msModel);
            msms_run_summary.Attributes.Append(msIonization);
            msms_run_summary.Attributes.Append(msMassAnalyzer);
            msms_run_summary.Attributes.Append(msDetector);
            msms_run_summary.Attributes.Append(raw_data_type);
            msms_run_summary.Attributes.Append(raw_data);

            msms_pipeline_analysis.AppendChild(msms_run_summary);

            XmlNode sample_enzyme = doc.CreateElement("sample_enzyme");

            XmlAttribute sample_enzime_name = doc.CreateAttribute("name");

            sample_enzime_name.Value = searchParams.enzyme_info.name;

            sample_enzyme.Attributes.Append(sample_enzime_name);

            XmlNode specifity = doc.CreateElement("specificity");

            XmlAttribute specificity_cut=doc.CreateAttribute("cut");
            XmlAttribute specificity_no_cut = doc.CreateAttribute("no_cut");
            XmlAttribute specificity_sense = doc.CreateAttribute("sense");

            specificity_cut.Value = searchParams.enzyme_info.cut;
            specificity_no_cut.Value = searchParams.enzyme_info.noCut;
            specificity_sense.Value = searchParams.enzyme_info.sense.ToString();

            specifity.Attributes.Append(specificity_cut);
            specifity.Attributes.Append(specificity_no_cut);
            specifity.Attributes.Append(specificity_sense);

            sample_enzyme.AppendChild(specifity);

            msms_run_summary.AppendChild(sample_enzyme);

            XmlNode search_summary = doc.CreateElement("search_summary");

            XmlAttribute base_name_search_summary = doc.CreateAttribute("base_name");
            base_name_search_summary.Value = str_base_name;

            XmlAttribute ssummary_fragment_mass_type = doc.CreateAttribute("fragment_mass_type");
            XmlAttribute ssummary_out_data_type = doc.CreateAttribute("out_data_type");
            XmlAttribute ssummary_out_data = doc.CreateAttribute("out_data");
            XmlAttribute ssummary_search_engine = doc.CreateAttribute("search_engine");
            XmlAttribute ssummary_precursor_mass_type = doc.CreateAttribute("precursor_mass_type");
            XmlAttribute ssummary_search_id = doc.CreateAttribute("search_id");

            ssummary_fragment_mass_type.Value = searchParams.mass_type_fragment.ToString();

            // ssumary_of_data will not work if matches from msf and srf are mixed
            OutData firstMatch = (OutData)targetList[0];
            string extensionOfFirst = Path.GetExtension(firstMatch.FileName);

            ssummary_out_data_type.Value = extensionOfFirst;
            ssummary_out_data.Value = str_base_name + "\\*" + extensionOfFirst;


            ssummary_search_engine.Value = "SEQUEST";
            ssummary_precursor_mass_type.Value = searchParams.mass_type_parent.ToString();
            ssummary_search_id.Value = "1";

            search_summary.Attributes.Append(base_name_search_summary);
            search_summary.Attributes.Append(ssummary_fragment_mass_type);
            search_summary.Attributes.Append(ssummary_out_data_type);
            search_summary.Attributes.Append(ssummary_out_data);
            search_summary.Attributes.Append(ssummary_search_engine);
            search_summary.Attributes.Append(ssummary_precursor_mass_type);
            search_summary.Attributes.Append(ssummary_search_id);                             
            
            
            XmlNode search_database = doc.CreateElement("search_database");

            XmlAttribute search_database_local_path = doc.CreateAttribute("local_path");
            XmlAttribute search_database_type = doc.CreateAttribute("type");

            search_database_local_path.Value = searchParams.first_database_name;
            search_database_type.Value = "AA";

            search_database.Attributes.Append(search_database_local_path);
            search_database.Attributes.Append(search_database_type);

            search_summary.AppendChild(search_database);

            XmlNode enzymatic_search_constraint = doc.CreateElement("enzymatic_search_constraint");

            XmlAttribute enzymatic_search_constraint_enzyme = doc.CreateAttribute("enzyme");
            XmlAttribute enzymatic_search_constraint_max_num_internal_cleavages = doc.CreateAttribute("max_num_internal_cleavages");
            XmlAttribute enzymatic_search_constraint_min_number_termini = doc.CreateAttribute("min_number_termini");

            enzymatic_search_constraint_enzyme.Value = searchParams.enzyme_info.name;
            enzymatic_search_constraint_max_num_internal_cleavages.Value = searchParams.max_num_internal_cleavage_sites.ToString();
            enzymatic_search_constraint_min_number_termini.Value = "1";

            enzymatic_search_constraint.Attributes.Append(enzymatic_search_constraint_enzyme);
            enzymatic_search_constraint.Attributes.Append(enzymatic_search_constraint_max_num_internal_cleavages);
            enzymatic_search_constraint.Attributes.Append(enzymatic_search_constraint_min_number_termini);                  


            search_summary.AppendChild(enzymatic_search_constraint);

            string shMods = srfHeader.Modifications;

            ArrayList modifs = new ArrayList();
            if (modificationsFile == "")
                modifs = getModsFromSrf(shMods, searchParams.mass_type_parent, ds);

            integrateModsFromXML(modificationsFile, modifs, ds, searchParams.mass_type_parent);

            ArrayList modsSymbols = new ArrayList();

            for (int nModif = 0; nModif < modifs.Count; nModif++)
            {
                Modifications mod = (Modifications)modifs[nModif];

                if (mod.variable == true)
                {
                    modsSymbols.Add(mod.symbol);
                }

                XmlNode nodeModif = doc.CreateElement("aminoacid_modification");

                XmlAttribute aaMod_aminoacid = doc.CreateAttribute("aminoacid");
                XmlAttribute aaMod_massdiff = doc.CreateAttribute("massdiff");
                XmlAttribute aaMod_mass = doc.CreateAttribute("mass");
                XmlAttribute aaMod_variable = doc.CreateAttribute("variable");
                XmlAttribute aaMod_symbol = doc.CreateAttribute("symbol");

                nodeModif.Attributes.Append(aaMod_aminoacid);
                nodeModif.Attributes.Append(aaMod_massdiff);
                nodeModif.Attributes.Append(aaMod_mass);
                nodeModif.Attributes.Append(aaMod_variable);

                aaMod_aminoacid.Value = mod.aminoacid;
                aaMod_massdiff.Value = mod.deltaMass.ToString();
                aaMod_mass.Value = mod.finalMass.ToString();
                switch (mod.variable)
                {
                    case true:
                        {
                            aaMod_variable.Value = "Y";
                            aaMod_symbol.Value = mod.symbol.ToString();
                            nodeModif.Attributes.Append(aaMod_symbol);
                            break;
                        }
                    case false:
                        {
                            aaMod_variable.Value = "N";
                            break;
                        }
                }

                search_summary.AppendChild(nodeModif);
            }

            
        

            
            for (int i = 1; i <= 13; i++)
            {
                XmlNode parameter = doc.CreateElement("parameter");
                XmlAttribute parameter_name = doc.CreateAttribute("name");
                XmlAttribute parameter_value = doc.CreateAttribute("value");

                parameter.Attributes.Append(parameter_name);
                parameter.Attributes.Append(parameter_value);

                search_summary.AppendChild(parameter);
            

            }


            int paramCounter = 1;
            foreach(XmlNode node in search_summary.ChildNodes) // (int i = 0; i < search_summary.ChildNodes.Count; i++)
            {
                if (node.Name == "parameter")
                {
                    switch (paramCounter)
                    {
                        case 1:
                            {
                                node.Attributes.Item(0).Value = "peptide_mass_tol";
                                node.Attributes.Item(1).Value = searchParams.peptide_mass_tolerance.ToString();
                                break; 
                            }
                        case 2:
                            {
                                node.Attributes.Item(0).Value = "fragment_ion_tol";
                                node.Attributes.Item(1).Value = searchParams.fragment_ion_tolerance.ToString();
                                break;
                            }
                        case 3:
                            {
                                node.Attributes.Item(0).Value = "ion_series";
                                node.Attributes.Item(1).Value = searchParams.ion_series.ToString();
                                break;
                            }
                        case 4:
                            {
                                node.Attributes.Item(0).Value = "max_num_differential_AA_per_mod";
                                node.Attributes.Item(1).Value = searchParams.max_num_differential_per_peptide.ToString();
                                break;
                            }
                        case 5:
                            {
                                node.Attributes.Item(0).Value = "nucleotide_reading_frame";
                                node.Attributes.Item(1).Value = searchParams.nucleotide_reading_frame.ToString();
                                break;
                            }
                        case 6:
                            {
                                node.Attributes.Item(0).Value = "num_output_lines";
                                node.Attributes.Item(1).Value = searchParams.num_output_lines.ToString();
                                break;
                            }
                        case 7:
                            {
                                node.Attributes.Item(0).Value = "remove_precursor_peak";
                                if (searchParams.remove_precursor_peak == false)
                                {
                                    node.Attributes.Item(1).Value = "0";
                                }
                                else 
                                {
                                    node.Attributes.Item(1).Value = "1";
                                }
                                    break;
                            }
                        case 8:
                            {
                                node.Attributes.Item(0).Value = "ion_cutoff_percentage";
                                node.Attributes.Item(1).Value = searchParams.ion_cutoff_percentage.ToString();
                                break;
                            }
                        case 9:
                            {
                                node.Attributes.Item(0).Value = "match_peak_count";
                                node.Attributes.Item(1).Value = searchParams.match_peak_count.ToString();
                                break;
                            }
                        case 10:
                            {
                                node.Attributes.Item(0).Value = "match_peak_allowed_error";
                                node.Attributes.Item(1).Value = searchParams.match_peak_allowed_error.ToString();
                                break;
                            }
                        case 11:
                            {
                                node.Attributes.Item(0).Value = "match_peak_tolerance";
                                node.Attributes.Item(1).Value = searchParams.match_peak_tolerance.ToString();
                                break;
                            }
                        case 12:
                            {
                                node.Attributes.Item(0).Value = "protein_mass_filter";
                                try
                                {
                                    node.Attributes.Item(1).Value = searchParams.protein_mass_filter.ToString();
                                }
                                catch { node.Attributes.Item(1).Value = ""; }
                                break;
                            }
                        case 13:
                            {
                                node.Attributes.Item(0).Value = "sequence_header_filter";
                                node.Attributes.Item(1).Value = searchParams.sequence_header_filter.ToString();
                                break;
                            }
                    }
                    paramCounter++;
                }
            }

            msms_run_summary.AppendChild(search_summary);


            XmlNode analysis_timestamp = doc.CreateElement("analysis_timestamp");

            XmlAttribute an_ts_analysis = doc.CreateAttribute("analysis");
            an_ts_analysis.Value = "probabilityRatio";
            XmlAttribute an_ts_time = doc.CreateAttribute("time");
            an_ts_time.Value = date.Value;
            XmlAttribute an_ts_id = doc.CreateAttribute("id");
            an_ts_id.Value = "1";


            analysis_timestamp.Attributes.Append(an_ts_analysis);
            analysis_timestamp.Attributes.Append(an_ts_time);
            analysis_timestamp.Attributes.Append(an_ts_id);

            msms_run_summary.AppendChild(analysis_timestamp);


            int iCounterIds = 1;
            foreach (OutData od in targetList)
            {
                if (od.FDR <= FDR)
                {
                    XmlNode spectrum_query = doc.CreateElement("spectrum_query");

                    XmlAttribute sq_spectrum = doc.CreateAttribute("spectrum");
                    XmlAttribute sq_start_scan = doc.CreateAttribute("start_scan");
                    XmlAttribute sq_end_scan = doc.CreateAttribute("end_scan");
                    XmlAttribute sq_precursor_neutral_mass = doc.CreateAttribute("precursor_neutral_mass");
                    XmlAttribute sq_assumed_charge = doc.CreateAttribute("assumed_charge");
                    XmlAttribute sq_index = doc.CreateAttribute("index");
                    XmlAttribute sq_retention_time = doc.CreateAttribute("retention_time");

                    string rawf = od.RAWFile.Split('.')[0].ToString();

                    sq_spectrum.Value = rawf+"."+od.FirstScan.ToString()+"."+od.LastScan.ToString()+"."+od.Charge.ToString();
                    sq_start_scan.Value = od.FirstScan.ToString();
                    sq_end_scan.Value = od.LastScan.ToString();
                    sq_retention_time.Value = od.retentionTime.ToString();

                    float prec_neutralMass = (float)od.PrecursorMass; //((float)od.PrecursorMass*od.Charge)-od.Charge*Settings.Default.HidrogenMass;


                    sq_precursor_neutral_mass.Value = prec_neutralMass.ToString("#.####");
                    sq_assumed_charge.Value = od.Charge.ToString();
                    sq_index.Value = iCounterIds.ToString();

                    spectrum_query.Attributes.Append(sq_spectrum);
                    spectrum_query.Attributes.Append(sq_start_scan);
                    spectrum_query.Attributes.Append(sq_end_scan);
                    spectrum_query.Attributes.Append(sq_retention_time);
                    spectrum_query.Attributes.Append(sq_precursor_neutral_mass);
                    spectrum_query.Attributes.Append(sq_assumed_charge);
                    spectrum_query.Attributes.Append(sq_index);


                    XmlNode search_result = doc.CreateElement("search_result");

                    XmlNode search_hit = doc.CreateElement("search_hit");

                    XmlAttribute searchh_hit_rank = doc.CreateAttribute("hit_rank");
                    XmlAttribute searchh_peptide = doc.CreateAttribute("peptide");
                    XmlAttribute searchh_peptide_prev_aa = doc.CreateAttribute("peptide_prev_aa");
                    XmlAttribute searchh_peptide_next_aa = doc.CreateAttribute("peptide_next_aa");
                    XmlAttribute searchh_protein = doc.CreateAttribute("protein");
                    XmlAttribute searchh_num_tot_proteins = doc.CreateAttribute("num_tot_proteins");
                    XmlAttribute searchh_num_matched_ions = doc.CreateAttribute("num_matched_ions");
                    XmlAttribute searchh_tot_num_ions = doc.CreateAttribute("tot_num_ions");
                    XmlAttribute searchh_calc_neutral_pep_mass = doc.CreateAttribute("calc_neutral_pep_mass");
                    XmlAttribute searchh_massdiff = doc.CreateAttribute("massdiff");
                    XmlAttribute searchh_num_tol_term = doc.CreateAttribute("num_tol_term");
                    XmlAttribute searchh_num_missed_cleavages = doc.CreateAttribute("num_missed_cleavages");
                    XmlAttribute searchh_is_rejected = doc.CreateAttribute("is_rejected");
                    XmlAttribute searchh_protein_descr = doc.CreateAttribute("protein_descr");

                    searchh_hit_rank.Value = "1";
                    
                    //check wether there is a preview aa
                    if (od.Sequence.Substring(1, 1) == ".")
                    {
                        searchh_peptide_prev_aa.Value = od.Sequence.Substring(0, 1);
                    }
                    else
                    {
                        searchh_peptide_prev_aa.Value = "";
                    }

                    //check wether there is a next aa
                    if (od.Sequence.Substring(od.Sequence.Length-2, 1) == ".")
                    {
                        searchh_peptide_next_aa.Value = od.Sequence.Substring(od.Sequence.Length-1, 1);
                    }
                    else
                    {
                        searchh_peptide_next_aa.Value = "";
                    }

                    string[] pep = od.Sequence.Split('.');


                    string pepNoSymbols;

                    if (pep.Length > 1) // sequence like K.LALALAR.M
                        pepNoSymbols = (string)pep[1].Clone();
                    else // sequence without dots
                        pepNoSymbols = (string)pep[0].Clone();

                    char[] chSymbol = new char[modsSymbols.Count];
                    for (int i = 0; i < modsSymbols.Count; i++)
                    {
                        chSymbol[i] = (char)modsSymbols[i];
                    }



                    int idxSymbol = pepNoSymbols.IndexOfAny(chSymbol);
                    while (idxSymbol != -1)
                    {

                        pepNoSymbols = pepNoSymbols.Remove(idxSymbol, 1);
                        idxSymbol = pepNoSymbols.IndexOfAny(chSymbol);

                    }
                     

                    searchh_peptide.Value = pepNoSymbols;
                    searchh_protein.Value = getProtein(od.ProteinDescription);
                    int numTotProteins=(int)od.ProteinsWithPeptide+1;
                    searchh_num_tot_proteins.Value = numTotProteins.ToString();
                    searchh_num_matched_ions.Value = od.IonsMatched.ToString();
                    searchh_tot_num_ions.Value = od.IonsCompared.ToString();
                    float calc_neutral_pep_mass = getNeutralPepMass(searchh_peptide.Value, ds, modifs, searchParams.mass_type_parent, false);
                    searchh_calc_neutral_pep_mass.Value = calc_neutral_pep_mass.ToString();
                    float fsearchh_massdiff=prec_neutralMass-calc_neutral_pep_mass;
                    searchh_massdiff.Value = fsearchh_massdiff.ToString();
                    

                    int numMissedCleav = getNumOfMissedCleaveages(pepNoSymbols, searchParams.enzyme_info);


                    searchh_num_tol_term.Value = numTolTerm(pepNoSymbols,searchParams.enzyme_info).ToString();

                    searchh_num_missed_cleavages.Value = numMissedCleav.ToString();
                    searchh_is_rejected.Value = "0";
                    searchh_protein_descr.Value = od.ProteinDescription;


                    search_hit.Attributes.Append(searchh_hit_rank);
                    search_hit.Attributes.Append(searchh_peptide);
                    search_hit.Attributes.Append(searchh_peptide_prev_aa);
                    search_hit.Attributes.Append(searchh_peptide_next_aa);
                    search_hit.Attributes.Append(searchh_protein);
                    search_hit.Attributes.Append(searchh_num_tot_proteins);
                    search_hit.Attributes.Append(searchh_num_matched_ions);
                    search_hit.Attributes.Append(searchh_tot_num_ions);
                    search_hit.Attributes.Append(searchh_calc_neutral_pep_mass);
                    search_hit.Attributes.Append(searchh_massdiff);
                    search_hit.Attributes.Append(searchh_num_tol_term);
                    search_hit.Attributes.Append(searchh_num_missed_cleavages);
                    search_hit.Attributes.Append(searchh_is_rejected);
                    search_hit.Attributes.Append(searchh_protein_descr);
                  
                    try
                    {
                    foreach (pRatio.Redundance myRed in od.Redundances)
                    {
                      
                            XmlNode alternative_protein = doc.CreateElement("alternative_protein");

                            XmlAttribute altProt_protein = doc.CreateAttribute("protein");
                            XmlAttribute altProt_protein_descr = doc.CreateAttribute("protein_descr");
                            XmlAttribute altProt_num_tol_term = doc.CreateAttribute("num_tol_term");

                            altProt_protein.Value = getProtein(myRed.FASTAProteinDescription);
                            altProt_protein_descr.Value = myRed.FASTAProteinDescription;
                            altProt_num_tol_term.Value = searchh_num_tol_term.Value;

                            alternative_protein.Attributes.Append(altProt_protein);
                            alternative_protein.Attributes.Append(altProt_protein_descr);
                            alternative_protein.Attributes.Append(altProt_num_tol_term);                                  

                            search_hit.AppendChild(alternative_protein);
                    }
                    }
                    catch { }


                    //<modification_info modified_peptide="KTAQKTAM[147]QCCL">

                    bool hasMods=false;
                    for (int i = 0; i < modifs.Count; i++)
                    {
                        Modifications mm = (Modifications)modifs[i];

                        if (mm.variable == false)
                        {
                            int hasFixMods;
                            if (pep.Length > 1)
                                hasFixMods = pep[1].IndexOf(mm.symbol);
                            else
                                hasFixMods = pep[0].IndexOf(mm.symbol);

                            if (hasFixMods!=-1) hasMods = true;
                        }

                        if (mm.variable == true)
                        {
                            int hasVarMods;
                            if (pep.Length > 1)
                                hasVarMods = pep[1].IndexOf(mm.symbol);
                            else
                                hasVarMods = pep[0].IndexOf(mm.symbol);

                            if (hasVarMods!=-1) hasMods = true;
                        }
 
                    }

                    if (hasMods)
                    {
                        XmlNode modification_info = doc.CreateElement("modification_info");

                        XmlAttribute modified_peptide = doc.CreateAttribute("modified_peptide");


                        string pepWithMassMods;
                            
                        if(pep.Length>1)
                            pepWithMassMods= (string)pep[1].Clone();
                        else
                            pepWithMassMods = (string)pep[0].Clone();
                        
                        string pepTmp;
                        if (pep.Length > 1)
                            pepTmp = (string)pep[1].Clone();
                        else
                            pepTmp = (string)pep[0].Clone();

                        int numFixedModsFound = 0;

                        foreach (Modifications mm in modifs)
                        {
                            switch (mm.variable)
                            {
                                case false:
                                    {
                                        int iSearch = pepTmp.IndexOf(mm.aminoacid);
                                        while (iSearch != -1)
                                        {
                                            XmlNode mod_aminoacid_mass = doc.CreateElement("mod_aminoacid_mass");
                                            XmlAttribute mod_aa_mass_position = doc.CreateAttribute("position");
                                            XmlAttribute mod_aa_mass_mass = doc.CreateAttribute("mass");

                                            int position = iSearch + 1+numFixedModsFound;
                                            mod_aa_mass_position.Value = position.ToString();
                                            mod_aa_mass_mass.Value = mm.finalMass.ToString();

                                            mod_aminoacid_mass.Attributes.Append(mod_aa_mass_position);
                                            mod_aminoacid_mass.Attributes.Append(mod_aa_mass_mass);

                                            modification_info.AppendChild(mod_aminoacid_mass);

                                            pepTmp = pepTmp.Remove(iSearch, 1);
                                            iSearch = pepTmp.IndexOf(mm.aminoacid);
                                            numFixedModsFound++;
                                        }
                                     
                                        pepWithMassMods = pepWithMassMods.Replace(mm.aminoacid, mm.aminoacid + "[" + mm.finalMass.ToString("###") + "]");
                                     
                                        break;
                                    }
                                case true:
                                    {
                                        int iSearch = pepTmp.IndexOf(mm.symbol);
                                        while (iSearch != -1)
                                        {
                                            XmlNode mod_aminoacid_mass = doc.CreateElement("mod_aminoacid_mass");
                                            XmlAttribute mod_aa_mass_position = doc.CreateAttribute("position");
                                            XmlAttribute mod_aa_mass_mass = doc.CreateAttribute("mass");

                                            int position = iSearch;
                                            mod_aa_mass_position.Value = position.ToString();
                                            mod_aa_mass_mass.Value = mm.finalMass.ToString();

                                            mod_aminoacid_mass.Attributes.Append(mod_aa_mass_position);
                                            mod_aminoacid_mass.Attributes.Append(mod_aa_mass_mass);

                                            modification_info.AppendChild(mod_aminoacid_mass);

                                            pepTmp = pepTmp.Remove(iSearch, 1);
                                            iSearch = pepTmp.IndexOf(mm.symbol);
                                                                                        
                                        }


                                        string sy = mm.symbol.ToString();
                                        pepWithMassMods=pepWithMassMods.Replace(sy, "[" + mm.finalMass.ToString("###") + "]");
                                        break; 
                                    }

                            }
                        }

                        //<mod_aminoacid_mass position="6" mass="147.192600"/>
                       
                        modified_peptide.Value = pepWithMassMods;

                        modification_info.Attributes.Append(modified_peptide);
                        search_hit.AppendChild(modification_info);
                    }

                    XmlNode search_score_xcorr = doc.CreateElement("search_score");
                    XmlNode search_score_deltacn = doc.CreateElement("search_score");
                    XmlNode search_score_deltacnstar = doc.CreateElement("search_score");
                    XmlNode search_score_spscore = doc.CreateElement("search_score");
                    XmlNode search_score_sprank = doc.CreateElement("search_score");

                    XmlAttribute search_score_xcorr_name = doc.CreateAttribute("name");
                    XmlAttribute search_score_deltacn_name = doc.CreateAttribute("name");
                    XmlAttribute search_score_deltacnstar_name = doc.CreateAttribute("name");
                    XmlAttribute search_score_spscore_name = doc.CreateAttribute("name");
                    XmlAttribute search_score_sprank_name = doc.CreateAttribute("name");



                    search_score_xcorr_name.Value = "xcorr";
                    search_score_deltacn_name.Value = "deltacn";
                    search_score_deltacnstar_name.Value = "deltacnstar";
                    search_score_spscore_name.Value = "spscore";
                    search_score_sprank_name.Value = "sprank";

                    XmlAttribute search_score_xcorr_val = doc.CreateAttribute("value");
                    XmlAttribute search_score_deltacn_val = doc.CreateAttribute("value");
                    XmlAttribute search_score_deltacnstar_val = doc.CreateAttribute("value");
                    XmlAttribute search_score_spscore_val = doc.CreateAttribute("value");
                    XmlAttribute search_score_sprank_val = doc.CreateAttribute("value");


                    search_score_xcorr_val.Value = od.Xcorr1Original.ToString();
                    search_score_deltacn_val.Value = od.DeltaCn.ToString();
                    search_score_deltacnstar_val.Value = "0";
                    search_score_spscore_val.Value = od.Sp.ToString();
                    search_score_sprank_val.Value = od.SpRank.ToString();

                    search_score_xcorr.Attributes.Append(search_score_xcorr_name);
                    search_score_deltacn.Attributes.Append(    search_score_deltacn_name);
                    search_score_deltacnstar.Attributes.Append(search_score_deltacnstar_name);
                    search_score_spscore.Attributes.Append(    search_score_spscore_name);
                    search_score_sprank.Attributes.Append(     search_score_sprank_name);

                    search_score_xcorr.Attributes.Append(      search_score_xcorr_val);
                    search_score_deltacn.Attributes.Append(    search_score_deltacn_val);
                    search_score_deltacnstar.Attributes.Append(search_score_deltacnstar_val);
                    search_score_spscore.Attributes.Append(    search_score_spscore_val);
                    search_score_sprank.Attributes.Append(     search_score_sprank_val);

                    search_hit.AppendChild(search_score_xcorr);
                    search_hit.AppendChild(search_score_deltacn);
                    search_hit.AppendChild(search_score_deltacnstar);
                    search_hit.AppendChild(search_score_spscore);
                    search_hit.AppendChild(search_score_sprank);

                    XmlNode search_hit_pRatio = doc.CreateElement("parameter");
                    XmlNode search_hit_FDR = doc.CreateElement("parameter");

                    XmlAttribute search_hit_pRatio_name = doc.CreateAttribute("name");
                    XmlAttribute search_hit_FDR_name = doc.CreateAttribute("name");

                    search_hit_pRatio_name.Value = "pRatio";
                    search_hit_FDR_name.Value = "FDR";

                    XmlAttribute search_hit_pRatio_val = doc.CreateAttribute("value");
                    XmlAttribute search_hit_FDR_val = doc.CreateAttribute("value");
                    
                    search_hit_pRatio_val.Value=od.pRatioTarget.ToString();
                    search_hit_FDR_val.Value = od.FDR.ToString();

                    search_hit_pRatio.Attributes.Append(search_hit_pRatio_name);
                    search_hit_pRatio.Attributes.Append(search_hit_pRatio_val);

                    search_hit_FDR.Attributes.Append(search_hit_FDR_name);
                    search_hit_FDR.Attributes.Append(search_hit_FDR_val);

                    search_hit.AppendChild(search_hit_pRatio);
                    search_hit.AppendChild(search_hit_FDR);


                    search_result.AppendChild(search_hit);

                    //<search_hit hit_rank="1" peptide="TRLSY" peptide_prev_aa="K" peptide_next_aa="F" protein="IPI00013177" num_tot_proteins="4" num_matched_ions="  2" tot_num_ions="  8" calc_neutral_pep_mass="638.7214" massdiff="-1.497700" num_tol_term="1" num_missed_cleavages="1" is_rejected="0" protein_descr="IPI:IPI00013177.1|SWISS-PROT:P41220|TREMBL:Q49A86;Q6I9U5|ENSEMBL:ENSP00000235382|REFSEQ:NP_002914|H-INV:HIT000033010|VEGA:OTTHUMP00000033592;OTTHUMP00000060765 Tax_Id=9606 Regulator of G-protein signaling 2">
                       
                    //<parameter name="pRatio" value="0.00001"/>
                    //<parameter name="FDR" value="0.01"/>
                       
                       
                    //</search_hit>



                    spectrum_query.AppendChild(search_result);

                    msms_run_summary.AppendChild(spectrum_query);
                    iCounterIds++;
                }
            }



            doc.Save(filename);

        }

        private static void integrateModsFromXML(string modificationsFile,
                                                ArrayList modifs,
                                                DataSet ds,
                                                DA_paramsReader.paramsData.massType massType)
        {
            ArrayList modsFromXML = new ArrayList();

            modsFromXML = DA_fileReader.extractModifications(modifs, modificationsFile);

            string[] occ;
            float[] miw;
            float[] aiw;
            getAAmassesFromDataSet(ds, out occ, out miw, out aiw);

            foreach (Modification xmlMod in modsFromXML)
            {
                Modifications srfMod = new Modifications();
                srfMod.aminoacid = xmlMod.aminoacid.ToString();
                srfMod.deltaMass = (float)xmlMod.deltaMass;
                srfMod.symbol = xmlMod.symbol;
                srfMod.variable = true;

                for (int i = 0; i < occ.Length; i++)
                {
                    if (occ[i] == xmlMod.aminoacid.ToString())
                    {
                        switch (massType)
                        {
                            case DA_paramsReader.paramsData.massType.monoisotopic:
                                {
                                    srfMod.finalMass = (float)(miw[i] + xmlMod.deltaMass);
                                    break;
                                }
                            case DA_paramsReader.paramsData.massType.average:
                                {
                                    srfMod.finalMass = (float)(aiw[i] + xmlMod.deltaMass);
                                    break;
                                }
                        }

                        break;
                    }
                }

                modifs.Add(srfMod);
            }
        }


        public static void writeWidth(  string foldername,
                                        double width,
                                        ArrayList mu_pIperFraction,
                                        int totBatches,
                                        int numBatch)
        {
            DirectoryInfo di = Directory.GetParent(foldername);

            string folderFullName = di.FullName;

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_pIwidth";
            if (totBatches > 1) filename += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filename += ".xls";
            string str_base_name = foldername.Trim();
            
            StreamWriter sw = new StreamWriter(filename, false);

            sw.Write("pI width: ");
            sw.Write("\t");
            sw.Write(width.ToString());
            sw.Write("\t");
            sw.Write(sw.NewLine);
            sw.Write(sw.NewLine);
            sw.Write("fraction");
            sw.Write("\t");
            sw.Write("mu pI");
            sw.Write("\t");
            sw.Write("stDev");
            sw.Write(sw.NewLine);

            foreach (pIfraction pif in mu_pIperFraction)
            {
                sw.Write(pif.RAWFileName.ToString());
                sw.Write("\t");
                sw.Write(pif.mu.ToString());
                sw.Write("\t");
                sw.Write(pif.stDev.ToString());
                sw.Write(sw.NewLine);

            }

            sw.Close();



 
        }


        public static void writeCSV(ArrayList targetList,
                                    double FDR,
                                    string foldername,
                                    int totBatches,
                                    int numBatch)
        {
            DirectoryInfo di = Directory.GetParent(foldername);

            string folderFullName = di.FullName;

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_results";
            if (totBatches > 1) filename += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filename += ".xls";
            string str_base_name = foldername.Trim();



            StreamWriter sw = new StreamWriter(filename, false);
            // First we write the headers.
            sw.Write("FileName");
            sw.Write("\t");
            sw.Write("RAWFile");
            sw.Write("\t");
            sw.Write("FirstScan");
            sw.Write("\t");
            sw.Write("LastScan");
            sw.Write("\t");
            sw.Write("Charge");
            sw.Write("\t");
            sw.Write("pRatio");
            sw.Write("\t");
            sw.Write("FDR");
            sw.Write("\t");
            sw.Write("FASTAProteinDescription");
            sw.Write("\t");
            sw.Write("Sequence");
            sw.Write("\t");
            sw.Write("pI");
            sw.Write("\t");
            sw.Write("PrecursorMass");
            sw.Write("\t");
            sw.Write("Xcorr1Search");
            sw.Write("\t");
            sw.Write("Xcorr1Original");
            sw.Write("\t");
            sw.Write("Xcorr2Search");
            sw.Write("\t");
            sw.Write("DeltaCn");
            sw.Write("\t");
            sw.Write("Sp");
            sw.Write("\t");
            sw.Write("SpRank");
            sw.Write("\t");
            sw.Write("ProteinsWithPeptide");
            sw.Write("\t");
            sw.Write("Redundances");
            sw.Write(sw.NewLine);

            //Write the data
            for (int i = 0; i < targetList.Count; i++)
            {
                OutData od = (OutData)targetList[i];

                if (od.FDR <= FDR)
                {
                    try
                    {
                        sw.Write(od.FileName.ToString());
                        sw.Write("\t");
                        sw.Write(od.RAWFile.ToString());
                        sw.Write("\t");
                        sw.Write(od.FirstScan.ToString());
                        sw.Write("\t");
                        sw.Write(od.LastScan.ToString());
                        sw.Write("\t");
                        sw.Write(od.Charge.ToString());
                        sw.Write("\t");
                        sw.Write(od.pRatioTarget.ToString());
                        sw.Write("\t");
                        sw.Write(od.FDR.ToString());
                        sw.Write("\t");
                        sw.Write(od.ProteinDescription.ToString());
                        sw.Write("\t");
                        sw.Write(od.Sequence.ToString());
                        sw.Write("\t");
                        sw.Write(od.pI.ToString());
                        sw.Write("\t");
                        sw.Write(od.PrecursorMass.ToString());
                        sw.Write("\t");
                        sw.Write(od.Xcorr1Search.ToString());
                        sw.Write("\t");
                        sw.Write(od.Xcorr1Original.ToString());
                        sw.Write("\t");
                        sw.Write(od.Xcorr2Search.ToString());
                        sw.Write("\t");
                        sw.Write(od.DeltaCn.ToString());
                        sw.Write("\t");
                        sw.Write(od.Sp.ToString());
                        sw.Write("\t");
                        sw.Write(od.SpRank.ToString());
                        sw.Write("\t");
                        sw.Write(od.ProteinsWithPeptide.ToString());
                        sw.Write("\t");

                        if (od.ProteinsWithPeptide > 0)
                        {

                            foreach (pRatio.Redundance myRed in od.Redundances)
                            {
                                try
                                {
                                    sw.Write(myRed.FASTAIndex.ToString());
                                    sw.Write(" ");
                                    sw.Write(myRed.FASTAProteinDescription.ToString());
                                    sw.Write(" -- ");

                                }
                                catch { }
                            }

                            //sw.Write("\t");

                        }


                    }
                    catch
                    {
                    }

                    sw.Write(sw.NewLine);
                }
            }

            sw.Close();
        }

        public static void writeArrayLists(ArrayList targetList,
                                            ArrayList decoyList,                                
                                            string foldername,
                                            int totBatches,
                                            int numBatch)
        {
            DirectoryInfo di = Directory.GetParent(foldername);

            string folderFullName = di.FullName;

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_ArrayLists";
            if (totBatches > 1) filename += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filename += ".xls";

            string filenameDecoy = foldername.Trim() + "\\" + di.Name.Trim() + "_ArrayListsDecoy";
            if (totBatches > 1) filenameDecoy += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filenameDecoy += ".xls";

            string str_base_name = foldername.Trim();




            StreamWriter sw = new StreamWriter(filename, false);
            StreamWriter swD = new StreamWriter(filenameDecoy, false);

            // First we write the headers.
            sw.Write("FileName");
            sw.Write("\t");
            sw.Write("FirstScan");
            sw.Write("\t");
            sw.Write("Charge");
            sw.Write("\t");
            sw.Write("Sequence");
            sw.Write("\t");
            sw.Write("Mass");
            sw.Write("\t");
            sw.Write("pI");
            sw.Write("\t");
            sw.Write("pRatioTarget");
            sw.Write("\t");
            sw.Write("pRatioDecoy");
            sw.Write("\t");
            sw.Write("FDR");
            sw.Write("\t");
            sw.Write("Xc1");
            sw.Write("\t");
            sw.Write("rankXc1");
            sw.Write("\t");
            sw.Write("rankXc2");
            sw.Write(sw.NewLine);

            swD.Write("FileName");
            swD.Write("\t");
            swD.Write("FirstScan");
            swD.Write("\t");
            swD.Write("Charge"); 
            swD.Write("\t");
            swD.Write("Sequence");
            swD.Write("\t");
            swD.Write("Mass");
            swD.Write("\t");
            swD.Write("pI");
            swD.Write("\t");
            swD.Write("pRatioTarget");
            swD.Write("\t");
            swD.Write("pRatioDecoy");
            swD.Write("\t");
            swD.Write("FDR");
            swD.Write("\t");
            swD.Write("Xc1");
            swD.Write("\t");
            swD.Write("rankXc1");
            swD.Write("\t");
            swD.Write("rankXc2");
            swD.Write(sw.NewLine);

            //Write the data
            for (int i = 0; i < targetList.Count; i++)
            {
                OutData od =(OutData) targetList[i];
                sw.Write(od.FileName.ToString());               
                sw.Write("\t");
                sw.Write(od.FirstScan.ToString());
                sw.Write("\t");
                sw.Write(od.Charge.ToString());
                sw.Write("\t");
                sw.Write(od.Sequence.ToString());
                sw.Write("\t");
                sw.Write(od.Mass.ToString());
                sw.Write("\t");
                sw.Write(od.pI.ToString());
                sw.Write("\t");
                sw.Write(od.pRatioTarget.ToString());
                sw.Write("\t");
                sw.Write(od.pRatioDecoy.ToString());
                sw.Write("\t");
                sw.Write(od.FDR.ToString());
                sw.Write("\t");
                sw.Write(od.Xcorr1Search.ToString());
                sw.Write("\t");
                sw.Write(od.rnkXc1.ToString());
                sw.Write("\t");
                sw.Write(od.rnkXcRandom.ToString());
                sw.Write(sw.NewLine);
                
            }
            
            sw.Close();


            for (int i = 0; i < decoyList.Count; i++)
            {
                OutData od = (OutData)decoyList[i];
                swD.Write(od.FileName.ToString());
                swD.Write("\t");
                swD.Write(od.FirstScan.ToString());
                swD.Write("\t");
                swD.Write(od.Charge.ToString());
                swD.Write("\t");
                swD.Write(od.Sequence.ToString());
                swD.Write("\t");
                swD.Write(od.Mass.ToString());
                swD.Write("\t"); 
                swD.Write(od.pI.ToString());
                swD.Write("\t");
                swD.Write(od.pRatioTarget.ToString());
                swD.Write("\t");
                swD.Write(od.pRatioDecoy.ToString());
                swD.Write("\t");
                swD.Write(od.FDR.ToString());
                swD.Write("\t");
                swD.Write(od.Xcorr1Search.ToString());
                swD.Write("\t");
                swD.Write(od.rnkXc1.ToString());
                swD.Write("\t");
                swD.Write(od.rnkXcRandom.ToString());
                swD.Write(sw.NewLine);

            }

            swD.Close();          
               

        }


        public static void writeALJoined(ArrayList allJoinedList,
                                        string foldername,
                                        int totBatches,
                                        int numBatch)
        {
            DirectoryInfo di = Directory.GetParent(foldername);

            string folderFullName = di.FullName;

            string filename = foldername.Trim() + "\\" + di.Name.Trim() + "_ALJoined";
            if (totBatches > 1) filename += (numBatch + 1).ToString() + "of" + totBatches.ToString();
            filename += ".xls";

            string str_base_name = foldername.Trim();
            
            StreamWriter sw = new StreamWriter(filename, false);
            // First we write the headers.
            sw.Write("FileName");
            sw.Write("\t");
            sw.Write("FirstScan");
            sw.Write("\t");
            sw.Write("Charge");
            sw.Write("\t");
            sw.Write("Mass");
            sw.Write("\t");
            sw.Write("pRatioTarget");
            sw.Write("\t");
            sw.Write("pRatioDecoy");
            sw.Write("\t");
            sw.Write("pI");
            sw.Write("\t");
            sw.Write("FDR");
            sw.Write("\t");
            sw.Write("XC");
            sw.Write("\t");
            sw.Write("DeltaCn");
            sw.Write("\t");
            sw.Write("D o I");
            sw.Write("\t");
            sw.Write("best?");

            sw.Write(sw.NewLine);

            //Write the data
            for (int i = 0; i < allJoinedList.Count; i++)
            {
                OutData od = (OutData)allJoinedList[i];
                sw.Write(od.FileName.ToString());
                sw.Write("\t");
                sw.Write(od.FirstScan.ToString());
                sw.Write("\t");
                sw.Write(od.Charge.ToString());
                sw.Write("\t");
                sw.Write(od.Mass.ToString());
                sw.Write("\t");
                sw.Write(od.pRatioTarget.ToString());
                sw.Write("\t");
                sw.Write(od.pRatioDecoy.ToString());
                sw.Write("\t");
                sw.Write(od.pI.ToString());
                sw.Write("\t");
                sw.Write(od.FDR.ToString());
                sw.Write("\t");
                sw.Write(od.Xcorr1Search.ToString());
                sw.Write("\t");
                // former deltaCn for ALJoined, WRONG
                // float deltaCn = (od.Xcorr1Search - od.Xcorr2Search) / (od.Xcorr1Search + od.Xcorr2Search);
                sw.Write(od.DeltaCn.ToString());
                sw.Write("\t");
                string DoI = "";
                string best = " ";
                switch (od.dbType)
                {
                    case OutData.databases.Target:
                        {
                            DoI = "D";
                                                           
                            break;
                        }
                    case OutData.databases.Decoy:
                        {
                            DoI = "I";
                            break;
                        }
                }
                if (od.Xcorr1Search >= od.XCorr1SearchOther) best = "1"; 

                sw.Write(DoI);
                sw.Write("\t");
                sw.Write(best);
                sw.Write(sw.NewLine);

            }

            sw.Close();
          
                
        }
    }
}
