using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoApp.Models;

namespace TodoApp.Converters
{
    public class DeadlineToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DeadlineStatus status)
            {
                switch (status)
                {
                    case DeadlineStatus.Overdue:
                    case DeadlineStatus.DueToday:
                        return new SolidColorBrush(Colors.Red);
                    case DeadlineStatus.DueTomorrow:
                        return new SolidColorBrush(Colors.Orange);
                    case DeadlineStatus.Normal:
                    default:
                        return new SolidColorBrush(Colors.Green);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}