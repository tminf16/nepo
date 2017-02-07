using Nepo.Common;
using System;
using System.Collections.Generic;
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
    public partial class InstanceOverview : UserControl
    {
        public List<Instance> AvailableInstances { get { return Session.Get.Instances; } set { } }
        public RelayCommand SelectInstanceCommand { get; set; }
        public InstanceOverview()
        {
            SelectInstanceCommand = new RelayCommand(SelectInstance);
            InitializeComponent();
        }

        private void SelectInstance(object obj)
        {
            GuiNavigation.Get.SelectInstance((Guid)obj);

        }
    }
}
