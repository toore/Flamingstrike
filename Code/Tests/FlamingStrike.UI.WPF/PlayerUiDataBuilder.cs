using System.Windows.Media;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using Tests.FlamingStrike.GameEngine;

namespace Tests.FlamingStrike.UI.WPF
{
    public class PlayerUiDataBuilder
    {
        private Color _color;

        public IPlayerUiData Build()
        {
            return new PlayerUiData(new PlayerBuilder().Build(), _color);
        }

        public PlayerUiDataBuilder Color(Color color)
        {
            _color = color;
            return this;
        }
    }
}