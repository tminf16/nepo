using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Nepo.Common.Rules
{
    [DataContract]
    public class CurveRule : ITargetFunctionComponent
    {
        [DataMember]
        public int Min { get; set; } = 15;
        [DataMember]
        public int Max { get; set; } = 100;

        private double? _HalfDelta = null;
        private double HalfDelta { get { return (double)(_HalfDelta ?? (_HalfDelta = (Max - Min) / 2.0)); } }
        private double? _Mid = null;
        private double Mid { get { return (double)(_Mid ?? (_Mid = (Min + HalfDelta))); } }
        private double? _MinusHalfDeltaSquare = null;
        private double MinusHalfDeltaSquare { get { return (double)(_MinusHalfDeltaSquare ?? (_MinusHalfDeltaSquare = -1.0 / HalfDelta / HalfDelta)); } }
        /// <summary>
        /// Creates a rule where all values between min an max are positive, values less than min are negative and all other values are 0.
        /// The highest value is 1, the lowest value is -1. The function that descripes all values is as following:
        /// f(x)= -1 / ((max-min)/2)^2 * (x - (min + max) / 2 )^2  + 1
        ///
        ///     min      max
        /// +1|  |   __   |
        ///   |  | ´    ` |
        ///   |  |/      \|
        ///  0|--+--------+--
        ///   | /
        /// -1|/ 
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public CurveRule(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public CurveRule()
        {

        }

        override public double CalculatePartialTargetValue(Solution solution)
        {
            double[] alreadyCounted = new double[DataHandler.GetMapConfig().ImmovableObjects.Count];
            for (int i = 0; i < alreadyCounted.Length; i++)
                alreadyCounted[i] = 0;
            double partialTargetValue = 0;
            var progress = solution.Progress - (solution.Progress % 20);
            double correction = (100 - progress) / 100.0;
            foreach (var po in solution.PlanningObjects)
            {
                int immoCount = 0;
                foreach (var immo in DataHandler.GetMapConfig().ImmovableObjects)
                {
                    immoCount++;
                    var distance = po.Location.GetDistance(immo.Location);
                    double value = F(distance, correction) *(double)immo.Weight;

                    if (0.0 < alreadyCounted[immoCount - 1])
                    {
                        if (alreadyCounted[immoCount - 1] < value )
                        {
                            partialTargetValue -= alreadyCounted[immoCount - 1];
                            alreadyCounted[immoCount - 1] = value;
                            partialTargetValue += value;
                        }
                    }
                    else
                    {
                        alreadyCounted[immoCount - 1] = value;
                        partialTargetValue += value;
                    }
                }
            }
            return partialTargetValue;
        }

        private double F(double x, double correction)
        {
            if (Max < x)
                return 0.0;
            var value = Math.Max(-1, MinusHalfDeltaSquare * Math.Pow(x - Mid, 2) + 1);
            var correctedValue = value;
            if(value > 0)
                correctedValue = (1 - value) * correction + value;
            else
                correctedValue = (-1 - value) * correction + value;
            return correctedValue;
        }

        override
        public ControlTemplate GetUiTemplate()
        {

            var uiElement = new ControlTemplate();
            var grid = new FrameworkElementFactory(typeof(Grid));
            //grid.SetValue(Grid.BackgroundProperty, Brushes.Blue);
            uiElement.VisualTree = grid;
            //for (int i = 0; i < 2; i++)
            {
                var ellipse = new FrameworkElementFactory(typeof(Ellipse));
                ellipse.SetValue(Ellipse.WidthProperty, Max * 2.0);
                ellipse.SetValue(Ellipse.HeightProperty, Max * 2.0);
                ellipse.SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Color.FromArgb(120, 0,0,0)));
                ellipse.SetValue(Ellipse.StrokeThicknessProperty, 0.5);

                RadialGradientBrush brush = new RadialGradientBrush()
                {
                    GradientOrigin = new Point(0.5, 0.5),
                    Center = new Point(0.5, 0.5)
                };
                brush.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(120), 255, 0, 0), 0));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0 * Min / Max));
                brush.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(120), 0, 255, 0), 1.0 * (Min + (Max - Min) / 3.0) / Max));
                brush.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(120), 0, 255, 0), 1.0 * (Max - (Max - Min) / 3.0) / Max));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

                
                grid.SetValue(Grid.WidthProperty, 2.0 * Max);
                grid.SetValue(Grid.HeightProperty, 2.0 * Max);
                grid.SetValue(Grid.BackgroundProperty, brush);
                grid.AppendChild(ellipse);
            }

            return uiElement;
        }
    }
}
