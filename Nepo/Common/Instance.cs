using System.Collections.Generic;

namespace Nepo.Common
{
    public class Instance
    {
        public MapConfig Map { get; set; } = new MapConfig();
        public List<AgentConfig> AgentConfigs { get; set; }
    }
}