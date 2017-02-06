using Nepo.Common.Rules;
using System.Collections.Generic;

namespace Nepo.Common
{
    public class AgentConfig
    {
        public List<Layer> Layers { get; set; }
        public List<ITargetFunctionComponent> Rules { get; set; }
        public AgentConfig()
        {
            Layers = new List<Layer>();
            Rules = new List<ITargetFunctionComponent>();
        }
    }
}