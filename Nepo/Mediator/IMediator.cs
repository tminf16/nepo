
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMediator" in both code and config file together.
    [ServiceContract]
    public interface IMediator
    {
        /// <summary>
        /// Registers an agent with the mediator.
        /// </summary>
        /// <param name="agentGuid">Unique Id of the agent.</param>
        /// <returns>The inital instance.</returns>
        [OperationContract]
        Instance Register(Guid agentGuid);

        /// <summary>
        /// Gets a list of proposed solutions from the mediator.
        /// </summary>
        /// <param name="agentGuid">Unique Id of the agent.</param>
        /// <returns>A list of proposed solutions.</returns>
        [OperationContract]
        List<Solution> GetProposedSolutions(Guid agentGuid);

        /// <summary>
        /// Allows an agent to vote for proposed solutions.
        /// </summary>
        /// <param name="votes">Tuple, consisting of solutionId and if the agent agrees to the solution.</param>
        [OperationContract]
        void Vote(Tuple<int, bool> votes);

    }
}
