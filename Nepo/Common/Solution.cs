using System;
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
}