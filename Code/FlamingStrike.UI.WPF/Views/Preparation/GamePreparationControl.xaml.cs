using System.Collections;
using System.Windows;

namespace FlamingStrike.UI.WPF.Views.Preparation
{
    public partial class GamePreparationControl
    {
        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(IList), typeof(GamePreparationControl), new PropertyMetadata(default(IList)));

        public GamePreparationControl()
        {
            InitializeComponent();
        }

        public IList Players
        {
            get { return (IList)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }
    }
}