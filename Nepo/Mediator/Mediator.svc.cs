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

        public List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            throw new NotImplementedException();
        }

        public void Vote(Tuple<int, bool> votes)
        {
            throw new NotImplementedException();
        }
    }
}
