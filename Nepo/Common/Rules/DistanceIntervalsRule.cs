using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Nepo.Common.Rules
{
    [DataContract]
    public class DistanceIntervalsRule : TargetFunctionComponentBase
    {
        [DataMember]
        public List<Interval> Intervals { get; set; }

        public DistanceIntervalsRule()
        {
            Intervals = new List<Interval>();
        }

        public void AddInterval(int min, int max, double value)
        {
            if (max < min)
                return;
            if (value > 1)
                value = 1;
            if (value < -1)
                value = -1;
            Intervals.Add(new Interval(min, max, value));
        }

        override
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
                    var interval = Intervals.FirstOrDefault(x => x.Min <= distance && x.Max > distance);
                    if (null == interval)
                        continue;
                    if (!alreadyCounted[immoCount - 1])
                    {
                        partialTargetValue += interval.Value* (double)immo.Weight;
                        alreadyCounted[immoCount - 1] = true;
                    }

                }
            }
            return partialTargetValue;
        }

        override
        public ControlTemplate GetUiTemplate()
        {

            var uiElement = new ControlTemplate();
            var grid = new FrameworkElementFactory(typeof(Grid));
            //grid.SetValue(Grid.BackgroundProperty, Brushes.Blue);
            uiElement.VisualTree = grid;
            foreach (var interval in Intervals)
            {
                var ellipse = new FrameworkElementFactory(typeof(Ellipse));
                SolidColorBrush b;
                if(0>interval.Value)
                    b = new SolidColorBrush(Color.FromArgb((byte)(-120*interval.Value), 255, 0, 0));
                else
                    b = new SolidColorBrush(Color.FromArgb((byte)(120 * interval.Value), 0, 255, 0));
                ellipse.SetValue(Ellipse.WidthProperty, (double)interval.Max*2);
                ellipse.SetValue(Ellipse.HeightProperty, (double)interval.Max*2);
                ellipse.SetValue(Ellipse.FillProperty, Brushes.Transparent);
                ellipse.SetValue(Ellipse.StrokeProperty, b);
                ellipse.SetValue(Ellipse.StrokeThicknessProperty, (double)(interval.Max - interval.Min));

                grid.AppendChild(ellipse);
            }

            return uiElement;
        }
    }

    [DataContract]
    public class Interval
    {
        [DataMember]
        public int Min { get; set; }
        [DataMember]
        public int Max { get; set; }
        [DataMember]
        public double Value { get; set; }

        public Interval()
        {

        }

        public Interval(int Min, int Max, double Value)
        {
            this.Min = Min;
            this.Max = Max;
            this.Value = Value;
        }
    }
}
