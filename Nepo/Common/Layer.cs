using System;
using System.Collections.Generic;
using System.Drawing;
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

        [XmlIgnore]
        [DataMember]
        public Bitmap Map
        {
            get
            {
                if (_map != null)
                {
                    return _map;
                }
                else
                {
                    _map = (Bitmap) Image.FromFile(System.IO.Path.Combine(Directory.GetCurrentDirectory(), this.FileName));
                    return _map;
                }
            }

            set
            {
                this._map = value;
                this._map.Save(System.IO.Path.Combine(Directory.GetCurrentDirectory(), this.FileName));
            }
        }
        
        public string FileName { get; set; }
        [DataMember]
        public double Weight { get; set; } = 1;
    }
}