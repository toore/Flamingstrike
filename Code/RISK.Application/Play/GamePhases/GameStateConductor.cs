using System;
using System.Collections.Generic;
using RISK.Application.Extensions;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateConductor
    {
        void InitializeFirstPlayerTurn(GameData gameData);
        void ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft);
        void ContinueWithAttackPhase(GameData gameData, ConqueringAchievement conqueringAchievement);
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
        private readonly IGameStateFsm _gameStateFsm;

        public GameStateConductor(
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IGameDataFactory gameDataFactory,
            IGameStateFsm gameStateFsm)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _gameDataFactory = gameDataFactory;
            _gameStateFsm = gameStateFsm;
        }

        public void InitializeFirstPlayerTurn(GameData gameData)
        {
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);

            ContinueToDraftArmies(gameData, numberOfArmiesToDraft);
        }

        public void ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft)
        {
            _gameStateFsm.Set(_gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft));
        }

        public void ContinueWithAttackPhase(GameData gameData, ConqueringAchievement conqueringAchievement)
        {
            throw new NotImplementedException();
        }

        public void SendArmiesToOccupy(GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
        {
            _gameStateFsm.Set(_gameStateFactory.CreateSendArmiesToOccupyGameState(this, gameData, attackingRegion, occupiedRegion));
        }

        public void Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            _gameStateFsm.Set(_gameStateFactory.CreateFortifyState(this, gameData, sourceRegion, destinationRegion, numberOfArmiesToFortify));
        }

        public void PassTurnToNextPlayer(IGameState currentGameState)
        {
            var players = currentGameState.Players;
            var nextPlayer = NextPlayer(players, currentGameState.CurrentPlayer);
            var territories = currentGameState.Territories;
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, territories);

            var gameData = _gameDataFactory.Create(nextPlayer, players, territories, currentGameState.Deck);

            _gameStateFsm.Set(_gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft));
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }

        public void GameIsOver(GameData gameData)
        {
            _gameStateFsm.Set(_gameStateFactory.CreateGameOverGameState(gameData));
        }
    }
}