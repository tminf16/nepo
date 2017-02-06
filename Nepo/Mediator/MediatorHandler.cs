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


        /// <summary>
        /// Registriere Client-Agenten und liefere initiale Lösung zurück
        /// 
        /// </summary>
        /// <param name="agentGuid"></param>
        /// <returns></returns>
        public Instance Register(Guid agentGuid)
        {
            this.AgentList.Add(agentGuid);

            if(null == Instance)
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

            for (int i = 0; i< 10; i++)
            {
                tmp = new Solution();
                tmp.SolutionID = i;

                for(int k=0; k<5; k++)
                {
                    tmp.PlanningObjects[k].Location = new System.Drawing.Point(10,20);
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
        internal void Vote(List<Tuple<int, bool>> votes, Guid agentGuid)
        {
            // Prüfe, ob alle abgestimmt haben (über Anzahl der vorhandenen Liste
                // Wenn nein, warten
                // Wenn ja, wähle die Top Lösung aus und mutiere.
                    // Rufe Callback auf

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