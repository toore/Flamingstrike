using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IRegion AttackingRegion { get; }
        IRegion OccupiedRegion { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }
}