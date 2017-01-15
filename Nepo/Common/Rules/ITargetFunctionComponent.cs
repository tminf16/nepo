using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nepo.Common.Rules
{
    public interface ITargetFunctionComponent
    {
        double CalculatePartialTargetValue(Solution solution);
        ControlTemplate GetUiTemplate();
    }
}
