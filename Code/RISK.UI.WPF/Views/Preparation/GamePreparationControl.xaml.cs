using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.Views.Preparation
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