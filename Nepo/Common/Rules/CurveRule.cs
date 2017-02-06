using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Nepo.Common.Rules
{
    public class CurveRule : ITargetFunctionComponent
    {
        public int Min { get; set; } = 15;
        public int Max { get; set; } = 100;

        private double HalfDelta;
        private double Mid;
        private double MinusHalfDeltaSquare;
        /// <summary>
        /// Creates a rule where all values between min an max are positive, values less than min are negative and all other values are 0.
        /// The highest value is 1, the lowest value is -1. The function that descripes all values is as following:
        /// f(x)= -1 / ((max-min)/2)^2 * (x - (min + max) / 2 )^2  + 1
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public CurveRule(int min, int max)
        {
            Min = min;
            Max = max;
            HalfDelta = (Max - Min) / 2.0;
            Mid = Min + HalfDelta;
            MinusHalfDeltaSquare = -1.0 / HalfDelta / HalfDelta;
        }

        public double CalculatePartialTargetValue(Solution solution)
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
                    double value = F(distance, correction);// *immo.Weight;

                    if (0.0 != alreadyCounted[immoCount - 1])
                    {
                        if (alreadyCounted[immoCount - 1] < value)
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

        public ControlTemplate GetUiTemplate()
        {

            var uiElement = new ControlTemplate();
            var grid = new FrameworkElementFactory(typeof(Grid));
            //grid.SetValue(Grid.BackgroundProperty, Brushes.Blue);
            uiElement.VisualTree = grid;
            for (int i = 0; i < 2; i++)
            {
                var ellipse = new FrameworkElementFactory(typeof(Ellipse));
                SolidColorBrush b;
                if (0 == i)
                    b = new SolidColorBrush(Color.FromArgb((byte)(120), 255, 0, 0));
                else
                    b = new SolidColorBrush(Color.FromArgb((byte)(120 ), 0, 255, 0));
                ellipse.SetValue(Ellipse.WidthProperty, (double)(0 == i ? Min : Max) * 2);
                ellipse.SetValue(Ellipse.HeightProperty, (double)(0 == i ? Min : Max) * 2);
                ellipse.SetValue(Ellipse.FillProperty, Brushes.Transparent);
                ellipse.SetValue(Ellipse.StrokeProperty, b);
                if(i == 0)
                    ellipse.SetValue(Ellipse.StrokeThicknessProperty, (double)(Min));
                else
                    ellipse.SetValue(Ellipse.StrokeThicknessProperty, (double)(Max - Min));

                grid.AppendChild(ellipse);
            }

            return uiElement;
        }
    }
}
