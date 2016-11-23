using System.Windows.Media;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class PlayerCardStatusViewModel
    {
        public PlayerCardStatusViewModel(string playerName, Color playerColor, int numberOfCards)
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