using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        string CurrentPlayerName { get; }
        IReadOnlyList<Territory> Territories { get; }
        IReadOnlyList<Player> Players { get; }
        Region AttackingRegion { get; }
        Region OccupiedRegion { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }
}