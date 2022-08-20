using System.Threading.Tasks;
using System.Windows.Media;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;

namespace FlamingStrike.UI.WPF.ViewModels
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
        string InformationText { get; }
        string PlayerName { get; }
        Color PlayerColor { get; }
        bool CanUserSelectNumberOfArmies { get; }
        bool CanShowCards { get; }
        bool CanEnterFortifyMode { get; }
        bool CanEnterAttackMode { get; }
        bool CanEndTurn { get; }
        void EnterAttackMode();
        void EnterFortifyMode();
        void EndTurn();
        Task EndGameAsync();
    }
}