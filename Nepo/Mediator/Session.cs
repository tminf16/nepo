using Nepo.Common;
using Nepo.DataGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator
{
    public class Session
    {
        private static Session _instance = null;
        public static Session Get { get { return _instance ?? (_instance = new Session()); } }
        public Instance CurrentInstance { get; set; }
        public ObservableCollection<Instance> AvailableInstances { get; set; }
        public Session()
        {
            AvailableInstances = new ObservableCollection<Instance>();
            foreach (var inst in Instance.LoadInstances())
                AvailableInstances.Add(inst);
        }

        public void Addinstance(bool test)
        {
            var conf = new GenerationConfig();
            if(test)
                conf.AgentsCount = 2;
            var inst = Generator.GenerateInstance(conf);
            inst.Save();
            AvailableInstances.Add(inst);
        }
    }
}
