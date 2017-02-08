using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Nepo.Common
{
    [DataContract]
    [Serializable]
    public class Layer
    {        
        private Bitmap _map;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Map")]
        public byte[] MapSerialized
        {
            get
            { // serialize
                if (this.Map == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    this.Map.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    this.Map = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        this.Map = new Bitmap(ms);
                    }
                }
            }
        }

        [DataMember]
        [XmlIgnore]
        public Bitmap Map
        {
            get;
            set;
        }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public double Weight { get; set; } = 1;
    }
}