﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Nepo.Common.Rules
{
    [DataContract]
    [XmlInclude(typeof(DistanceIntervalsRule))]
    [XmlInclude(typeof(CurveRule))]
    public abstract class ITargetFunctionComponent
    { 
        
        public abstract double CalculatePartialTargetValue(Solution solution);
        public abstract ControlTemplate GetUiTemplate();
    }
}
