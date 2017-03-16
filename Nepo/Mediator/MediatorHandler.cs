using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Nepo.DataGenerator;
using Nepo.Common;
using Nepo.Common.Rules;
using System.Threading;

namespace Mediator
{
    public class MediatorHandler
    {

        private Instance _inst;
        public Instance Instance { get { return _inst; } set { _inst = value;Optimizer.Instance.SetMap(_inst.Map); } }
        private List<Guid> AgentList = new List<Guid>();
        private MediatorService service;
        private DecisionHandler DecisonHandler = new DecisionHandler();
        private List<Solution> currentSolution = new List<Solution>();
        private int maxRound = 1000;

        private static MediatorHandler _instance;
        public static MediatorHandler HandlerInstance { get { return _instance; } }
        public bool Ready { get; set; }
        public event EventHandler NewDataAvailable;


        public MediatorHandler(MediatorService service)
        {
            this.service = service;
            Optimizer.maxRounds = maxRound;
            _instance = this;
            Logger.Maxrounds = maxRound;
            Ready = true;
            //if (Instance == null)
            //{
            //    Instance = Session.Get.CurrentInstance;//InitInstance(MediatorConfig.Get.TestMode);
            //    Optimizer.Instance.SetMap(Instance.Map);
            //}
        }

        public void Reset()
        {
            DecisonHandler.Reset();
            Optimizer.Instance.Reset();
            service.DataReadyCallback(CanIHasPope.BlackSmoke);
            NewDataAvailable?.Invoke(null, null);
        }

        /// <summary>
        /// Registriere Client-Agenten und liefere initiale Lösung zurück
        /// 
        /// </summary>
        /// <param name="agentGuid"></param>
        /// <returns></returns>ö
        public Instance Register(Guid agentGuid)
        {
            this.AgentList.Add(agentGuid);

            //if(Instance == null)
            //{
            //    Instance = InitInstance(MediatorConfig.Get.TestMode);
            //    Optimizer.Instance.SetMap(Instance.Map);
            //}

            while (null == Instance)
                Thread.Sleep(100);

            Logger.AnzTuerme = Instance.Map.PlanningObjectCount;
            Logger.PrintAnzTuerme();

            return Instance;
        }

        public void Unregister(Guid agentGuid)
        {
            this.AgentList.Remove(agentGuid);
        }

        public Solution GetCurrentSolution(Guid agentGuid)
        {
            return Optimizer.Instance.SelectChild(0).Item1;
        }


        /// <summary>
        /// 
        /// Called if Client receives <paramm name="NewDataEventArgs"></paramm> to get
        /// new Solutions
        /// 
        /// </summary>
        /// <param name="agentGuid"></param>
        /// <returns></returns>
        public List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            return Optimizer.Instance.SelectChild(0).Item2;
        }



        /// <summary>
        /// Client liefert die Lösung mit deren Bewertung zurück. 
        /// </summary>
        /// <param name="votes"></param>
        internal void Vote(Guid guid, List<Tuple<int, bool>> votes)
        {
            DecisonHandler.saveVote(guid, votes);
            
            // Prüfe, ob mindestens zwei Teilnehmer (Provider, Client) vorhanden
            if (AgentList.Count < 2)
            {
                // Es sind noch nicht alle Teilnehmer am Mediator angemeldet
                // Speichere Vote des Teilnehmers in Struktur
                return;

            }
   
                // Alle angemeldet -> Initial Solution
                //this.currentSolution = generateNewSolution();
                //service.DataReadyCallback(CanIHasPope.BlackSmoke);

            // Prüfe, ob jeder Teilnehmer abgestimmt hat
            if (AllClientsVoted())
            {
                Optimizer.Instance.FindNewAcceptedSolution(DecisonHandler.GetVotesForRound());

                // Nächste Runde
                DecisonHandler.newRound();

                if(DecisonHandler.CurrentRound > maxRound)
                {
                    // Abstimmung beendet
                    service.DataReadyCallback(CanIHasPope.WhiteSmoke);
                    NewDataAvailable?.Invoke(null, null);
                }
                else
                {
                    // Es folgt eine weitere Abstimmungsrunde
                    service.DataReadyCallback(CanIHasPope.BlackSmoke);
                    NewDataAvailable?.Invoke(null, null);
                }

            }
        }

        /// <summary>
        /// Check if all Clients commited their votes
        /// </summary>
        /// <returns></returns>
        private bool AllClientsVoted()
        {
            Boolean all_ready = true;
            foreach (var item in AgentList)
            {
                if (!DecisonHandler.hasClientVoted(item))
                {
                    all_ready = false;
                }
            }

            return all_ready;
        }

   


        /// <summary>
        /// 
        /// Initialisiere eine Instanz (aus Testdatensätzen) bzw. Mock mit Random Zahlen
        ///     TODO REPLACE MOCK mit SampleData
        /// 
        /// </summary>
        private Instance InitInstance(bool testmode)
        {

            // Maximum 7 Towers for debugging
            GenerationConfig conf = new GenerationConfig();
            conf.Constraints.MaxPlanningObjectCount = 7;

            if (testmode)
            {
                conf.InstanceCount = 1;
                conf.AgentsCount = 2;
            }
            List<Instance> liste = Generator.GenerateInstances(conf).Result;
            
            
            //Logger.AnzTuerme = liste[0].Map.PlanningObjectCount;
            //Logger.PrintAnzTuerme();

            return liste[0];
        }


    }
}