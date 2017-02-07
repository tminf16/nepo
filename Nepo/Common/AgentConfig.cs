using Nepo.Common.Rules;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nepo.Common
{
    [DataContract]
    public class AgentConfig
    {
        [DataMember]
        public List<Layer> Layers { get; set; }
        [DataMember]
        public List<ITargetFunctionComponent> Rules { get; set; }
        public AgentConfig()
        {
            Layers = new List<Layer>();
            Rules = new List<ITargetFunctionComponent>();
        }
    }
}