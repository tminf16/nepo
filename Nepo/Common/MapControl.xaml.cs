﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Nepo.Common
{
    /// <summary>
    /// Interaktionslogik für MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private Solution _currentSolution = null;
        private AgentConfig _config = null;
        private MapConfig _currentMap = null;
        public ObservableCollection<Ellipse> AllImmovables { get; set; }
        public ObservableCollection<BitmapSource> MapLayerBitmaps { get; set; }
        public ObservableCollection<BitmapSource> AgentLayerBitmaps { get; set; }
        public ObservableCollection<Grid> Movables { get; set; }
        
        private int ImmoSize = 5;

        public MapControl()
        {
            AllImmovables = new ObservableCollection<Ellipse>();
            MapLayerBitmaps = new ObservableCollection<BitmapSource>();
            AgentLayerBitmaps = new ObservableCollection<BitmapSource>();
            Movables = new ObservableCollection<Grid>();
            InitializeComponent();
        }
        
        public void Configure(MapConfig mapconfig, AgentConfig config)
        {
            _currentMap = mapconfig;
            _config = config;
            this.Width = _currentMap.MapSize.Width;
            this.Height = _currentMap.MapSize.Height;


            foreach (var maplayer in mapconfig.Layers)
            {
                var ms = maplayer.PngMemoryStream;
                MapLayerBitmaps.Add(ToBitmapImage(ms));
            }
            if (null != config)
                foreach (var agentlayer in config.Layers)
                {
                    var ms = agentlayer.PngMemoryStream;
                    AgentLayerBitmaps.Add(ToBitmapImage(ms));
                }

            AllImmovables.Clear();

            decimal minWeight = 0;
            decimal maxWeight = 1;
            if (0 != _currentMap.ImmovableObjects.Count)
            {
                minWeight = _currentMap.ImmovableObjects.Min(x => x.Weight);
                maxWeight = _currentMap.ImmovableObjects.Max(x => x.Weight);
            }

            foreach (var immo in _currentMap.ImmovableObjects)
            {
                int additionalSize = (int)((immo.Weight - minWeight) / (maxWeight - minWeight) * 10);
                var tmpItem = new Ellipse()
                {
                    Width = ImmoSize + additionalSize,
                    Height = ImmoSize + additionalSize,
                    Fill = System.Windows.Media.Brushes.Red,
                };
                Canvas.SetTop(tmpItem, immo.Location.Y - (tmpItem.Height / 2));
                Canvas.SetLeft(tmpItem, immo.Location.X - (tmpItem.Width / 2));
                AllImmovables.Add(tmpItem);
            }

            Movables.Clear();
            List<ControlTemplate> templates = new List<ControlTemplate>();
            if(null != config)
            {
                foreach (var rule in config.Rules)
                {
                    templates.Add(rule.GetUiTemplate());
                }
            }
            else
            {
                templates.Add(GetDefaultTemplate());
            }
            for (int i = 0; i < mapconfig.PlanningObjectCount; i++)
            {
                var tmpGrid = new Grid()
                {
                    Width = mapconfig.MapSize.Width,
                    Height = mapconfig.MapSize.Height,
                    Background = System.Windows.Media.Brushes.Transparent
                };
                foreach (var template in templates)
                {
                    var tmpItem = new Control() { Width = tmpGrid.Width, Height = tmpGrid.Height};
                    tmpItem.Template = template;
                    tmpGrid.Children.Add(tmpItem);
                }
                Movables.Add(tmpGrid);
            }
        }

        public void SetSolution(Solution solution)
        {
            _currentSolution = solution;
            Render();
        }

        public void Render()
        {
            
            for (int i = 0; i < _currentSolution?.PlanningObjects.Length; i++)
            {
                var tmpCtrl = Movables.ElementAt(i);
                var po = _currentSolution.PlanningObjects[i];
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetTop(tmpCtrl, po.Location.Y - (tmpCtrl.Height / 2));
                    Canvas.SetLeft(tmpCtrl, po.Location.X - (tmpCtrl.Width / 2));
                });
            }
        }

        private ControlTemplate GetDefaultTemplate()
        {
            var uiElement = new ControlTemplate();
            var grid = new FrameworkElementFactory(typeof(Grid));
            uiElement.VisualTree = grid;
            var rect1 = new FrameworkElementFactory(typeof(Rectangle));
            rect1.SetValue(Rectangle.HeightProperty, 1.0);
            rect1.SetValue(Rectangle.WidthProperty, 20.0);
            rect1.SetValue(Rectangle.FillProperty, System.Windows.Media.Brushes.Black);

            var rect2 = new FrameworkElementFactory(typeof(Rectangle));
            rect2.SetValue(Rectangle.WidthProperty, 1.0);
            rect2.SetValue(Rectangle.HeightProperty, 20.0);
            rect2.SetValue(Rectangle.FillProperty, System.Windows.Media.Brushes.Black);

            grid.SetValue(Grid.WidthProperty, 20.0 );
            grid.SetValue(Grid.HeightProperty, 20.0);
            grid.AppendChild(rect1);
            grid.AppendChild(rect2);

            return uiElement;
        }

        private BitmapImage ToBitmapImage(MemoryStream stream)
        {
            
            stream.Position = 0;
            BitmapImage result = new BitmapImage();
            result.BeginInit();
            // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
            // Force the bitmap to load right now so we can dispose the stream.
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = stream;
            result.EndInit();
            result.Freeze();
            return result;
        }
    }
}