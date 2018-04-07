using System.Windows.Media;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;

namespace Tests.UI.WPF
{
    public class PlayerStatusViewModelBuilder
    {
        public PlayerStatusViewModel Build()
        {
            return new PlayerStatusViewModel("player", Colors.White, 1);
        }
    }
}