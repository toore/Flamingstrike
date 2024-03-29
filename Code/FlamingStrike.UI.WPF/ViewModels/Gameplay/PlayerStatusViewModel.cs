using System.Windows.Media;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class PlayerStatusViewModel
    {
        public PlayerStatusViewModel(string playerName, Color playerColor, int numberOfCards)
        {
            PlayerName = playerName;
            PlayerColor = playerColor;
            NumberOfCards = numberOfCards;
        }

        public string PlayerName { get; }

        public Color PlayerColor { get; }

        public int NumberOfCards { get; }
    }
}