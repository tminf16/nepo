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
using Microsoft.Win32;
using System.Windows.Forms;

namespace NepoGUI
{
    /// <summary>
    /// Interaktionslogik für ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : System.Windows.Controls.UserControl
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
            //get rules
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
            //Get Layers
            if ( Config.Layers != null)
            {
                foreach ( var layer in Config.Layers)
                {
                    //Add Layer
                    Layers.Add(new Layer { FileName = layer.FileName, MapSerialized = layer.MapSerialized, Weight = layer.Weight, Map = layer.Map });

                    //Show Layer on Map
                }
            }
            //Activate the default button group
            if (TB_MaxRange.Text != "" && TB_MinRange.Text != "") RB_CurveRule.IsChecked = true;
            else RB_IntervallRule.IsChecked = true;

            //Load Map
            ConfigurationMapControl.Configure(Session.Get.Map, Session.Get.Config);
        }

        public ObservableCollection<Interval> Intervals { get; set; }

        public ObservableCollection<Layer> Layers { get; set; }

        //Buttons
        public RelayCommand DeleteRowCommand { get; set; }
        public RelayCommand AddRowCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddLayerCommand { get; set; }
        public RelayCommand DeleteLayerCommand { get; set; }

        public static readonly DependencyProperty CurveMinRangeProperty =
           DependencyProperty.Register("CurveMinRange", typeof(int), typeof(ConfigurationControl), new PropertyMetadata(0));

        public ConfigurationControl()
        {
            AddRowCommand = new RelayCommand(AddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow);
            SaveCommand      = new RelayCommand(SaveConfig);
            AddLayerCommand = new RelayCommand(AddLayer);
            DeleteLayerCommand = new RelayCommand(DeleteLayer);

            Intervals = new ObservableCollection<Interval>();
            Layers = new ObservableCollection<Layer>();
            InitializeComponent();
            
        }

        private void DeleteLayer(object obj)
        {
            Layers.Remove((Layer)obj);
        }

        private void AddLayer(object obj)
        {
            //File Upload
            System.Windows.Forms.OpenFileDialog dataBrowser = new System.Windows.Forms.OpenFileDialog();
            dataBrowser.Filter = "Bitmap Files (*.bmp)|*.bmp|Pictures (*png)|*.png|Whatever you think (*.*)|*.*";
            if (dataBrowser.ShowDialog() == DialogResult.OK)
            {
                //Upload and display Layer
                Layers.Add(new Layer
                {
                    FileName = dataBrowser.FileName,
                    Weight = 1,
                });

                //Save and reload
                SaveConfig(obj);
            }
            
        }

        private void AddRow(object obj)
        {
            //Add a new empty Interval to the list
            Intervals.Add(new Interval { Min = 0, Max = 0, Value = 0 });
        }

        private void DeleteRow(object obj)
        {
            Intervals.Remove((Interval)obj);
        }

        private void TB_MinRange_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckNumericText(object sender, TextCompositionEventArgs e)
        {
            foreach (char charItem in e.Text)
            {
                if (Char.IsWhiteSpace(charItem) || (!Char.IsNumber(charItem) && !charItem.Equals('.') ) ) 
                {
                    e.Handled = true;
                }
            }
        }

        private void CheckNoSpace(object sender, TextCompositionEventArgs e)
        {
            foreach(char charItem in e.Text)
            {
                e.Handled = Char.IsWhiteSpace(charItem) ? true : false;
            }
        }

        private void SaveConfig(object obj)
        {
            //Delete all rules in Config and set new
            Config.Rules.Clear();
            if (RB_CurveRule.IsChecked == true) //Curve Rule
            {
                Config.Rules.Add(new CurveRule( Int32.Parse(TB_MinRange.Text), Int32.Parse(TB_MaxRange.Text)));
            }
            else if(Intervals != null)      //Interval Rule
            {
                var intervalsRule = new DistanceIntervalsRule();
                foreach (var interval in Intervals)
                {
                    intervalsRule.AddInterval( interval.Min, interval.Max, interval.Value);
                }
                Config.Rules.Add(intervalsRule);
            }
            //Remove existing layers
            Config.Layers.Clear();

            //Add new Layers
            if(Layers != null)
            {
                foreach(var layer in Layers)
                {
                    Config.Layers.Add(layer);
                }
            }

            //Save
            Config.Save();
            //Reload Map
            ConfigurationMapControl.Configure(Session.Get.Map, Session.Get.Config);
        }

        private void CheckNoSpace(object sender, System.Windows.Input.KeyEventArgs e)
        {
                e.Handled = e.Key == Key.Space ? true : false;
        }
    }
}