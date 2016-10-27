using System.Windows.Media;
using RISK.UI.WPF.ViewModels.Preparation;

namespace Tests.RISK.GameEngine.Builders
{
    public class PlayerUiDataBuilder
    {
        public IPlayerUiData Build()
        {
            return new PlayerUiData(Make.Player.Build(), Colors.Black);
        }
    }
}