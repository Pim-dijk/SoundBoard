using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundBoard.Utilities
{
    public class NotEmptyStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object value in values)
            {
                if (value.ToString() == "")
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("NotEmptyStringConverter is a OneWay converter.");
        }
    }
}