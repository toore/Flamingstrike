using System.Windows;

namespace RISK.WorldMap.Territories
{
    public partial class TerritoryButtonUserControl
    {
        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.Register("PathStyle", typeof(Style), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(Style)));
        public static readonly DependencyProperty TerritoryNameProperty = DependencyProperty.Register("TerritoryName", typeof(string), typeof(TerritoryButtonUserControl), new PropertyMetadata(default(string)));

        public TerritoryButtonUserControl()
        {
            InitializeComponent();
        }

        public Style PathStyle
        {
            get { return (Style)GetValue(PathStyleProperty); }
            set { SetValue(PathStyleProperty, value); }
        }

        public string TerritoryName
        {
            get { return (string)GetValue(TerritoryNameProperty); }
            set { SetValue(TerritoryNameProperty, value); }
        }
    }
}