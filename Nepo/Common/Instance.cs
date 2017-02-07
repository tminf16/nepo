using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nepo.Common
{
    [DataContract]
    public class Instance
    {
        [DataMember]
        public Guid InstanceId;
        [DataMember]
        public MapConfig Map { get; set; } = new MapConfig();
        [DataMember]
        public List<AgentConfig> AgentConfigs { get; set; }
    }
}