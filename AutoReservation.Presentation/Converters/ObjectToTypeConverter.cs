using AutoReservation.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoReservation.Presentation.Converters
{
    class ObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                string name = value.GetType().Name;
                int underscoreIndex = name.IndexOf('_');
                return underscoreIndex != -1 ? name.Substring(0, underscoreIndex) : name;
            }

            return default(object);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
