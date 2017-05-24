using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace pRatio
{

    public class Pkgroup
    {
        public Pkgroup()
        { }
        
        public string group
        {
            get { return groupVal; }
            set { groupVal = value; }
        }
        public int n
        {
            get { return nVal; }
            set { nVal = value; }
        }
        public float pKa
        {
            get { return pKaVal; }
            set { pKaVal = value; }
        }
        public ionizType charge
        {
            get { return chargeVal; }
            set { chargeVal = value; }
        }

        public bool required
        {
            get { return requiredVal; }
            set { requiredVal = value; }
        }

        private string groupVal;
        private int nVal;
        private float pKaVal;
        private ionizType chargeVal;
        private bool requiredVal;

        public enum ionizType
        {
            positive,
            negative
        }
    }

    public class Isoelectric
    { 
        public static float calpI(DataSet ds, string sequence)
        {

           
            ArrayList posGroups = new ArrayList();
            ArrayList negGroups = new ArrayList();

            string sSequence = sequence.Trim();
            char[] seps = new char[] { '.' };
            string[] sSeqSplit = sSequence.Split(seps);

            char[] seq = new char[0];

            if (sSeqSplit.Length == 3)
                seq = sSeqSplit[1].ToCharArray();
            else
                seq = sSeqSplit[0].ToCharArray();


            DataView dv = new DataView(ds.Tables["aminoacid"]);
            DataView dvg = new DataView(ds.Tables["group"]);

 
            for (int i = 0; i < dv.Count; i++)
            {
                string charge = dv[i].Row["charge"].ToString().Trim(); 
                string occ=dv[i].Row["one_char_code"].ToString();
                
                //Add to groupList
                Pkgroup currGroup = new Pkgroup();
                currGroup.group = occ;
                currGroup.n = 0;
                
                currGroup.required = false;
                currGroup.pKa = float.Parse(dv[i].Row["pKa"].ToString());

              
                foreach (char ch in seq)
                {
                    if (ch.ToString() == currGroup.group)
                    {
                        currGroup.n++;              
                    }
                }

                if (charge == "positive")
                {
                    currGroup.charge = Pkgroup.ionizType.positive;
                    posGroups.Add(currGroup);
                }
                if (charge == "negative")
                {
                    currGroup.charge = Pkgroup.ionizType.negative;
                    negGroups.Add(currGroup);
                }
    

            }

            for (int i = 0; i < dvg.Count; i++)
            {
                string charge = dvg[i].Row["charge"].ToString().Trim();
                string name = dvg[i].Row["name"].ToString();
                string required = dvg[i].Row["required"].ToString();

                //Add to groupList
                Pkgroup currGroup = new Pkgroup();
                currGroup.group = name;
                currGroup.n = 0;

                currGroup.required = false;
                if (required == "yes")
                {
                    currGroup.required = true;
                    currGroup.n = 1;
                }

                currGroup.pKa = float.Parse(dvg[i].Row["pKa"].ToString());
                
 
                if (charge == "positive")
                {
                    currGroup.charge = Pkgroup.ionizType.positive;
                    posGroups.Add(currGroup);
                }
                if (charge == "negative")
                {
                    currGroup.charge = Pkgroup.ionizType.negative;
                    negGroups.Add(currGroup);
                }

 
            }

           //calculate the pI... two rounds variing pH
            // first round: pH 1 --> 14 Delta_pH = 0.25
            // second round: pH (best from first round - 0.25) --> (best from first round + 0.25) delta_pH =0.25/20 
            // if Bolzano condition is accomplished, break & recalculate around the point (triangulation to calculate it)
            // if Bolzano condition is not accomplished, just calculate around the point closer to zero.
            int totalRounds = 2;
            float delta_pH = 0.25F;
            float best_pH = 1;
            float pH1 = 1;
            float pH2 = 14;
            float pepCharges1 = 0;
            float pepCharges2 = 0;
            float best_pepCharges = 10000.0F;
            float concH1=0;
            float concH2 = 0;
            float down_pH = 1;
            float up_pH = 14;
            int sign1 = 0;
            int sign2 = 0;
            bool bolzano = false;

            for (int round = 1; round <= totalRounds; round++)
            {                
                for (float fpH = down_pH; fpH < up_pH; fpH = fpH + delta_pH)
                {
                    //pH=-log[H+]
                    concH1 = (float)Math.Pow(10, -fpH);
                    concH2 = (float)Math.Pow(10, -(fpH + delta_pH));

                    pepCharges1 = 0;
                    pepCharges2 = 0;
                    foreach (Pkgroup gr in posGroups)
                    {
                        pepCharges1 += (float)gr.n*concH1/(concH1+(float)Math.Pow(10,-gr.pKa));
                        pepCharges2 += (float)gr.n * concH2 / (concH2 + (float)Math.Pow(10, -gr.pKa));
                    }
                    foreach (Pkgroup gr in negGroups)
                    {
                        pepCharges1 += (float)gr.n * (concH1 / (concH1 + (float)Math.Pow(10, -gr.pKa)) - 1);
                        pepCharges2 += (float)gr.n * (concH2 / (concH2 + (float)Math.Pow(10, -gr.pKa)) - 1);

                    }

                    //Check for a sign change (Bolzano)
                    sign1 = Math.Sign(pepCharges1);
                    sign2 = Math.Sign(pepCharges2);
                    if (sign1 != sign2) 
                    {
                        bolzano = true;
                        pH1 = fpH;
                        pH2 = fpH + delta_pH;
                        break;
                    }


                    if (Math.Abs(pepCharges1) <= Math.Abs(best_pepCharges)) 
                    {
                        best_pepCharges = pepCharges1;
                        best_pH = fpH;
                    }
                    
                }
                
                //Center the sweep in the best value
                //if Bolzano is accomplished, then linear fit to find the correct point
                if (bolzano) 
                {
                    float z0;
                    float b;

                    b = (pepCharges2 - pepCharges1) / (pH2 - pH1);
                    z0 = pepCharges2 - b * pH2;

                    //pH(z=0) == best_pH
                    best_pH = -z0 / b;


                }
                
                down_pH = best_pH - delta_pH;
                up_pH = best_pH + delta_pH;
                delta_pH /= 20;
                

            }

            if (best_pH < 1.0F) best_pH = 1.0F;
            if (best_pH > 14.0F) best_pH = 14.0F;
            

            return best_pH;
        }
    
    }
}
