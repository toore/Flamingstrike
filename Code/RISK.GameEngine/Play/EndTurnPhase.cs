using System.Collections.Generic;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IEndTurnPhase : IGameStatus
    {
        void EndTurn();
    }

    public class EndTurnPhase : IEndTurnPhase
    {
        private readonly IEndTurnGameState _endTurnGameState;

        public EndTurnPhase(IEndTurnGameState endTurnGameState)
        {
            _endTurnGameState = endTurnGameState;
        }

        public IPlayer Player => _endTurnGameState.Player;
        public IReadOnlyList<ITerritory> Territories => _endTurnGameState.Territories;
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas => _endTurnGameState.Players;

        public void EndTurn()
        {
            _endTurnGameState.EndTurn();
        }
    }
}