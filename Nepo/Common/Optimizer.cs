﻿using Nepo.Common.Rules;
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
        //public int maxRounds { get; set;  } = 0; //initialisiert durch MapConfig
        private int currentRound = 0;
        public static int childsCount = 100;


        public MapConfig map;

        public void SetMap(MapConfig map)
        {
            this.map = map;
        }
        private Tuple<Solution, List<Solution>> GenerateSolutions()
        {
            if (null == currentlyAccepted)
            {
                currentlyAccepted = new Solution();
                currentlyAccepted.FillRandomValues(map);
            }
            List<Solution> children = new List<Solution>();
            children.Add(currentlyAccepted);
            currentRound++;
            currentlyAccepted.Progress = (int)((100.0 * currentRound) / map.MaxRounds);
            for (int i = 0; i < childsCount; i++)
                children.Add(currentlyAccepted.CreateChildSolution(map));
            availableChildren = children;
            return new Tuple<Solution, List<Solution>>(currentlyAccepted, children);
        }

        public void Reset()
        {
            currentlyAccepted = null;
            availableChildren = null;
            currentRound = 0;
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

        public static List<int> FindBestSolutions(List<Solution> availableChildSolutions, AgentConfig config, MapConfig map, int count)
        {
            List<Tuple<int, double>> targetValues = new List<Tuple<int, double>>();
            foreach (var sol in availableChildSolutions)
            {
                targetValues.Add(new Tuple<int, double>(sol.SolutionID, CalculateTargetValue(sol, config, map)));
            }
            var results = targetValues.OrderByDescending(x => x.Item2).ThenByDescending(x=>x.Item1).Take(count).Select(x=>x.Item1).ToList();
            return results;
        }

        public static double CalculateTargetValue(Solution currentSolution, AgentConfig config, MapConfig map)
        {
            double targetvalue = 0;
            foreach (var rule in config.Rules)
            {
                targetvalue += rule.CalculatePartialTargetValue(currentSolution, map);
            }
            foreach (var po in currentSolution.PlanningObjects)
            {
                foreach (var layer in config.Layers)
                {
                    if (!po.IsInMap(map))
                        continue;
                    var color = layer.Map.GetPixel(po.Location.X, po.Location.Y);
                    var brightness = color.A / 255.0;
                    //var factor = (brightness * 2 - 1);
                    targetvalue += brightness * layer.Weight;
                }
            }

            return targetvalue;
        }

        private static Random _rand = new Random((int)DateTime.Now.Ticks);
        private static int _agentCounter = 0;
        public void FindNewAcceptedSolution(List<Tuple<Guid, int>> list)
        {
            var agentsCount = list.GroupBy(x => x.Item1).Count();
            var results = list.GroupBy(x => x.Item2).OrderByDescending(x => x.Count()).Select(x=>new { id = x.Key, count = x.Count() });
            var selection = results.First();
            SelectChild(selection.id);
        }
        public void FindNewAcceptedSolution(List<List<int>> list)
        {
            var merged = list.SelectMany(x=>x);
            var grouped = merged.GroupBy(x => x).OrderByDescending(x => x.Count());
            var result = grouped.FirstOrDefault();
            if (list.Count() == result.Count())
            {
                SelectChild(result.FirstOrDefault());
                return;
            }
            _agentCounter %= list.Count();
            var selection = list[_agentCounter][_rand.Next(list[_agentCounter].Count())];
            _agentCounter++;
            SelectChild(selection);
        }
        public void FindNewAcceptedSolution(List<int> list)
        {
            var results = list.OrderByDescending(x => x).ToList();
            var selection = results.First();
            SelectChild(selection);
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
