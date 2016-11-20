using System.Windows.Media;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace Tests.FlamingStrike.GameEngine.Builders
{
    public class PlayerUiDataBuilder
    {
        public IPlayerUiData Build()
        {
            return new PlayerUiData(Make.Player.Build(), Colors.Black);
        }
    }
}