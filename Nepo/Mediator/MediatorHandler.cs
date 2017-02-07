using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nepo.DataGenerator;
using Nepo.Common;

namespace Mediator
{
    public class MediatorHandler
    {

        //private Instance Instance = null;
        private readonly Instance Instance = new Instance();
        private List<Guid> AgentList = new List<Guid>();
        private MediatorService service;
        private DecisionHandler DecisonHandler = new DecisionHandler();
        private List<Solution> currentSolution = new List<Solution>();


        public MediatorHandler(MediatorService service)
        {
            this.service = service;
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
            return Instance;
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
            return currentSolution;
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

            // Alle Teilnehmer sind angemeldet

            // Prüfe, ob jeder Teilnehmer abgestimmt hat
            if (allClientsVoted())
            {
                // Alle Abstimmungen erhalten -> Mutiere Lösung und rufe Callback auf
                this.currentSolution = generateNewSolution();

                // Mediator is Ready for Clients
                service.DataReadyCallback(CanIHasPope.BlackSmoke);
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

            var config = new MapConfig()
            {
                MapSize = new System.Drawing.Size(500, 500),
                PlanningObjectCount = 5
            };
            Random rand = new Random((int)DateTime.Now.Ticks);
            config.ImmovableObjects = new List<ImmovableObject>();
            for (int i = 0; i < 20; i++)
            {
                config.ImmovableObjects.Add(new ImmovableObject()
                {
                    Location = new System.Drawing.Point(rand.Next(500), rand.Next(500)),
                    Weight = rand.Next(100, 150)
                });
            }
            DataHandler.SaveMapConfig(config);

            Instance instance = new Instance();
            instance.Map = config;

            return instance;

        }


    }
}