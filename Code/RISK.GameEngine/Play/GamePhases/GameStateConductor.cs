using System;
using System.Collections.Generic;
using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public interface IGameStateConductor
    {
        void CurrentPlayerStartsNewTurn(GameData gameData);
        void ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft);
        void ContinueWithAttackPhase(GameData gameData, TurnConqueringAchievement turnConqueringAchievement);
        void SendArmiesToOccupy(GameData gameData, IRegion attackingRegion, IRegion occupiedRegion);
        void Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
        void PassTurnToNextPlayer(IGameState currentGameState);
        void GameIsOver(GameData gameData);
    }

    public class GameStateConductor : IGameStateConductor
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IGameContext _gameContext;

        public GameStateConductor(
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IGameDataFactory gameDataFactory,
            IGameContext gameContext)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _gameDataFactory = gameDataFactory;
            _gameContext = gameContext;
        }

        public void CurrentPlayerStartsNewTurn(GameData gameData)
        {
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);

            ContinueToDraftArmies(gameData, numberOfArmiesToDraft);
        }

        public void ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft)
        {
            var draftArmiesGameState = _gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft);

            _gameContext.Set(draftArmiesGameState);
        }

        public void ContinueWithAttackPhase(GameData gameData, TurnConqueringAchievement turnConqueringAchievement)
        {
            var attackGameState = _gameStateFactory.CreateAttackGameState(this, gameData, turnConqueringAchievement);

            _gameContext.Set(attackGameState);
        }

        public void SendArmiesToOccupy(GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
        {
            var sendArmiesToOccupyGameState = _gameStateFactory.CreateSendArmiesToOccupyGameState(this, gameData, attackingRegion, occupiedRegion);

            _gameContext.Set(sendArmiesToOccupyGameState);
        }

        public void Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            var fortifyState = _gameStateFactory.CreateFortifyState(this, gameData, sourceRegion, destinationRegion, numberOfArmiesToFortify);

            _gameContext.Set(fortifyState);
        }

        public void PassTurnToNextPlayer(IGameState currentGameState)
        {
            var players = currentGameState.Players;
            var nextPlayer = NextPlayer(players, currentGameState.CurrentPlayer);
            var territories = currentGameState.Territories;

            var gameData = _gameDataFactory.Create(nextPlayer, players, territories, currentGameState.Deck);

            CurrentPlayerStartsNewTurn(gameData);
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }

        public void GameIsOver(GameData gameData)
        {
            var gameOverGameState = _gameStateFactory.CreateGameOverGameState(gameData);

            _gameContext.Set(gameOverGameState);
        }
    }
}