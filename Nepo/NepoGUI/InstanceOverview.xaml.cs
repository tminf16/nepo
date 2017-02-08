using Nepo.Common;
using System;
using System.Collections.Generic;
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
        public List<Instance> AvailableInstances { get { return Session.Get.Instances; } set { } }
        public RelayCommand SelectInstanceCommand { get; set; }
        public RelayCommand CheckInstanceCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public InstanceOverview()
        {
            SelectInstanceCommand = new RelayCommand(SelectInstance);
            CheckInstanceCommand = new RelayCommand(CheckInstance);
            InitializeComponent();
            Task.Run(()=>CheckInstance(null));
        }

        private void SelectInstance(object obj)
        {
            GuiNavigation.Get.SelectInstance((Guid)obj);
        }

        private void CheckInstance(object obj)
        {
            Session.Get.CheckMediator();
            AvailableInstances = Instance.LoadInstances();
            var onlineinstance = AvailableInstances.SingleOrDefault(x => x.InstanceId == Session.Get.ServerInstance);
            if (null != onlineinstance)
            {
                onlineinstance.Online = true;
                Logger.testinstanzguid = onlineinstance.InstanceId;
                //Logger.print();
            }
            Dispatcher.Invoke(() =>OnPropertyChanged("AvailableInstances"));
        }
    }
}
