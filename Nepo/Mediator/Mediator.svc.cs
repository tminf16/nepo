using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Nepo.Common;

namespace Mediator
{
    /// <summary>
    /// Handles WCF specific logic.
    /// </summary>
    public class MediatorService : IMediator
    {

        private MediatorHandler handler = new MediatorHandler();
 
        public Instance Register(Guid agentGuid)
        {
            return handler.Register(agentGuid);
        }


        public List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            return handler.GetProposedSolutions(agentGuid);
        }

        public void Vote(List<Tuple<int, bool>> votes)
        {
            handler.Vote(votes);
        }
    }

    public class MediatorServiceCallback : IMediatorCallback
    {
        public void DataReady(CanIHasPope popeState)
        {
            throw new NotImplementedException();
        }
    }
}
