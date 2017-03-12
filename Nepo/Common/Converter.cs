using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class NullToTestimageConverter : System.Windows.Data.IValueConverter
    {
        static Resources res;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == res)
            {
                res = new Resources();
                res.InitializeComponent();
            }
            if (null == value)
                return res["zahnrad"];
            var list = (value as Instance).AgentConfigs as List<AgentConfig>;
            if (null != list && list.Count == 0)
                return res["zahnrad"];
            return res["testbild"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
