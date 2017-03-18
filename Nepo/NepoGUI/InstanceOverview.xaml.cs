using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NepoGUI
{
    /// <summary>
    /// Interaktionslogik für InstanceOverview.xaml
    /// </summary>
    public partial class InstanceOverview : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<Instance> _instances;
        public ObservableCollection<Instance> AvailableInstances { get
            {
                if (null != _instances)
                    return _instances;
                _instances = new ObservableCollection<Instance>();
                foreach (var item in Session.Get.Instances)
                    _instances.Add(item);
                return _instances;
                }
            set
            {
            } }
        public RelayCommand SelectInstanceCommand { get; set; }
        public RelayCommand DeleteInstanceCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public InstanceOverview()
        {
            SelectInstanceCommand = new RelayCommand(SelectInstance);
            DeleteInstanceCommand = new RelayCommand(DeleteInstance);
            InitializeComponent();
            Task.Run(()=>CheckInstance(null));
        }

        private void DeleteInstance(object obj)
        {
            var inst = obj as Instance;
            AvailableInstances.Remove(inst);
            inst.Delete();
        }

        private void SelectInstance(object obj)
        {
            GuiNavigation.Get.SelectInstance((Instance)obj);
        }

        private void CheckInstance(object obj)
        {
            Session.Get.CheckMediator();
            Dispatcher.Invoke(()=>
            {
                AvailableInstances.Clear();
                foreach (var item in Instance.LoadInstances())
                    AvailableInstances.Add(item);

            });
            var onlineinstance = AvailableInstances.SingleOrDefault(x => x.InstanceId == Session.Get.ServerInstance);
            if (null != onlineinstance)
            {
                onlineinstance.Online = true;
                Logger.Get.Testinstanzguid = onlineinstance.InstanceId;
                Optimizer.Instance.maxRounds = Session.Get.Map.MaxRounds;
                //Logger.print();
            }
            Dispatcher.Invoke(() => OnPropertyChanged("AvailableInstances"));
        }
    }
}
