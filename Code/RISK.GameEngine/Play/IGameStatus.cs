using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface IGameStatus
    {
        IPlayer Player { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
    }
}