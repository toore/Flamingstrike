using System.Collections.Generic;
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
        bool CanEnterFortifyMode { get; }
        bool CanEnterAttackMode { get; }
        bool CanEndTurn { get; }
        void EnterAttackMode();
        void EnterFortifyMode();
        void EndTurn();
        void EndGame();
    }
}