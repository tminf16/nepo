using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.DataGenerator
{
    /// <summary>
    /// Defines the contraints for the generation of maps.
    /// </summary>
    public class MapGenerationConstraints
    {
        /// <summary>
        /// Minimum Map Size.
        /// </summary>
        public Size MinSize { get; set; } = new Size(500, 500);

        /// <summary>
        /// Maximum Map Size.
        /// </summary>
        public Size MaxSize { get; set; } = new Size(500, 500);

        /// <summary>
        /// Minimum Amount of <see cref="PlanningObject"/>s.
        /// </summary>
        public int MinPlanningObjectCount { get; set; } = 2;

        /// <summary>
        /// Maximum Amount of <see cref="PlanningObject"/>s.
        /// </summary>
        public int MaxPlanningObjectCount { get; set; } = 20;

        /// <summary>
        /// Minimum Amount of <see cref="ImmovableObject"/>s.
        /// </summary>
        public int MinImmovableObjectCount { get; set; } = 30;

        /// <summary>
        /// Maximum Amount of <see cref="ImmovableObject"/>s.
        /// </summary>
        public int MaxImmovableObjectCount { get; set; } = 30;

        /// <summary>
        /// Minimum Amount of <see cref="Layer"/>s.
        /// </summary>
        public int MinLayerCount { get; set; } = 1;

        /// <summary>
        /// Maximum Amount of <see cref="Layer"/>s.
        /// </summary>
        public int MaxLayerCount { get; set; } = 1;

        /// <summary>
        /// Constraints for generating immovable objects.
        /// </summary>
        public ImmovableObjectContraints ImmovableObjectContraints { get; set; }        
    }
}
