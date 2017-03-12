using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Nepo.Common;
using Nepo.Common.Rules;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GuiNavigation navi;
        public MainWindow()
        {
            InitializeComponent();
            navi = new GuiNavigation(MyIncrediblePresenter);
            
            this.SizeToContent = SizeToContent.Height | SizeToContent.Width;
        }
    }
}
