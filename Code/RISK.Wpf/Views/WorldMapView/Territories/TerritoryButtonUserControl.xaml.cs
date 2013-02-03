using System.Windows;
using System.Windows.Media;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public partial class TerritoryButtonUserControl
    {
        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.Register("PathStyle", typeof(Style), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Style)));
        public static readonly DependencyProperty GeometryChildrenProperty = DependencyProperty.Register("GeometryChildren", typeof(GeometryCollection), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(GeometryCollection)));
        public static readonly DependencyProperty NormalFillColorProperty = DependencyProperty.Register("NormalFillColor", typeof(Color), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty NormalStrokeColorProperty = DependencyProperty.Register("NormalStrokeColor", typeof(Color), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty MouseOverFillColorProperty = DependencyProperty.Register("MouseOverFillColor", typeof(Color), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty MouseOverStrokeColorProperty = DependencyProperty.Register("MouseOverStrokeColor", typeof(Color), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Color)));

        public TerritoryButtonUserControl()
        {
            InitializeComponent();
        }

        public Style PathStyle
        {
            get { return (Style)GetValue(PathStyleProperty); }
            set { SetValue(PathStyleProperty, value); }
        }

        public GeometryCollection GeometryChildren
        {
            get { return (GeometryCollection)GetValue(GeometryChildrenProperty); }
            set { SetValue(GeometryChildrenProperty, value); }
        }
    

        public Color NormalFillColor
        {
            get { return (Color)GetValue(NormalFillColorProperty); }
            set { SetValue(NormalFillColorProperty, value); }
        }

        public Color NormalStrokeColor
        {
            get { return (Color)GetValue(NormalStrokeColorProperty); }
            set { SetValue(NormalStrokeColorProperty, value); }
        }

        public Color MouseOverFillColor
        {
            get { return (Color)GetValue(MouseOverFillColorProperty); }
            set { SetValue(MouseOverFillColorProperty, value); }
        }

        public Color MouseOverStrokeColor
        {
            get { return (Color)GetValue(MouseOverStrokeColorProperty); }
            set { SetValue(MouseOverStrokeColorProperty, value); }
        }
    }
}