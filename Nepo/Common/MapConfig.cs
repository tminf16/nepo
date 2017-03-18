using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace Nepo.Common
{
    [Serializable]
    [DataContract]
    public class MapConfig
    {
        [DataMember]
        public Size MapSize { get; set; }
        [DataMember]
        public int PlanningObjectCount { get; set; }
        [DataMember]
        public List<ImmovableObject> ImmovableObjects { get; set; }
        [DataMember]
        public List<Layer> Layers { get; set; }
        [DataMember]
        public int ForcedAcceptance { get; set; }
        [DataMember]
        public int MaxRounds { get; set; }

        public MapConfig()
        {
            ImmovableObjects = new List<ImmovableObject>();
            Layers = new List<Layer>();
            //ForcedAcceptance = 1;
            //MaxRounds = 10;
        }
    }
}
