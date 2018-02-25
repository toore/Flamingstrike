using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlamingStrike.UI.WPF.Views.Gameplay
{
    public class NumberOfCardsToCanvasDrawingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var numberOfCards = (int)value;

            return Enumerable.Range(0, numberOfCards)
                .Select(i => CreateFrameworkElement(i, numberOfCards))
                .ToList();
        }

        private static FrameworkElement CreateFrameworkElement(int index, int numberOfCards)
        {
            var left = index * 2.0;
            var top = index * 0.25;
            var opacity = GetCardOpacity(index, numberOfCards);

            var canvas = new Canvas();
            canvas.Children.Add(CreateRectangle(left, top, opacity));
            return canvas;
        }

        private static double GetCardOpacity(int index, int numberOfCards)
        {
            return (index + 1.0) / numberOfCards;
        }

        private static Rectangle CreateRectangle(double left, double top, double opacity)
        {
            var rectangle = new Rectangle();
            rectangle.SetValue(Canvas.LeftProperty, left);
            rectangle.SetValue(Canvas.TopProperty, top);
            rectangle.Opacity = opacity;
            rectangle.Stroke = Brushes.Black;

            return rectangle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}