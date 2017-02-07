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
                Guid instanceId;
                if (!Guid.TryParse(folder.Name, out instanceId))
                    continue;
                var tmpInstance = new Instance();
                tmpInstance.InstanceId = instanceId;
                tmpInstance.Map = DataHandler.GetMapConfig(instanceId.ToString());
                tmpInstance.AgentConfigs = DataHandler.GetAgentConfigs(instanceId.ToString());

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