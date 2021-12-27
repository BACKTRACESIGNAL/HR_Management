using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HR_Management.Converter
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Nullable<bool> gender = value as Nullable<bool>;
            switch (gender)
            {
                case true:
                    return 0;
                case false:
                    return 1;
                default:
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Nullable<int> gender = value as Nullable<int>;
            switch (gender)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                default:
                    return true;
            }

        }
    }
}
