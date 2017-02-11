using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Nepo.DataGenerator;
using Nepo.Common;
using Nepo.Common.Rules;

namespace Mediator
{
    public class MediatorHandler
    {

        public Instance Instance;
        private List<Guid> AgentList = new List<Guid>();
        private MediatorService service;
        private DecisionHandler DecisonHandler = new DecisionHandler();
        private List<Solution> currentSolution = new List<Solution>();
        private int maxRound = 1000;

        private static MediatorHandler _instance;
        public static MediatorHandler HandlerInstance { get { return _instance; } }
        public event EventHandler NewDataAvailable;


        public MediatorHandler(MediatorService service)
        {
            this.service = service;
            Optimizer.maxRounds = maxRound;
            _instance = this;
            Logger.Maxrounds = maxRound;
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

            if(Instance == null)
            {
                Instance = InitInstance();
                Optimizer.Instance.SetMap(Instance.Map);
            }

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
        /// Generate a new Solution
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Solution> generateNewSolution()
        {
            List<Solution> Solution_Mock = new List<Solution>();
            Solution tmp;

            // Mock the Mutation
            for (int i = 0; i < 10; i++)
            {
                tmp = new Solution();
                tmp.SolutionID = i;

                PlanningObject[] po = { new PlanningObject(), new PlanningObject(), new PlanningObject() }; // creates populated array of length 2
                tmp.PlanningObjects = po;

                for (int k = 0; k < 3; k++)
                {
                    tmp.PlanningObjects[k].Location = new System.Drawing.Point(10, 20);
                }
                Solution_Mock.Add(tmp);
            }
            return Solution_Mock;
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
            if (allClientsVoted())
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
        private bool allClientsVoted()
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
        private Instance InitInstance()
        {

            // Maximum 7 Towers for debugging
            GenerationConfig conf = new GenerationConfig();
            conf.Constraints.MaxPlanningObjectCount = 7;

            List<Instance> liste = Generator.GenerateInstances(conf).Result;
            //liste[0].Map.ImmovableObjects = liste[0].Map.ImmovableObjects.Take(1).ToList();
            liste[0].AgentConfigs = new List<AgentConfig>();
            liste[0].AgentConfigs.Add(
                new AgentConfig()
                {
                    Rules = new List<TargetFunctionComponentBase>()
                });
            var rule = new DistanceIntervalsRule();
            rule.AddInterval(15, 100, 1);
            liste[0].AgentConfigs.ElementAt(0).Rules.Add(
                        rule);

            Logger.AnzTuerme = liste[0].Map.PlanningObjectCount;
            Logger.printAnzTuerme();

            return liste[0];
        }


    }
}