using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{

    public class Logger
    {

        public static Guid Testinstanzguid { get; set; }
        public static int AnzTuerme { get; set; }


        public static Dictionary<Guid, double> TargetValueByClient = new Dictionary<Guid, double>();

        public static double MediationResult { get; set; }
        
        public static int Maxrounds { get; set; }
        public static int AnzVorschlaegeProRunde { get; set; } = Optimizer.childsCount;
        public static int AnzErzwungeneAkzeptanz { get; set; }

        private static String OutputFilepath = Directory.GetCurrentDirectory() + "\\" + "NepoLog.txt";


        /// <summary>
        /// Target Value of Client after Habemus Papam
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="targetValue"></param>
        public static void AddMyTargetValue(Guid guid, double targetValue)
        {

            if (!TargetValueByClient.ContainsKey(guid))
            {
                TargetValueByClient.Add(guid, targetValue);
                WriteToFile("CLIENT=" + guid + ":targetValue=" + targetValue);
            }

        }

        public static void PrintGUID()
        {
            Console.WriteLine("TestinstGUID="+Testinstanzguid);
            WriteToFile("TestinstGUID=" + Testinstanzguid);
        }

        public static void PrintTarget()
        {
        }

        public static void PrintAnzTuerme()
        {
            Console.WriteLine("Anzahl Tuerme=" + AnzTuerme);
            WriteToFile("Anzahl Tuerme=" + AnzTuerme);
        }


        // InstanceGUID
        // AnzTuerme
        // ClientABest	
        // ClientAMediation	
        // ClientBBest	
        // ClientBMediation
        // MaxRounds	
        // AnzVorschlaegeProRunde	
        // AnzErzwungeneAkzeptanz
        public static void DumpResult()
        {

        }

        public static void WriteToFile(String value)
        {


            // This text is added only once to the file.
            if (!File.Exists(OutputFilepath))
            {
                // Create a file to write to.
                using (StreamWriter file = File.CreateText(OutputFilepath))
                {
                    file.WriteLine(value);
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter file = File.AppendText(OutputFilepath))
            {
                file.WriteLine(value);
            }


        }


    }





}
