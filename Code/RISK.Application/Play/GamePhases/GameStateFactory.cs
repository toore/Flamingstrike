using System;
using System.Collections.Generic;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateConductor
    {
        IGameState InitializeFirstPlayerTurn(GameData gameData);
        IGameState ContinueToDraftArmies(GameData gameData, int numberOfArmiesToDraft);
        IGameState ContinueWithAttackPhase(GameData gameData);
        IGameState SendInArmiesToOccupy(GameData gameData);
        IGameState Fortify(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
        IGameState PassTurnToNextPlayer(IGameState currentGameState);
    }

    public class GameStateConductor : IGameStateConductor
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;

        public GameStateConductor(IGameStateFactory gameStateFactory, IArmyDraftCalculator armyDraftCalculator)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
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

        public IGameState ContinueWithAttackPhase(GameData gameData)
        {
            throw new NotImplementedException();
        }

        public IGameState SendInArmiesToOccupy(GameData gameData)
        {
            return _gameStateFactory.CreateSendInArmiesToOccupyGameState(this, gameData);
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

            var gameData = new GameData(nextPlayer, players, territories, currentGameState.Deck);

            return _gameStateFactory.CreateDraftArmiesGameState(this, gameData, numberOfArmiesToDraft);
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }
    }

    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(IGameStateConductor gameStateConductor, GameData gameData, int numberOfArmiesToDraft);
        IGameState CreateAttackGameState(IGameStateConductor gameStateConductor, GameData gameData);
        IGameState CreateSendInArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData);
        IGameState CreateFortifyState(IGameStateConductor gameStateConductor, GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBattle _battle;
        private readonly IArmyDraftUpdater _armyDraftUpdater;

        public GameStateFactory(IBattle battle, IArmyDraftUpdater armyDraftUpdater)
        {
            _battle = battle;
            _armyDraftUpdater = armyDraftUpdater;
        }

        public IGameState CreateDraftArmiesGameState(IGameStateConductor gameStateConductor, GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(gameStateConductor, _armyDraftUpdater, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(IGameStateConductor gameStateConductor, GameData gameData)
        {
            return new AttackGameState(gameStateConductor, _battle, gameData);
        }

        public IGameState CreateSendInArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData)
        {
            return new SendInArmiesToOccupyGameState(gameStateConductor, gameData);
        }

        public IGameState CreateFortifyState(IGameStateConductor gameStateConductor, GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            return new FortifyGameState(gameStateConductor, gameData);
        }

        public IGameState CreateGameOverGameState(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}