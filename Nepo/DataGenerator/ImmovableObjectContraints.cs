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
        public int MaxDistanceBetweenObjects { get; set; } = 0;

        /// <summary>
        /// Type of Weigth to be used for Weight generation.
        /// </summary>
        public WeightType WeightType { get; set; } = WeightType.DecimalTotal;

        /// <summary>
        /// Minimum weight to be generated.
        /// </summary>
        /// <remarks>
        /// Is ignored if <see F:cref="WeightType.DecimalTotal"/> is used.
        /// Is floored if <see F:cref="WeightType.Absolute"/>is used.
        /// </remarks>
        public double MinWeight { get; set; } = 1;

        /// <summary>
        /// Maximum weight to be generated.
        /// </summary>
        /// <remarks>
        /// Is ignored if <see cref="F:WeightType.DecimalTotal"/> is used.
        /// Is floored if <see F:cref="WeightType.Absolute"/>is used.
        /// </remarks>
        public double MaxWeight { get; set; } = 10000;

        /// <summary>
        /// Precision of the decimal number for weight generation.
        /// </summary>
        public int FractionPrecision { get; set; } = 5;
    }

    /// <summary>
    /// Type of Weight to be generated.
    /// </summary>
    public enum WeightType
    {
        /// <summary>
        /// Generate integers defined by contraints.
        /// </summary>
        Absolute,

        /// <summary>
        /// Generate fractions between 0 and 1.
        /// </summary>
        Decimal,

        /// <summary>
        /// Generate fractions between 0 and 1 so that the total of all weights equals 1. 
        /// </summary>
        DecimalTotal
    }


}