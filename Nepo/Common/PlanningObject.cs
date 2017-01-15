using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class PlanningObject
    {
        public Point Location { get; set; }
    }

    public static class PointExtensions
    {
        public static int GetDistance(this Point firstPoint, Point secondPoint)
        {
            return (int)Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }
    }
}
