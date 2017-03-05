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
using Nepo.Common.Rules;

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
            
            
            var result = new List<Instance>();

            for (int i = 1; i <= constraints.InstanceCount; ++i)
            {
                result.Add(GenerateInstance(constraints));
            }

            var tcs = new TaskCompletionSource<List<Instance>>();
            tcs.SetResult(result);
            return tcs.Task;
        }


        public static Instance GenerateInstance(GenerationConfig config)
        {
            return GenerateInstance(config, Guid.NewGuid());
        }

        public static Instance GenerateInstance(GenerationConfig config, Guid guid)
        {                                    
            var rng = new Random(guid.GetHashCode());
            var accepted_solutions = 1; // Solutions, clients are forced to accept
            var constraints = config.Constraints;
            var result = new Instance
            {
                InstanceId = guid,
                Map =
                {
                    MapSize = rng.NextSize(constraints.MinSize, constraints.MaxSize),
                    PlanningObjectCount = rng.Next(constraints.MinPlanningObjectCount, constraints.MaxPlanningObjectCount),
                    ForcedAcceptance = accepted_solutions
                }
            };
            var immovableObjectCount = rng.Next(constraints.MinImmovableObjectCount, constraints.MaxImmovableObjectCount);
            result.Map.ImmovableObjects = GenerateImmovableObjects(immovableObjectCount, constraints, rng);
            result.Map.Layers = GenerateLayers(constraints, rng, result);
            result.AgentConfigs = new List<AgentConfig>();
            for (int i = 0; i < config.AgentsCount; i++)
            {
                var ac = new AgentConfig();
                switch (i%2)
                {
                    case 0:
                        var intRule = new DistanceIntervalsRule();
                        intRule.AddInterval(0, 150, 1);
                        ac.Rules.Add(intRule);
                        break;
                    case 1:
                        var curveRule = new CurveRule(50, 150);
                        ac.Rules.Add(curveRule);
                        break;
                }
                ac.Layers.Add(new Layer()
                {
                    FileName = "HeightMap.png",
                    Map = BitmapGenerator.AddHeightNoise(new Bitmap(result.Map.MapSize.Width, result.Map.MapSize.Height), rng)
                });
                result.AgentConfigs.Add(ac);
            }
            return result;
        }

        public static List<Layer> GenerateLayers(MapGenerationConstraints constraints, Random rng, Instance instance)
        {
            var result = new List<Layer>();
            var layerOne = new Layer()
            {
                FileName = $"Deadzones.png",
                Map = BitmapGenerator.AddBlobs(new Bitmap(instance.Map.MapSize.Width, instance.Map.MapSize.Height), rng, constraints)
            };            
            result.Add(layerOne);
            return result;
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