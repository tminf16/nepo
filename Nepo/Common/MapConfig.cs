using System;
using System.Collections.Generic;
using System.Drawing;

namespace Nepo.Common
{
    [Serializable]
    public class MapConfig
    {
        public Size MapSize { get; set; }
        public int PlanningObjectCount { get; set; }
        public List<ImmovableObject> ImmovableObjcts { get; set; }
        public List<Layer> Layers { get; set; }
    }
}
