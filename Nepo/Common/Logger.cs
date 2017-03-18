using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nepo.Common
{

    public class Logger
    {

        private static Logger _instance = null;
        public static Logger Get { get { return _instance ?? (_instance = new Logger()); } }

        private readonly Object thisLock = new Object();

        public Boolean localOptimization { get; set; } = false;

        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public Guid Testinstanzguid { get; set; }
        public int AnzTuerme { get; set; }


        public Dictionary<Guid, double> TargetValueByClient = new Dictionary<Guid, double>();

        public double MediationResult { get; set; }

        public int Maxrounds { get; set; }
        public int AnzVorschlaegeProRunde { get; set; } = Optimizer.childsCount;
        public int AnzErzwungeneAkzeptanz { get; set; }

        //private static String OutputFilepath = Directory.GetCurrentDirectory() + "\\" + "NepoLog.txt";
        // Ein gemeinsames Logfile im TMP Verzeichnis
        private static String OutputFilepath = Environment.GetEnvironmentVariable("TMP") + "\\" + "NepoLog.txt";


        /// <summary>
        /// Target Value of Client after Habemus Papam
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="targetValue"></param>
        public void AddMyTargetValue(Guid guid, double targetValue)
        {

            if (!TargetValueByClient.ContainsKey(guid))
            {
                TargetValueByClient.Add(guid, targetValue);
                //WriteToFile("CLIENT=" + guid + ":targetValue=" + targetValue);
            }

        }

        public void PrintGUID()
        {
            Console.WriteLine("TestinstGUID=" + Testinstanzguid);
            WriteToFile("TestinstGUID=" + Testinstanzguid);
        }

        public void PrintTarget()
        {
        }

        public void PrintAnzTuerme()
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
        public void DumpResult()
        {

        }

        /// <summary>
        /// 
        /// TODO make Threadsafe
        /// </summary>
        /// <param name="value"></param>
        public void WriteToFile(String value)
        {

            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter file = File.AppendText(OutputFilepath))
                {
                    file.WriteLine(value);
                    file.Close();
                }
            }
            finally
            {
                //Release Lock
                _readWriteLock.ExitWriteLock();
            }

        }

        public void finish()
        {
            lock (thisLock)
            {
                if (localOptimization)
                {
                    WriteToFile("################# NEW LOCAL ENTRY ####################");

                }
                else
                {
                    WriteToFile("################# NEW REMTOE ENTRY ####################");
                }
                    WriteToFile("TestinstGUID=" + Testinstanzguid);
                    WriteToFile("MaxRounds=" + Maxrounds);
                    WriteToFile("AnzVorschlaegeProRunde=" + AnzVorschlaegeProRunde);
                    WriteToFile("AnzErzwungeneAkzeptanz=" + AnzErzwungeneAkzeptanz);
                    WriteToFile("Anzahl Tuerme=" + AnzTuerme);

                    foreach (var item in TargetValueByClient)
                    {
                        WriteToFile("CLIENT=" + item.Key + ":targetValue=" + item.Value);
                    }
                
                    WriteToFile("################# END ENTRY ####################");
                
            }
        }

    }

}
