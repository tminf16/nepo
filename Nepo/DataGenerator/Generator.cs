// ----------------------------------------------------------------------
// <copyright file="Generator.cs" author="Stefan Ghizelea">
//         Copyright © 2017 - Stefan Ghizelea All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Nepo.Common;

namespace Nepo.DataGenerator
{
    public static class Generator
    {
        /// <summary>
        /// Generates a List of Instances
        /// </summary>
        /// <param name="count">Number of Instances to generate.</param>
        /// <param name="constraints">Contraints for map generation.</param>
        /// <returns></returns>
        public static Task<List<Instance>> GenerateInstances(GenerationConfig constraints = null)
        {
            if (constraints == null)
            {
                constraints = new GenerationConfig();
            }

            var rng = constraints.Seed == -1 ? new Random() : new Random(constraints.Seed);
            var result = new List<Instance>();

            for (int i = 1; i <= constraints.InstanceCount; ++i)
            {
                result.Add(GenerateInstance(constraints.Constraints, rng));
            }

            var tcs = new TaskCompletionSource<List<Instance>>();
            tcs.SetResult(result);
            return tcs.Task;
        }

        internal static Instance GenerateInstance(MapGenerationConstraints constraints, Random rng)
        {

            var result = new Instance();
            result.Map.MapSize = rng.NextSize(constraints.MinSize, constraints.MaxSize);
            result.Map.PlanningObjectCount = rng.Next(constraints.MinPlanningObjectCount, constraints.MaxPlanningObjectCount);


            return result;
        }
    }

    public static class RandomExtensions
    {
        public static Size NextSize(this Random rng, Size min, Size max)
        {
            return new Size(rng.Next(min.Width, max.Width), rng.Next(min.Height, max.Height));
        }
    }
}