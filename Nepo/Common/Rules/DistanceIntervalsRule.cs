using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Nepo.Common.Rules
{
    public class DistanceIntervalsRule : ITargetFunctionComponent
    {
        public List<Tuple<int, int, double>> Intervals { get; set; }

        public DistanceIntervalsRule()
        {
            Intervals = new List<Tuple<int, int, double>>();
        }

        public void AddInterval(int min, int max, double value)
        {
            if (max < min)
                return;
            if (value > 1)
                value = 1;
            if (value < -1)
                value = -1;
            Intervals.Add(new Tuple<int, int, double>(min, max, value));
        }

        public double CalculatePartialTargetValue(Solution solution)
        {
            bool[] alreadyCounted = new bool[DataHandler.GetMapConfig().ImmovableObjects.Count];
            for (int i = 0; i < alreadyCounted.Length; i++)
                alreadyCounted[i] = false;
            double partialTargetValue = 0;
            foreach (var po in solution.PlanningObjects)
            {
                int immoCount = 0;
                foreach (var immo in DataHandler.GetMapConfig().ImmovableObjects)
                {
                    immoCount++;
                    var distance = po.Location.GetDistance(immo.Location);
                    var interval = Intervals.FirstOrDefault(x => x.Item1 <= distance && x.Item2 > distance);
                    if (null == interval)
                        continue;
                    if (!alreadyCounted[immoCount - 1])
                    {
                        partialTargetValue += interval.Item3;
                        alreadyCounted[immoCount - 1] = true;
                    }

                }
            }
            return partialTargetValue;
        }

        public ControlTemplate GetUiTemplate()
        {

            var uiElement = new ControlTemplate();
            var grid = new FrameworkElementFactory(typeof(Grid));
            //grid.SetValue(Grid.BackgroundProperty, Brushes.Blue);
            uiElement.VisualTree = grid;
            foreach (var tuple in Intervals)
            {
                var ellipse = new FrameworkElementFactory(typeof(Ellipse));
                SolidColorBrush b;
                if(0>tuple.Item3)
                    b = new SolidColorBrush(Color.FromArgb((byte)(-120*tuple.Item3), 255, 0, 0));
                else
                    b = new SolidColorBrush(Color.FromArgb((byte)(120 * tuple.Item3), 0, 255, 0));
                ellipse.SetValue(Ellipse.WidthProperty, (double)tuple.Item2*2);
                ellipse.SetValue(Ellipse.HeightProperty, (double)tuple.Item2*2);
                ellipse.SetValue(Ellipse.FillProperty, Brushes.Transparent);
                ellipse.SetValue(Ellipse.StrokeProperty, b);
                ellipse.SetValue(Ellipse.StrokeThicknessProperty, (double)(tuple.Item2 - tuple.Item1));

                grid.AppendChild(ellipse);
            }

            return uiElement;
        }
    }
}
