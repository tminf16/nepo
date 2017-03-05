using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class MediatorConfig
    {
        private static MediatorConfig _instance = null;
        public static MediatorConfig Get { get { return _instance ?? (_instance = DataHandler.GetConfig<MediatorConfig>()); } }
        public void Save()
        {
            this.Save<MediatorConfig>();
        }

        public MediatorConfig()
        {
            TestMode = false;
        }

        public bool TestMode { get; set; }
    }
}
