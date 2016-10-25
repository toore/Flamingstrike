using System.Collections.Generic;

namespace RISK.GameEngine.Play.GameStates
{
    public interface IEndTurnGameState
    {
        IPlayer Player { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> Players { get; }
        void EndTurn();
    }

    public class EndTurnGameState : IEndTurnGameState
    {
        private readonly GameData _gameData;
        private readonly IGamePhaseConductor _gamePhaseConductor;

        public EndTurnGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor)
        {
            _gameData = gameData;
            _gamePhaseConductor = gamePhaseConductor;
        }

        public IPlayer Player => _gameData.CurrentPlayer;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IReadOnlyList<IPlayerGameData> Players => _gameData.PlayerGameDatas;

        public void EndTurn()
        {
            _gamePhaseConductor.PassTurnToNextPlayer(_gameData);
        }
    }
}