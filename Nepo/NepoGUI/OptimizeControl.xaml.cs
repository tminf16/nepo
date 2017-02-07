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
        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static readonly DependencyProperty ProgressProperty =
           DependencyProperty.Register("Progress", typeof(int), typeof(OptimizeControl), new PropertyMetadata(0));



        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.Register("TargetValue", typeof(double), typeof(OptimizeControl), new PropertyMetadata(0.0));



        public RelayCommand StartVotingCommand { get; set; }

        public OptimizeControl()
        {
            StartVotingCommand = new RelayCommand(StartVoting);
            InitializeComponent();
            Session.Get.NewDataAvailable += Get_NewDataAvailable;

        }

        private void StartVoting(object obj)
        {
            Vote();   
        }

        private void Vote()
        {
            Session.Get.Vote(
                Optimizer.FindBestSolutions(
                    Session.Get.AvailableChildSolutions,
                    Session.Get.Config, 1));
        }

        private void Get_NewDataAvailable(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TargetValue = Optimizer.CalculateTargetValue(Session.Get.CurrentSolution, Session.Get.Config);
                Progress = Session.Get.CurrentSolution.Progress;
            });
            Draw();
            Vote();
        }

        public void Draw()
        {
            OptimizeMapControl.SetSolution(Session.Get.CurrentSolution);
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
