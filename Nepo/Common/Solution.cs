using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Nepo.Common
{
    [DataContract]
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

        [DataMember]
        public int SolutionID { get; set; }

        [DataMember]
        public PlanningObject[] PlanningObjects { get; set; }
        /// <summary>
        /// Overall progress of the optimization between 0 and 100. Agents can adjust the target function depending on the progress.
        /// </summary>
        [DataMember]
        public int Progress { get; set; }

        internal void FillRandomValues(MapConfig map)
        {
            PlanningObjects = new PlanningObject[map.PlanningObjectCount];
            for (int i = 0; i < map.PlanningObjectCount; i++)
            {
                var po = new PlanningObject()
                {
                    Location = new System.Drawing.Point(_rand.Next(map.MapSize.Width), _rand.Next(map.MapSize.Height))
                };
                foreach (var layer in map.Layers)
                {
                    var bmp = layer.Map;
                    if (!po.IsInMap(map))
                        continue;
                    var pixel = bmp.GetPixel(po.Location.X, po.Location.Y);
                    if (0 != pixel.A)
                    {
                        i--;
                        continue;
                    }
                }
                PlanningObjects[i] = po;
            }
        }

        internal Solution CreateChildSolution(MapConfig map)
        {
            Solution tmpChild = new Solution(PlanningObjects.Length)
            {
                Progress = Progress
            };
            int mutation = _rand.Next(PlanningObjects.Length);
            for (int i = 0; i < PlanningObjects.Length; i++)
            {
                tmpChild.PlanningObjects[i] = new PlanningObject();
                var tmpLocation = PlanningObjects[i].Location;

                if (i == mutation)
                {
                    var po = tmpChild.PlanningObjects[i];
                    po.Location = new System.Drawing.Point(
                            (int)(tmpLocation.X + _rand.NextGaussian(sigma: 15)),
                            (int)(tmpLocation.Y + _rand.NextGaussian(sigma: 15)));

                    foreach (var layer in map.Layers)
                    {
                        var bmp = layer.Map;
                        if (!po.IsInMap(map))
                            continue;
                        Color pixel;
                        lock (bmp)
                            pixel = bmp.GetPixel(po.Location.X, po.Location.Y);
                        if (0 != pixel.A)
                        {
                            i--;
                            continue;
                        }
                    }
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
}