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
        public List<Ellipse> Immovables { get; set; }
        public List<Grid> Movables { get; set; }
        public MapConfig Map { get { return DataHandler.GetMapConfig(); } }
        public int MapWidth { get { return Map.MapSize.Width; }}
        public int MapHeight { get { return Map.MapSize.Height; }}


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


        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(int), typeof(MainWindow), new PropertyMetadata(0));



        public double MaximumTargetValue { get; set; }
   
        public MainWindow()
        {            
            Movables = new List<Grid>();

            ManualOptimizeCommand = new RelayCommand(GetNewSolution);
            ResetSolutionCommand = new RelayCommand(ResetSolution);
            AutomateSolutionCommand = new RelayCommand(AutomateSolution);
            
            currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            MaximumTargetValue = (double)Session.Get.Map.ImmovableObjects.Sum(x => x.Weight);
            TargetValue = Optimizer.CalculateTargetValue(currentSolution, Session.Get.Config);


            InitializeComponent();


            OptimizeMapControl.Configure(Session.Get.Map, Session.Get.Config);
            OptimizeMapControl.SetSolution(currentSolution);

            ConfigMapControl.Configure(Session.Get.Map, Session.Get.Config);
            ConfigMapControl.Render();

            this.SizeToContent = SizeToContent.Height | SizeToContent.Width;
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
                double tmpTargetValue = 0;
                while (currentSolution.Progress < 100)
                {
                    i++;
                    Dispatcher.Invoke(() => tmpTargetValue = TargetValue);
                    if (i % 100 == 0)
                    {
                        await Task.Delay(100);
                    }
                    GetNewSolution(null);
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
                currentSolution = result.Item1;
                double currentTargetValue = Optimizer.CalculateTargetValue(currentSolution, Session.Get.Config);
                bestValue = currentTargetValue;
                bestId = currentSolution.SolutionID;
                foreach (var sol in result.Item2)
                {
                    double tmptarget = Optimizer.CalculateTargetValue(sol, Session.Get.Config);
                    if(tmptarget >= bestValue)
                    {
                        bestValue = tmptarget;
                        bestId = sol.SolutionID;
                        currentSolution = sol;
                    }
                }
                Optimizer.Instance.SelectChild(bestId);
            }
            Dispatcher.Invoke(() =>
            {
                TargetValue = bestValue;
                Progress = currentSolution.Progress;
            });

            DrawSolution();
        }

        private void DrawSolution()
        {
            if (null == currentSolution)
                currentSolution = Optimizer.Instance.SelectChild(0).Item1;
            OptimizeMapControl.SetSolution(currentSolution);
            
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
