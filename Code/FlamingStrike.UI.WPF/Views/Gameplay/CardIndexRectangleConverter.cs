using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace FlamingStrike.UI.WPF.Views.Gameplay
{
    public class CardIndexRectangleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //<!--<Rectangle Opacity=".5" />
            //                                <Rectangle Opacity=".75" 
            //                                           Canvas.Left="2" 
            //                                           Canvas.Top="1" />
            //                                <Rectangle Canvas.Left="4" 
            //                                           Canvas.Top="2" />
            //                                <Rectangle Canvas.Left="6" 
            //                                           Canvas.Top="3" />
            //                                <Rectangle Canvas.Left="8" 
            //                                           Canvas.Top="4" />-->

            var numberOfCards = (int)value;

            var rectangles = new List<FrameworkElement>();
            for (var i = 0; i < numberOfCards; i++)
            {
                var container = new Canvas();
                var rectangle = new Rectangle();
                container.Children.Add(rectangle);

                rectangle.SetValue(Canvas.LeftProperty, i * 2.0);
                rectangle.SetValue(Canvas.TopProperty, i * 0.0);
                rectangle.Opacity = (i + 1.0) / (numberOfCards + 1);

                rectangles.Add(container);
            }

            return rectangles;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}