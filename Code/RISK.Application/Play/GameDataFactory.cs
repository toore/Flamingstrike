using System.Collections.Generic;

namespace RISK.Application.Play
{
    public interface IGameDataFactory
    {
        GameData Create(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck);
    }

    public class GameDataFactory : IGameDataFactory
    {
        public GameData Create(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck)
        {
            return new GameData(currentPlayer, players, territories, deck);
        }
    }
}