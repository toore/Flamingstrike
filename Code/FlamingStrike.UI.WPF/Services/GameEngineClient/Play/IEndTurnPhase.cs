using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface IEndTurnPhase
    {
        string CurrentPlayerName { get; }
        IReadOnlyList<Territory> Territories { get; }
        IReadOnlyList<Player> Players { get; }
        void EndTurn();
    }
}