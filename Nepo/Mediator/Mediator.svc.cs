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
        public Instance Register(Guid agentGuid)
        {
            throw new NotImplementedException();
        }


        public Tuple<List<Solution>, int> GetProposedSolutions(Guid agentGuid)
        {
            throw new NotImplementedException();
        }

        public void Vote(Tuple<int, bool> votes)
        {
            throw new NotImplementedException();
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
