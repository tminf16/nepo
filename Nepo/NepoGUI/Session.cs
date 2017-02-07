using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NepoGUI
{
    public class Session
    {
        private static Session _get = null;
        public static Session Get { get { return _get ?? (_get = new Session()); } }
        
        public AgentConfig Config { get; set; }
        public MapConfig Map { get; set; }
        public List<Instance> Instances { get; set; }
        private Instance _currentInstance;

        private NepoClient _client = new NepoClient();

        public Instance CurrentInstance { get { return _currentInstance; } set
            {
                _currentInstance = value;
                if (null == value)
                {
                    Config = DataHandler.GetAgentConfig();
                    Map = DataHandler.GetMapConfig();
                    return;
                }
                Map = value.Map;
                if (null == value.AgentConfigs || 0 == value.AgentConfigs.Count)
                    Config = DataHandler.GetAgentConfig();
                else
                    Config = value.AgentConfigs.FirstOrDefault();

            } }

        public Session()
        {
            Config = DataHandler.GetAgentConfig();
            Map = DataHandler.GetMapConfig();
            Instances = Instance.LoadInstances();
        }

        public void Save()
        {
            DataHandler.SaveAgentConfig(Config);
            DataHandler.SaveMapConfig(Map);
        }

        public void CheckMediator()
        {
            var result = _client.Register();
            result.Save();
            Instances = Instance.LoadInstances();
        }
    }
}
