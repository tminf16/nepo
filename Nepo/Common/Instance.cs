using System;
using System.Collections.Generic;
using System.IO;
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

        public static List<Instance> LoadInstances()
        {
            List<Instance> result = new List<Instance>();
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            foreach (var folder in currentDir.GetDirectories())
            {
                Guid instanceId;
                if (!Guid.TryParse(folder.Name, out instanceId))
                    continue;
                var tmpInstance = new Instance();
                tmpInstance.InstanceId = instanceId;
            }

            return result;
        }
    }
}