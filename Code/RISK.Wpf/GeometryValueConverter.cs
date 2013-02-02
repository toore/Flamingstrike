using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using RISK.Domain.Extensions;

namespace RISK
{
    [ValueConversion(typeof(string), typeof(GeometryCollection))]
    public class GeometryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geometry = Geometry.Parse((string)value);
            return new GeometryCollection(geometry.AsList());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}