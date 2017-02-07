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
    /// Interaktionslogik für OptimizeControl.xaml
    /// </summary>
    public partial class OptimizeControl : UserControl
    {
        public OptimizeControl()
        {
            InitializeComponent();
        }

        public void LoadValues()
        {
            Instance currentInstance = Session.Get.CurrentInstance;
            if (null == currentInstance)
                return;
            OptimizeMapControl.Configure(Session.Get.Map, Session.Get.Config);
        }
    }
}
