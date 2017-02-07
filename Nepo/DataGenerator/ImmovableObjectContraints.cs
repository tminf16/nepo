// ----------------------------------------------------------------------
// <copyright file="ImmovableObjectContraints.cs" author="Stefan Ghizelea">
//         Copyright © 2017 - Stefan Ghizelea All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


using Nepo.Common;

namespace Nepo.DataGenerator
{
    /// <summary>
    /// Defines constraints for generation of a set of <see cref="ImmovableObject"/>.
    /// </summary>
    public class ImmovableObjectContraints
    {
        /// <summary>
        /// Minimum distance between two <see cref="ImmovableObject"/>s.
        /// </summary>
        public int MinDistanceBetweenObjects { get; set; } = 0;

        /// <summary>
        /// Maximum distance between two <see cref="ImmovableObject"/>s.
        /// </summary>
        public int MaxDistanceBetweenObjects { get; set; } = int.MaxValue;

        /// <summary>
        /// Precision of the decimal number for weight generation.
        /// </summary>
        public int FractionPrecision { get; set; } = 5;
    }
}