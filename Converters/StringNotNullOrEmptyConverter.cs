using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace NotasAcademicasApp.Converters
{
    public class StringNotNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !string.IsNullOrEmpty(value as string);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}