using Nepo.Common.Rules;
using System;
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
        public List<TargetFunctionComponentBase> Rules { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        public AgentConfig()
        {
            Layers = new List<Layer>();
            Rules = new List<TargetFunctionComponentBase>();
        }
    }
}