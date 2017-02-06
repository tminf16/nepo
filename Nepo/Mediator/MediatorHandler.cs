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
        private List<Guid> agentList = new List<Guid>();


        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="agentGuid"></param>
        /// <returns></returns>
        public Instance Register(Guid agentGuid)
        {
            if(null == Instance)
            {
                Instance = initInstance();
            }

            return Instance;
        }

       public  List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            throw new NotImplementedException();

        }

        internal void Vote(List<Tuple<int, bool>> votes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// Initialisiere eine Instanz (aus Testdatensätzen) bzw. Mock mit Random Zahlen
        /// 
        /// </summary>
        private Instance initInstance()
        {
            throw new NotImplementedException();
        }


    }
}