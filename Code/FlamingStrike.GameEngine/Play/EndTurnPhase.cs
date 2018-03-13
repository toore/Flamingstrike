using System.Collections.Generic;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IEndTurnPhase
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        void EndTurn();
    }

    public class EndTurnPhase : IEndTurnPhase
    {
        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        private readonly IEndTurnGameState _endTurnGameState;

        public EndTurnPhase(
            IPlayer currentPlayer,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayerGameData> playerGameDatas,
            IEndTurnGameState endTurnGameState)
        {
            CurrentPlayer = currentPlayer;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            _endTurnGameState = endTurnGameState;
        }

        public void EndTurn()
        {
            _endTurnGameState.EndTurn();
        }
    }
}