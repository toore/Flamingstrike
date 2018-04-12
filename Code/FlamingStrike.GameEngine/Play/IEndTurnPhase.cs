using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IEndTurnPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayer> Players { get; }
        void EndTurn();
    }
}