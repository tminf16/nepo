using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Nepo.Common;

namespace Mediator
{
    /// <summary>
    /// Handles WCF specific logic.
    /// </summary>
    public class MediatorService : IMediator
    {
        private static MediatorHandler handler;
        private static readonly Dictionary<Guid, IMediatorCallback> callbackChannels = new Dictionary<Guid, IMediatorCallback>();


        public MediatorService()
        {
            if(handler == null)
            {
                handler = new MediatorHandler(this);
            }
        }

        public Instance Register(Guid agentGuid)
        {
            callbackChannels.Add(agentGuid, OperationContext.Current.GetCallbackChannel<IMediatorCallback>());
            return handler.Register(agentGuid);
        }

        public List<Solution> GetProposedSolutions(Guid agentGuid)
        {
            return handler.GetProposedSolutions(agentGuid);
        }

        public void Vote(List<Tuple<int, bool>> votes, Guid agentGuid)
        {
            handler.Vote(agentGuid, votes);
        }

        public void DataReadyCallback(CanIHasPope popeState)
        {
            foreach (var callback in callbackChannels)
            {
                Task.Run(() => { callback.Value.DataReady(popeState); });
            }
        }
    }
}
