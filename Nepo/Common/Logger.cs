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

        public static Guid testinstanzguid { get; set; }
        public static int anzTuerme { get; set; }
        public static double clientABest { get; set; }
        public static double clientAMediation { get; set; }
        public static double clientBBest { get; set; }
        public static double clientBMediation { get; set; }
        public static int maxrounds { get; set; }
        public static int anzVorschlaegeProRunde { get; set; } = Optimizer.childsCount;
        public static int anzErzwungeneAkzeptanz { get; set; }

        private static String outputFilepath = "C:\\tmp\\sample.txt";

        public static void printGUID()
        {
            

            Console.WriteLine("TestinstGUID="+testinstanzguid);
            WriteToFile("TestinstGUID=" + testinstanzguid);
        }

        public static void printTarget()
        {
            WriteToFile("TargetVal=" + clientAMediation);
        }

        public static void printAnzTuerme()
        {
            Console.WriteLine("Anzahl Tuerme=" + anzTuerme);
            WriteToFile("Anzahl Tuerme=" + anzTuerme);
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
        public static void dumpResult()
        {

        }

        public static void WriteToFile(String value)
        {


            // This text is added only once to the file.
            if (!File.Exists(outputFilepath))
            {
                // Create a file to write to.
                using (StreamWriter file = File.CreateText(outputFilepath))
                {
                    file.WriteLine(value);
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter file = File.AppendText(outputFilepath))
            {
                file.WriteLine(value);
            }


        }


    }





}
