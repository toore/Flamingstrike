using System.Collections.Generic;

namespace RISK.GameEngine.Play.GameStates
{
    public interface ISendArmiesToOccupyGameState
    {
        IPlayer Player { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IRegion AttackingRegion { get; }
        IRegion OccupiedRegion { get; }
        IReadOnlyList<IPlayerGameData> Players { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }

    public class SendArmiesToOccupyGameState : ISendArmiesToOccupyGameState
    {
        private readonly GameData _gameData;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;

        public SendArmiesToOccupyGameState(
            GameData gameData,
            IGamePhaseConductor gamePhaseConductor,
            ITerritoryOccupier territoryOccupier,
            IRegion attackingRegion,
            IRegion occupiedRegion)
        {
            AttackingRegion = attackingRegion;
            OccupiedRegion = occupiedRegion;
            _gameData = gameData;
            _gamePhaseConductor = gamePhaseConductor;
            _territoryOccupier = territoryOccupier;
        }

        public IPlayer Player => _gameData.CurrentPlayer;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IRegion AttackingRegion { get; }
        public IRegion OccupiedRegion { get; }
        public IReadOnlyList<IPlayerGameData> Players => _gameData.Players;

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            var updatedTerritories = _territoryOccupier.SendInAdditionalArmiesToOccupy(_gameData.Territories, AttackingRegion, OccupiedRegion, numberOfArmies);
            var updatedGameData = new GameData(updatedTerritories, _gameData.Players, _gameData.CurrentPlayer, _gameData.Cards);

            _gamePhaseConductor.ContinueWithAttackPhase(
                TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                updatedGameData);
        }
    }
}