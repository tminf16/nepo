using System.Drawing;
using System.Runtime.Serialization;

namespace Nepo.Common
{
    [DataContract]
    public class ImmovableObject
    {
        [DataMember]
        public Point Location { get; set; }
        [DataMember]
        public decimal Weight { get; set; }
    }
}