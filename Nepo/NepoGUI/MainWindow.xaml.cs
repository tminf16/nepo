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

namespace Nepo.GUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Ellipse> Immovables { get; set; }
        public List<Ellipse> Movables { get; set; }
        public MapConfig Map { get { return DataHandler.GetMapConfig(); } }
        public int MapWidth { get { return Map.MapSize.Width; } set { } }
        public int MapHeight { get { return Map.MapSize.Height; } set { } }
        private int ImmoSize = 5;
        private int MovableSize = 5;

        private Solution currentSolution;

        public MainWindow()
        {
            CreateSampleMap();
            Immovables = new List<Ellipse>();
            foreach (var immo in Map.ImmovableObjects)
            {
                var tmpItem = new Ellipse()
                {
                    Width = ImmoSize,
                    Height = ImmoSize,
                    Fill = Brushes.Red,
                };
                Canvas.SetTop(tmpItem, immo.Location.X - (ImmoSize / 2));
                Canvas.SetLeft(tmpItem, immo.Location.Y - (ImmoSize / 2));
                Immovables.Add(tmpItem);
            }

            Movables = new List<Ellipse>();
            currentSolution = Optimizer.Instance.GenerateSolutions().Item1;
            foreach (var po in currentSolution.PlanningObjects)
            {
                var tmpItem = new Ellipse()
                {
                    Width = MovableSize,
                    Height = MovableSize,
                    Fill = Brushes.Black,
                };
                Canvas.SetTop(tmpItem, po.Location.X - (MovableSize / 2));
                Canvas.SetLeft(tmpItem, po.Location.Y - (MovableSize / 2));
                Movables.Add(tmpItem);
            }

            InitializeComponent();
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
