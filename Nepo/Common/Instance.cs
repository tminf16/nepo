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
        public Guid InstanceId { get; set; }
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
                if (!Guid.TryParse(folder.Name, out var instanceId))
                    continue;
                var tmpInstance = new Instance()
                {
                    InstanceId = instanceId,
                    Map = DataHandler.GetMapConfig(instanceId.ToString()),
                    AgentConfigs = DataHandler.GetAgentConfigs(instanceId.ToString())
                };
                result.Add(tmpInstance);
            }

            return result;
        }

        public void Save()
        {
            DirectoryInfo instanceDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + InstanceId.ToString());
            if (!instanceDir.Exists)
                instanceDir.Create();
            DataHandler.SaveAgentConfigs(AgentConfigs, InstanceId.ToString());
            DataHandler.SaveMapConfig(Map, InstanceId.ToString());
        }
    }
}