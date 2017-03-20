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
        public int AnzPlanningObjects { get; set; }

        public static Dictionary<Guid, double> TargetValueByClient = new Dictionary<Guid, double>();
     

        public double MediationResult { get; set; }

        public int Maxrounds { get; set; }
        public int AnzVorschlaegeProRunde { get; set; } = Optimizer.childsCount;
        public int AnzErzwungeneAkzeptanz { get; set; }

        //private static String OutputFilepath = Directory.GetCurrentDirectory() + "\\" + "NepoLog.txt";
        // Ein gemeinsames Logfile im TMP Verzeichnis
        private static String OutputFilepath = Environment.GetEnvironmentVariable("TMP") + "\\" + "NepoLog.txt";

        private static Mutex mut = new Mutex(false, "print");
        public Guid agentConfigID { get; set; }


        public Logger()
        {
            // Print Header if File does not exist
            if (!File.Exists(OutputFilepath))
            {
                WriteLnToFile("Type\tAgentConfig\tTestInstanzID\tMaxRounds\tVorschlaegeProRunde\tErzwungeneAkzeptanz\tAnzahlPlanningObjects\tClientID\tTargetValue");
            }
        }

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
            }
            else
            {
                TargetValueByClient.Remove(guid);
                TargetValueByClient.Add(guid,targetValue);
            }
        }

        /// <summary>
        /// 
        /// Write a line Threadsafe to File
        /// 
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
                        //file.WriteLine(value);
                        file.Write(value);
                        file.Close();
                    }
            }
            finally
            {
                //Release Lock
                _readWriteLock.ExitWriteLock();
            }

        }

        /// <summary>
        /// 
        /// Write a line Threadsafe to File
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteLnToFile(String value)
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



        /// <summary>
        /// 
        /// Typ	AgentConfig	TestinstanzID	MaxRounds	VorschlaegeProRunde	ErzwungeneAkzeptanz	AnzahlTuerme	ClientID	TargetValue
        /// 
        /// </summary>
        public void finish()
        {
            // Wait until it is safe to enter.
            mut.WaitOne();
            lock (thisLock)
            {
                if (localOptimization)
                {
                    WriteToFile("local\t");
                }
                else
                {
                    WriteToFile("remote\t");
                }
                WriteToFile(agentConfigID+"\t");
                WriteToFile(Testinstanzguid+"\t");
                WriteToFile(Maxrounds+"\t");
                WriteToFile(AnzVorschlaegeProRunde+"\t");
                WriteToFile(AnzErzwungeneAkzeptanz+"\t");
                WriteToFile(AnzPlanningObjects+"\t");

                foreach (var item in TargetValueByClient)
                {
                    WriteToFile(item.Key + "\t" + item.Value);
                }
                WriteLnToFile("");
                
            }
            // Release the Mutex.
            mut.ReleaseMutex();
        }

        public void DebugDump()
        {
            // Wait until it is safe to enter.
            mut.WaitOne();
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
                    WriteToFile("AgentConfigID=" + agentConfigID);
                    WriteToFile("TestinstGUID=" + Testinstanzguid);
                    WriteToFile("MaxRounds=" + Maxrounds);
                    WriteToFile("AnzVorschlaegeProRunde=" + AnzVorschlaegeProRunde);
                    WriteToFile("AnzErzwungeneAkzeptanz=" + AnzErzwungeneAkzeptanz);
                    WriteToFile("Anzahl Tuerme=" + AnzPlanningObjects);

                    foreach (var item in TargetValueByClient)
                    {
                        WriteToFile("CLIENT=" + item.Key + ":targetValue=" + item.Value);
                    }
                
                    WriteToFile("################# END ENTRY ####################");
                
            }
            // Release the Mutex.
            mut.ReleaseMutex();
        }

    }

}
