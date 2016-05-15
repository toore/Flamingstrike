using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public interface IGameDataFactory
    {
        GameData Create(IInGamePlayer currentPlayer, IReadOnlyList<IInGamePlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck);
    }

    public class GameDataFactory : IGameDataFactory
    {
        public GameData Create(IInGamePlayer currentPlayer, IReadOnlyList<IInGamePlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck)
        {
            return new GameData(currentPlayer, players, territories, deck);
        }
    }
}