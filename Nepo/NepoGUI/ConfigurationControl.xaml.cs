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
using Nepo.Common;
using Nepo.Common.Rules;
using System.Collections.ObjectModel;

namespace NepoGUI
{
    /// <summary>
    /// Interaktionslogik für ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {

        public int MyProperty { get; set; }

        public int CurveMinRange
        {
            get { return (int)GetValue(CurveMinRangeProperty); }
            set { SetValue(CurveMinRangeProperty, value); }
        }

        public AgentConfig Config { get; private set; }

        internal void LoadValues()
        {
            Config = Session.Get.Config;
            var curveRule = Config.Rules.Where(x => x.GetType() == typeof(CurveRule)).FirstOrDefault();
            if (curveRule != null)
            {
                TB_MinRange.Text = (curveRule as CurveRule).Min.ToString();
                TB_MaxRange.Text = (curveRule as CurveRule).Max.ToString();
            }
            var distanceIntervalsRules = (DistanceIntervalsRule)Config.Rules.Where(x => x.GetType() == typeof(DistanceIntervalsRule)).FirstOrDefault();
            if (distanceIntervalsRules != null)
            {
                foreach (var interval in distanceIntervalsRules.Intervals)
                {
                    //Add Interval
                    Intervals.Add(new Interval { Min = interval.Min, Max = interval.Max, Value = interval.Value });
                }
            }
            //Load Map
            ConfigurationMapControl.Configure(Session.Get.Map, Session.Get.Config);
        }

        public ObservableCollection<Interval> Intervals { get; set; }

        public RelayCommand DeleteRowCommand { get; set; }

        public static readonly DependencyProperty CurveMinRangeProperty =
           DependencyProperty.Register("CurveMinRange", typeof(int), typeof(ConfigurationControl), new PropertyMetadata(0));

        public ConfigurationControl()
        {
            DeleteRowCommand = new RelayCommand(DeleteRow);

            Intervals = new ObservableCollection<Interval>();
            InitializeComponent();
            
        }

        private void DeleteRow(object obj)
        {
            Intervals.Remove((Interval)obj);
        }

        private void TB_MinRange_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TB_MinRange_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char charItem in e.Text)
            {
                if (!Char.IsNumber(charItem))
                {
                    e.Handled = true;
                }
            }
        }
    }
}