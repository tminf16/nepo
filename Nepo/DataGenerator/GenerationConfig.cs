﻿// ----------------------------------------------------------------------
// <copyright file="InstanceGenerationConstraints.cs" author="Stefan Ghizelea">
//         Copyright © 2017 - Stefan Ghizelea All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


namespace Nepo.DataGenerator
{
    /// <summary>
    /// Config for instance generation.
    /// </summary>
    public class GenerationConfig
    {
        /// <summary>
        /// Seed used for instance generation. Set to -1 for no seed.
        /// </summary>
        public int Seed { get; set; } = 0;

        /// <summary>
        /// Number of Instances to generate.
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Constraints for Map Generation.
        /// </summary>
        public MapGenerationConstraints Constraints { get; set; } = new MapGenerationConstraints();
    }
}