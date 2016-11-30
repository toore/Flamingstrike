using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGameStatus
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
    }

    public class GameStatus : IGameStatus
    {
        public GameStatus(IPlayer player, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas)
        {
            CurrentPlayer = player;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
        }

        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
    }
}