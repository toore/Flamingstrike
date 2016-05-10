using System;
using System.Collections.Generic;
using RISK.Application.Extensions;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateConductor
    {
        IGameState InitializeFirstPlayerTurn(GameData gameData);
        IGameState ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft);
        IGameState ContinueWithAttackPhase(GameData gameData, ConqueringAchievement conqueringAchievement);
        IGameState SendArmiesToOccupy(GameData gameData, IRegion attackingRegion, IRegion occupiedRegion);
        IGameState Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
        IGameState PassTurnToNextPlayer(IGameState currentGameState);
        IGameState GameIsOver(GameData gameData);
    }

    public class GameStateConductor : IGameStateConductor
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IGameDataFactory _gameDataFactory;

        public GameStateConductor(IGameStateFactory gameStateFactory, IArmyDraftCalculator armyDraftCalculator, IGameDataFactory gameDataFactory)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _gameDataFactory = gameDataFactory;
        }

        public IGameState InitializeFirstPlayerTurn(GameData gameData)
        {
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);

            return ContinueToDraftArmies(gameData, numberOfArmiesToDraft);
        }

        public IGameState ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft)
        {
            return _gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft);
        }

        public IGameState ContinueWithAttackPhase(GameData gameData, ConqueringAchievement conqueringAchievement)
        {
            throw new NotImplementedException();
        }

        public IGameState SendArmiesToOccupy(GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return _gameStateFactory.CreateSendArmiesToOccupyGameState(this, gameData, attackingRegion, occupiedRegion);
        }

        public IGameState Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            return _gameStateFactory.CreateFortifyState(this, gameData, sourceRegion, destinationRegion, numberOfArmiesToFortify);
        }

        public IGameState PassTurnToNextPlayer(IGameState currentGameState)
        {
            var players = currentGameState.Players;
            var nextPlayer = NextPlayer(players, currentGameState.CurrentPlayer);
            var territories = currentGameState.Territories;
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, territories);

            var gameData = _gameDataFactory.Create(nextPlayer, players, territories, currentGameState.Deck);

            return _gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft);
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }

        public IGameState GameIsOver(GameData gameData)
        {
            return _gameStateFactory.CreateGameOverGameState(gameData);
        }
    }
}