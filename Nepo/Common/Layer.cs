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
        private MemoryStream _ms = null;
        public MemoryStream PngMemoryStream { get {
                byte[] tmp;
                if (null == _ms)
                    tmp = MapSerialized;
                return _ms; } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Map")]
        public byte[] MapSerialized
        {
            get
            { // serialize
                if (this.Map == null) return null;

                if (null == _ms)
                {
                    _ms = new MemoryStream();
                    this.Map.Save(_ms, ImageFormat.Png);
                }
                return _ms.ToArray();
            }
            set
            { // deserialize
                if (value == null)
                {
                    this.Map = null;
                }
                else
                {
                    if(null == _ms)
                        _ms = new MemoryStream(value);

                    this.Map = new Bitmap(_ms);
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