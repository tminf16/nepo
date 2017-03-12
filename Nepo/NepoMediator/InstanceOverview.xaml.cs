using Mediator;
using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NepoMediator
{
    /// <summary>
    /// Interaction logic for InstanceOverview.xaml
    /// </summary>
    public partial class InstanceOverview : UserControl
    {
        public ObservableCollection<Instance> AvailableInstances { get { return Session.Get.AvailableInstances; } set { } }
        public RelayCommand SelectInstanceCommand { get; set; }
        public RelayCommand DeleteInstanceCommand { get; set; }
        public RelayCommand AddInstanceCommand { get; set; }
        private MainWindow mainWindow;
        public InstanceOverview(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            SelectInstanceCommand = new RelayCommand(SelectInstance);
            DeleteInstanceCommand = new RelayCommand(DeleteInstance);
            AddInstanceCommand = new RelayCommand(AddInstance);
            InitializeComponent();
        }

        private void AddInstance(object obj)
        {
            if (!Boolean.TryParse(obj as String, out bool test))
                return;
            Session.Get.Addinstance(test);
        }

        private void DeleteInstance(object obj)
        {
            var inst = obj as Instance;
            AvailableInstances.Remove(inst);
            inst.Delete();
        }

        private void SelectInstance(object obj)
        {
            var inst = obj as Instance;
            mainWindow.MainContent.Content = new SessionView(inst);
            
        }
    }
}
