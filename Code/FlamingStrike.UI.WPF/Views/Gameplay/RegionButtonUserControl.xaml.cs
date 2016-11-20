using System.Windows;
using System.Windows.Media;

namespace FlamingStrike.UI.WPF.Views.Gameplay
{
    public partial class RegionButtonUserControl
    {
        public static readonly DependencyProperty GeometryChildrenProperty = DependencyProperty.Register("GeometryChildren", typeof(GeometryCollection), typeof(RegionButtonUserControl), new PropertyMetadata(default(GeometryCollection)));
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(RegionButtonUserControl), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register("StrokeColor", typeof(Color), typeof(RegionButtonUserControl), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(RegionButtonUserControl), new PropertyMetadata(default(bool)));

        public RegionButtonUserControl()
        {
            InitializeComponent();
        }

        public GeometryCollection GeometryChildren
        {
            get { return (GeometryCollection)GetValue(GeometryChildrenProperty); }
            set { SetValue(GeometryChildrenProperty, value); }
        }

        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }
    
        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
    }
}