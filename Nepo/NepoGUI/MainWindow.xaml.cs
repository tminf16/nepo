using Nepo.Common;
using Nepo.Common.Rules;
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

namespace Nepo.GUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Ellipse> Immovables { get; set; }
        public List<Control> Movables { get; set; }
        public MapConfig Map { get { return DataHandler.GetMapConfig(); } }
        public int MapWidth { get { return Map.MapSize.Width; } set { } }
        public int MapHeight { get { return Map.MapSize.Height; } set { } }
        private int ImmoSize = 5;
        private int MovableSize = 500;

        private Solution currentSolution;
        public RelayCommand ManualOptimizeCommand { get; set; }
        public RelayCommand ResetSolutionCommand { get; set; }
        public RelayCommand AutomateSolutionCommand { get; set; }


        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.Register("TargetValue", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

        public double MaximumTargetValue { get; set; }

        DistanceIntervalsRule tmpRule = new DistanceIntervalsRule();
        public MainWindow()
        {
            Movables = new List<Control>();
            ManualOptimizeCommand = new RelayCommand(GetNewSolution);
            ResetSolutionCommand = new RelayCommand(ResetSolution);
            AutomateSolutionCommand = new RelayCommand(AutomateSolution);
            tmpRule.AddInterval(0, 15, -1);
            tmpRule.AddInterval(15,100,0.5);
            CreateSampleMap();
            Immovables = new List<Ellipse>();
            double minWeight = Map.ImmovableObjects.Min(x => x.Weight);
            double maxWeight = Map.ImmovableObjects.Max(x => x.Weight);
            MaximumTargetValue = 0;
            foreach (var immo in Map.ImmovableObjects)
            {
                MaximumTargetValue += 0.5 * immo.Weight;
                int additionalSize = (int)((immo.Weight - minWeight) / (maxWeight - minWeight) * 10);
                var tmpItem = new Ellipse()
                {
                    Width = ImmoSize + additionalSize,
                    Height = ImmoSize + additionalSize,
                    Fill = Brushes.Red,
                };
                Canvas.SetTop(tmpItem, immo.Location.Y - (ImmoSize / 2));
                Canvas.SetLeft(tmpItem, immo.Location.X - (ImmoSize / 2));
                Immovables.Add(tmpItem);
            }

            DrawSolution();
            currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            
            TargetValue = tmpRule.CalculatePartialTargetValue(currentSolution);
            InitializeComponent();
        }
                
        private void ResetSolution(object obj)
        {
            Optimizer.Instance.Reset();
            currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            GetNewSolution(null);
        }
        private void AutomateSolution(object obj)
        {
            new TaskFactory().StartNew(async () =>
            {
                int i = 0;
                int targetvalueCounter = 0;
                double lastTargetValue = 0;
                double tmpTargetValue = 0;
                while (i < 10000)
                {
                    i++;
                    Dispatcher.Invoke(() => tmpTargetValue = TargetValue);
                    if (i % 1 == 0)
                    {
                        await Task.Delay(100);
                    }
                    GetNewSolution(null);
                    if (tmpTargetValue == lastTargetValue)
                        targetvalueCounter++;
                    else
                    {
                        lastTargetValue = tmpTargetValue;
                        targetvalueCounter = 0;
                    }

                    if (targetvalueCounter > 100)
                        break;
                }
            });
        }

        private void GetNewSolution(object obj)
        {
            int bestId = 0;
            double bestValue = 0;
            for (int i = 0; i < 10; i++)
            {
                i++;
                var result = Optimizer.Instance.SelectChild(0);
                double currentTargetValue = tmpRule.CalculatePartialTargetValue(currentSolution);
                bestValue = currentTargetValue;
                bestId = currentSolution.SolutionID;
                foreach (var sol in result.Item2)
                {
                    double tmptarget = tmpRule.CalculatePartialTargetValue(sol);
                    if(tmptarget >= bestValue)
                    {
                        bestValue = tmptarget;
                        bestId = sol.SolutionID;
                        currentSolution = sol;
                    }
                }
                Optimizer.Instance.SelectChild(bestId);
            }
            Dispatcher.Invoke(() => TargetValue = bestValue);
            

            DrawSolution();
        }

        private void DrawSolution()
        {
            if (null == currentSolution)
                currentSolution = Optimizer.Instance.SelectChild(0).Item1;

            if (0 == Movables.Count())
            {
                ControlTemplate ct = tmpRule.GetUiTemplate();
                foreach (var po in currentSolution.PlanningObjects)
                {
                    var tmpItem = new Control() { Width = MovableSize, Height = MovableSize};
                    tmpItem.Template = ct;
                    Movables.Add(tmpItem);
                }
            }

            for (int i = 0; i < currentSolution.PlanningObjects.Length; i++)
            {
                var tmpCtrl = Movables.ElementAt(i);
                var po = currentSolution.PlanningObjects[i];
                Dispatcher.Invoke(() => 
                {
                    Canvas.SetTop(tmpCtrl, po.Location.Y - (MovableSize / 2));
                    Canvas.SetLeft(tmpCtrl, po.Location.X - (MovableSize / 2));
                });
            }
        }

        private static void CreateSampleMap()
        {
            var config = new MapConfig()
            {
                MapSize = new System.Drawing.Size(500, 500),
                PlanningObjectCount = 5
            };
            Random rand = new Random((int)DateTime.Now.Ticks);
            config.ImmovableObjects = new List<ImmovableObject>();
            for (int i = 0; i < 20; i++)
            {
                config.ImmovableObjects.Add(new ImmovableObject()
                {
                    Location = new System.Drawing.Point(rand.Next(500), rand.Next(500)),
                    Weight = rand.Next(100, 150) 
                });
            }
            DataHandler.SaveMapConfig(config);
        }
    }
}
