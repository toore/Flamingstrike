using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayer> Players { get; }
        Region AttackingRegion { get; }
        Region OccupiedRegion { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }
}