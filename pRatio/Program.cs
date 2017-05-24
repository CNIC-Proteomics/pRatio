using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace pRatio
{
   
    static class GUI
    {
        
            
        static public void run()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }

    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        //  http://www.csharp411.com/console-output-from-winforms-application/ 



        [STAThread]
        static void Main(string[] args)
        {
            if (args.GetLength(0) == 0)
            {
                GUI.run();

            }
            else
            {
                CLI.run();
            }
         
        }

    }


    static class CLI
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        static public void run()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            pRatio.frmMain pRatio = new frmMain(true);
        }
    }

   
   
    class CommandLineParser
    {

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        public CommandLineParser()
        {
            this.readCommandLineArgs();
            this.parseCommandLineArgs();
        }

        public Dictionary<string, object> getParameters()
        {
            return this.parameters;
        }

        public bool isDefined(string param)
        {
            return parameters.ContainsKey(param);
        }


        public void setDefault(string paramName, object value)
        {
            if (isDefined(paramName))
            {
                parameters.Remove(paramName);
            }
            parameters.Add(paramName, value);
        }

        public object getParameterByName(string paramName)
        {
            object value = null;
            try
            {
                parameters.TryGetValue(paramName, out value);
            }
            catch (System.ArgumentNullException ex)
            {
            }
            return value;
        }

        private void readCommandLineArgs()
        {
            String[] arguments = Environment.GetCommandLineArgs();

            for (int i = 1; i < arguments.GetLength(0); i++)
            {
                try
                {
                    switch (arguments[i])
                    {
                        case "-t":
                        case "-T":
                            parameters.Add("TargetSearch", (string)arguments[i + 1]);
                            break;

                        case "-d":
                        case "-D":
                            parameters.Add("DecoySearch", (string)arguments[i + 1]);
                            break;

                        case "-pI":
                        case "-PI":
                            parameters.Add("usePI", (bool)true);
                            break;

                        case "-FDRpI":
                        case "-FDRPI":
                            parameters.Add("FDRpI", (double)Double.Parse(arguments[i + 1]));
                            break;

                        case "-pepXML":
                        case "-PEPXML":
                            parameters.Add("pepXML", (bool)true);
                            break;

                        case "-SEQUESTparams":
                        case "-SEQUESTPARAMS":
                            parameters.Add("SEQUESTparamsFile", (string)arguments[i + 1]);
                            break;

                        case "-FDRmethod":
                        case "-FDRMETHOD":
                            parameters.Add("FDRMethod", (string)arguments[i + 1]);
                            break;

                        case "-FDRCutoff":
                        case "-FDRCUTOFF":
                            parameters.Add("FDRCutoff", (double)Double.Parse(arguments[i + 1]));
                            break;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    usageCommand();
                }
            }

        }


        private void usageCommand()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("pRatio.exe usage: pRatio.exe -t targetDirectory -d decoyDirectory ");
            System.Console.WriteLine("                            [-FDRMethod (mixed|separated|concatenated)] [-FDRCutoff number]");
            System.Console.WriteLine("                            [-pI ] [-FDRpI number ]");
            System.Console.WriteLine("                            [-pepXML  -SEQUESTParam paramFileName]");
            System.Console.WriteLine(); System.Console.WriteLine();
            System.Console.WriteLine("Default & hardcoded behavior");
            System.Console.WriteLine();
            System.Console.WriteLine("default -FDRMethod mixed.");
            System.Console.WriteLine("-FDRCutoff default value 0.05.");
            System.Console.WriteLine("using -pI parameter without setting a FDRpI will use FDRCutoff value.");

            System.Console.WriteLine();
            Environment.Exit(-1);

        }

        private void parseCommandLineArgs()
        {

            // mandatory parameters must exist.
            try   // target directory
            {
                if (!parameters.ContainsKey("TargetSearch"))
                {
                    throw new System.ApplicationException("pRatio.exe : Target Directory must be set after -t option");
                }
            }
            catch (System.ApplicationException ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(ex.Message);
                usageCommand();
            }

            try
            { //decoy directory
                if (!parameters.ContainsKey("DecoySearch"))
                {
                    throw new System.ApplicationException("pRatio.exe : Decoy Directory must be set after -d option");
                }
            }
            catch (System.ApplicationException ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(ex.Message);
                usageCommand();

            }


            // Files exist;
            if (!Directory.Exists((string)this.getParameterByName("TargetSearch")))
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Target search SRF folder not properly specified use -t directoryName");
                usageCommand();
            }


            if (!Directory.Exists((string)this.getParameterByName("DecoySearch")))
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Decoy search SRF folder not properly specified. Use -d directoryName");
                usageCommand();
            }

            if (this.isDefined("pepXML") && !File.Exists((string)this.getParameterByName("SEQUESTparamsFile")))
            {
                System.Console.WriteLine();
                System.Console.WriteLine("You have not selected a params file!");
                usageCommand();
            }


            // FDRCutoff value must be double between [0-1]

            double FDRcutOff = 0.5;

            if (this.isDefined("FDRCutoff"))
            {
                FDRcutOff = ( double ) this.getParameterByName("FDRCutoff");
                if (typeof(double) !=  this.getParameterByName("FDRCutoff").GetType())
                // if (!fdrOk)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("FDR-cutoff not properly specified (not a number)");
                    usageCommand();
                }
                else
                {
                    if (FDRcutOff < 0 || FDRcutOff > 10)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("FDR-cutoff not properly specified  ( 0 <= FDR-cutoff <= 1 )");
                        usageCommand();
                    }
                }
            }
            this.setDefault("FDRCutoff", (double)FDRcutOff);

            if (this.isDefined("usePI"))
            {
                double FDRpIcutoff = (double) this.getParameterByName("FDRpI");
                if (this.isDefined("FDRpI"))
                {

                   
                    if (typeof(double) != this.getParameterByName("FDRpI").GetType()) 
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("FDR cut-off for pI probability is not a number!");
                        usageCommand();
                    }
                    else
                    {
                        if (FDRpIcutoff < 0 || FDRpIcutoff > 1)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("FDR cut-off for pI probability is not a correct value!");
                            usageCommand();
                        }
                    }
                    this.setDefault("FDRpI", (double)FDRpIcutoff);
                }
            }
        }
    }
}

  

    