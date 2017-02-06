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

        private Instance Instance = null;
        private List<Guid> AgentList = new List<Guid>();
        private MediatorService service;
        private DecisionHandler DecisonHandler = new DecisionHandler();


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

            if (null == Instance)
            {
                Instance = InitInstance();
            }



            return Instance;
        }


        /// <summary>
        /// 
        /// Client verlangt nach einer neuen Lösung. Mediator liefert eine Liste mit
        /// akzeptierbaren Lösungen.
        /// 
        /// </summary>
        /// <param name="agentGuid"></param>
        /// <returns></returns>
        public List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            List<Solution> Solution_Mock = new List<Solution>();
            Solution tmp;

            for (int i = 0; i < 10; i++)
            {
                tmp = new Solution();
                tmp.SolutionID = i;

                for (int k = 0; k < 5; k++)
                {
                    tmp.PlanningObjects[k].Location = new System.Drawing.Point(10, 20);
                }
                Solution_Mock.Add(tmp);
            }

            Console.WriteLine("");
            // Generiere eine mutierte Lösung

            throw new NotImplementedException();
        }

        /// <summary>
        /// Client liefert die Lösung mit deren Bewertung zurück. 
        /// </summary>
        /// <param name="votes"></param>
        internal void Vote(Guid guid, List<Tuple<int, bool>> votes)
        {
            // Prüfe, ob mindestens zwei Teilnehmer vorhanden
            if (AgentList.Count < 2)
            {
                // Es sind noch nicht alle Teilnehmer angemeldet
                // Struktur mit (Rundennummer, Client, Abstimmergebnis)
                DecisonHandler.saveVote(guid, votes);
                return;
                // Prüfe, ob alle abgestimmt haben (über Anzahl der vorhandenen Liste
            }

            // Alle Teilnehmer sind angemeldet

            // Hat jeder Teilnehmer abgestimmt?
            Boolean all_ready = true;
            foreach (var item in AgentList)
            {
                if (!DecisonHandler.hasClientVoted(item))
                {
                    all_ready = false;
                }
            }

            if (all_ready)
            {
                // Alle Abstimmungen eingetroffen
            }
            else
            {
                // Nicht alle Abstimmungen eingetroffen
            }


            // Wenn nein, warten
            // Wenn ja, wähle die Top Lösung aus und mutiere.
            // Rufe Callback auf

            //service.DataReadyCallback(CanIHasPope.WhiteSmoke);

            throw new NotImplementedException();
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