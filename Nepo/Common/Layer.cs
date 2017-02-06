using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Nepo.Common
{
    [Serializable]
    public class Layer
    {
        /// <summary>
        /// Polygons always have 100% opacity.
        /// </summary>
        public List<List<Point>> Polygons { get; set; }

        private Bitmap _map;

        [XmlIgnore]
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
                    _map = (Bitmap) Image.FromFile(this.MapPath);
                    return _map;
                }
            }
        }

        public string MapPath { get; set; }
        public double Weight { get; set; } = 1;
    }
}