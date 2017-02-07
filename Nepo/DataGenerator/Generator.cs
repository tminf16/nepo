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


        public static Instance GenerateInstance(MapGenerationConstraints constraints, Random rng)
        {
            var result = new Instance
            {
                Map =
                {
                    MapSize = rng.NextSize(constraints.MinSize, constraints.MaxSize),
                    PlanningObjectCount = rng.Next(constraints.MinPlanningObjectCount, constraints.MaxPlanningObjectCount)
                }
            };
            var immovableObjectCount = rng.Next(constraints.MinImmovableObjectCount, constraints.MaxImmovableObjectCount);                        
            result.Map.ImmovableObjects = GenerateImmovableObjects(immovableObjectCount, constraints, rng);            
            return result;
        }

        public static Layer GenerateLayer(MapGenerationConstraints constraints, Random rng, MapConfig map)
        {
            var layer = new Layer();

            var bitmap = new Bitmap(map.MapSize.Height, map.MapSize.Width);
        }

        

        private static List<ImmovableObject> GenerateImmovableObjects(int count, MapGenerationConstraints constraints, Random rng)
        {
            var result = new List<ImmovableObject>();

            var weightSeeds = new List<decimal>();
            
            var factor = Math.Pow(10, constraints.ImmovableObjectContraints.FractionPrecision);
            for (var i = 1; i < count; ++i) // The missing Equals sign is not a typo, since we use the distances between the generated numbers as weights we need to generate one fewer.
            {
                weightSeeds.Add(rng.Next(0, (int) factor) / (decimal)factor);
            }
            weightSeeds.Add(0);
            weightSeeds.Add(1);

            weightSeeds.Sort();

            for(var i = 1; i <= count; ++i)
            {
                var immovableObject = new ImmovableObject()
                {

                    Weight = weightSeeds[i] - weightSeeds[i - 1]
                };

                var conflict = false;
                do
                {
                    immovableObject.Location = new Point(rng.Next(0, constraints.MaxSize.Width), rng.Next(0, constraints.MaxSize.Height));
                    foreach (var immo in result)
                    {
                        var distance = immo.Location.GetDistance(immovableObject.Location);
                        if (distance < constraints.ImmovableObjectContraints.MinDistanceBetweenObjects || distance > constraints.ImmovableObjectContraints.MaxDistanceBetweenObjects)
                        {
                            conflict = true;
                            break;
                        }
                    }
                } while (conflict);

                result.Add(immovableObject);
            }

            return result;
        }
    }

    /// <summary>
    /// Extensions for Random Class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Gets a random size.
        /// </summary>
        /// <param name="rng">Random object.</param>
        /// <param name="minValue">Minimum Size.</param>
        /// <param name="maxValue">Maximum Size.</param>
        /// <returns>Returns a random size between min and max.</returns>
        /// <remarks>
        /// New Size will have a height between min.Height and max.Height, same for Width.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// MinValue.Height is greater than MaxValue.Height or MinValue.Width is greater than MaxValue.Height.
        /// </exception>
        public static Size NextSize(this Random rng, Size minValue, Size maxValue)
        {
            return new Size(rng.Next(minValue.Width, maxValue.Width), rng.Next(minValue.Height, maxValue.Height));
        }
    }
}