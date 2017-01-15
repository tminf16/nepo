using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class Optimizer
    {
        private static Optimizer _instance = null;
        public static Optimizer Instance { get { return _instance ?? (_instance = new Optimizer()); } }

        private Solution currentlyAccepted = null;
        private List<Solution> availableChildren = null;
        private Tuple<Solution, List<Solution>> GenerateSolutions()
        {
            if (null == currentlyAccepted)
            {
                currentlyAccepted = new Solution();
                var map = DataHandler.GetMapConfig();
                currentlyAccepted.FillRandomValues(map.MapSize.Width, map.MapSize.Height, map.PlanningObjectCount);
            }
            List<Solution> children = new List<Solution>();
            for (int i = 0; i < 50; i++)
                children.Add(currentlyAccepted.CreateChildSolution());
            availableChildren = children;
            return new Tuple<Solution, List<Solution>>(currentlyAccepted, children);
        }

        public void Reset()
        {
            currentlyAccepted = null;
            availableChildren = null;
        }

        public Tuple<Solution, List<Solution>> SelectChild(int id)
        {
            if (0 == id)
            {
                if (null == currentlyAccepted)
                    return GenerateSolutions();
                else
                    return new Tuple<Solution, List<Solution>>(currentlyAccepted, availableChildren);
            }
            var child = availableChildren?.SingleOrDefault(x => x.SolutionID == id);
            if (null == child)
                return new Tuple<Solution, List<Solution>>(currentlyAccepted, availableChildren);
            currentlyAccepted = child;
            availableChildren = null;

            return GenerateSolutions();
        }
    }

    public class Solution
    {
        private static int IdCounter = 0;
        private static Random _rand = new Random((int)DateTime.Now.Ticks);
        public Solution()
        {
            IdCounter++;
            SolutionID = IdCounter;
        }
        public Solution(int objectCount)
        {
            IdCounter++;
            SolutionID = IdCounter;
            PlanningObjects = new PlanningObject[objectCount];
        }
        public int SolutionID { get; set; }
        public PlanningObject[] PlanningObjects { get; set; }

        internal void FillRandomValues(int width, int height, int planningObjectCount)
        {
            PlanningObjects = new PlanningObject[planningObjectCount];
            for (int i = 0; i < planningObjectCount; i++)
            {
                var po = new PlanningObject()
                {
                    Location = new System.Drawing.Point(_rand.Next(width), _rand.Next(height))
                };
                PlanningObjects[i] = po;
            }
        }

        internal Solution CreateChildSolution()
        {
            Solution tmpChild = new Solution(PlanningObjects.Length);
            int mutation = _rand.Next(PlanningObjects.Length);
            for (int i = 0; i < PlanningObjects.Length; i++)
            {
                tmpChild.PlanningObjects[i] = new PlanningObject();
                var tmpLocation = PlanningObjects[i].Location;

                if (i == mutation)
                {
                    tmpChild.PlanningObjects[i].Location =
                    new System.Drawing.Point(
                        (int)(tmpLocation.X + _rand.NextGaussian(sigma: 15)),
                        (int)(tmpLocation.Y + _rand.NextGaussian(sigma: 15)));
                }
                else
                {
                    tmpChild.PlanningObjects[i].Location =
                    new System.Drawing.Point(
                        (int)(tmpLocation.X),
                        (int)(tmpLocation.Y));
                }

            }
            return tmpChild;
        }
    }

    public static class RandomExtension
    {
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}
