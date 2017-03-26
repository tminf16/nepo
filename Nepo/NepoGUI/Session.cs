using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NepoGUI
{
    public class Session
    {
        private static Session _get = null;
        public static Session Get { get { return _get ?? (_get = new Session()); } }

        public AgentConfig Config { get; set; }
        public AgentConfig ExtendedConfig { get {
                if (null == CurrentInstance)
                    return null;
                if (null == CurrentInstance.AgentConfigs)
                    return null;
                if (2 > CurrentInstance.AgentConfigs.Count)
                    return null;
                return CurrentInstance.AgentConfigs[1];
            } }
        public MapConfig Map { get; set; }
        public List<Instance> Instances { get; set; }
        private Instance _currentInstance;
        private Guid _serverInstance;
        public Guid ServerInstance { get { return _serverInstance; } }

        private NepoClient _client = new NepoClient();
        private Solution _currentSolution;
        public Solution CurrentSolution { get { return _currentSolution; } }
        private List<Solution> _availableChildSolutions;
        public List<Solution> AvailableChildSolutions { get { return _availableChildSolutions; }  }

        public event EventHandler NewDataAvailable;

        public Guid getNepoClientGUID()
        {
            NepoClient _c = _client;
            return _c.GetGUID();
        }

        /// <summary>
        /// Finish the local optimization for logging purpose
        /// </summary>
        public void finishLocal()
        {
            Client_HabemusPapam(null,null);
        }

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
                Optimizer.Instance.SetMap(Map);
                if (null == value.AgentConfigs || 0 == value.AgentConfigs.Count)
                    Config = DataHandler.GetAgentConfig();
                else
                    Config = value.AgentConfigs.FirstOrDefault();
                if (value.InstanceId == ServerInstance)
                {
                    _currentSolution = _client.GetCurrentSolution();
                    _availableChildSolutions = _client.GetProposedSolutions();
                }
                else
                {
                    _currentSolution = Optimizer.Instance.SelectChild(0).Item1;
                    _availableChildSolutions = Optimizer.Instance.SelectChild(0).Item2;
                }
                CommandManager.InvalidateRequerySuggested();
            } }

        internal void Reset()
        {
            Optimizer.Instance.Reset();
            _currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            _availableChildSolutions = Optimizer.Instance.SelectChild(0).Item2;
        }

        public Session()
        {
            Config = DataHandler.GetAgentConfig();
            Map = DataHandler.GetMapConfig();
            Instances = Instance.LoadInstances();
            _client.NewDataAvailable += NewDataFromMediator;
            _client.HabemusPapam += Client_HabemusPapam;
        }

        private void Client_HabemusPapam(object sender, EventArgs e)
        {
            //_currentSolution = _client.GetCurrentSolution();
            Console.WriteLine("Habemus Papam");
            Logger.Get.AnzPlanningObjects = Map.PlanningObjectCount;
            Logger.Get.Maxrounds = Optimizer.Instance.map.MaxRounds;

            Logger.Get.AddMyTargetValue(_client.GetGUID(), Optimizer.CalculateTargetValue(_currentSolution, Config, Map));
            //Console.WriteLine(Config);
            Logger.Get.agentConfigID = Config.Guid;
            // Hier Config für LogDump            
            Logger.Get.finish();
            
        }


        internal void Vote(List<int> list)
        {
            _client.Vote(list.Select(x => new Tuple<int, bool>(x, true)).ToList());
        }

        private void NewDataFromMediator(object sender, NewDataEventArgs e)
        {
            _availableChildSolutions = e.ProposedSolutions;
            _currentSolution = _client.GetCurrentSolution();
            NewDataAvailable?.Invoke(false, null);
        }

        public void NewLocalData()
        {
            _availableChildSolutions = Optimizer.Instance.SelectChild(0).Item2;
            _currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            NewDataAvailable?.Invoke(true, null);

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
            _serverInstance = result.InstanceId;
            Instances = Instance.LoadInstances();
        }
    }
}
