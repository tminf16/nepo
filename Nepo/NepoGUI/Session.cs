﻿using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NepoGUI
{
    public class Session
    {
        private static Session _get = null;
        public static Session Get { get { return _get ?? (_get = new Session()); } }
        
        public AgentConfig Config { get; set; }
        public MapConfig Map { get; set; }
        public List<Instance> Instances { get; set; }
        private Instance _currentInstance;

        private NepoClient _client = new NepoClient();
        private Solution _currentSolution;
        public Solution CurrentSolution { get { return _currentSolution; } }
        private List<Solution> _availableChildSolutions;
        public List<Solution> AvailableChildSolutions { get { return _availableChildSolutions; }  }

        public event EventHandler NewDataAvailable;

        public Instance CurrentInstance { get { return _currentInstance; } set
            {
                _currentInstance = value;
                if (null == value)
                {
                    Config = DataHandler.GetAgentConfig();
                    Map = DataHandler.GetMapConfig();
                    return;
                }
                Map = value.Map;
                if (null == value.AgentConfigs || 0 == value.AgentConfigs.Count)
                    Config = DataHandler.GetAgentConfig();
                else
                    Config = value.AgentConfigs.FirstOrDefault();
                _currentSolution = _client.GetCurrentSolution();
                _availableChildSolutions = _client.GetProposedSolutions();
            } }

        public Session()
        {
            Config = DataHandler.GetAgentConfig();
            Map = DataHandler.GetMapConfig();
            Instances = Instance.LoadInstances();
            _client.NewDataAvailable += NewDataFromMediator;
            _client.HabemusPapam += _client_HabemusPapam;
        }

        private void _client_HabemusPapam(object sender, EventArgs e)
        {
            
            _currentSolution = _client.GetCurrentSolution();
        }

        internal void Vote(List<int> list)
        {
            _client.Vote(list.Select(x => new Tuple<int, bool>(x, true)).ToList());
        }

        private void NewDataFromMediator(object sender, NewDataEventArgs e)
        {
            _availableChildSolutions = e.ProposedSolutions;
            _currentSolution = _client.GetCurrentSolution();
            NewDataAvailable?.Invoke(null, null);
        }

        public void Save()
        {
            DataHandler.SaveAgentConfig(Config);
            DataHandler.SaveMapConfig(Map);
        }

        public void CheckMediator()
        {
            var result = _client.Register();
            result.Save();
            Instances = Instance.LoadInstances();
        }
    }
}
