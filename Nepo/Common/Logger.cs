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
        public static int anzVorschlaegeProRunde { get; set; }
        public static int anzErzwungeneAkzeptanz { get; set; }

        private static String outputFilepath = "C:\\tmp\\sample.txt";

        public static void printGUID()
        {
            Console.WriteLine("TestinstGUID="+testinstanzguid);
        }

        public static void printAnzTuerme()
        {
            Console.WriteLine("Anzahl Tuerme=" + anzTuerme);
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
            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            using (StreamWriter file = new StreamWriter(@outputFilepath))
            {
                file.WriteLine(value);
            }

        }


    }





}
