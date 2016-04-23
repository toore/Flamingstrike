using System.Collections.Generic;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft);
        IGameState CreateAttackGameState(GameData gameData);
        IGameState CreateSendInArmiesToOccupyGameState(GameData gameData);
        IGameState CreateFortifyState(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
        //Rename PassTurnToNextPlayer
        IGameState CreateNextTurnGameState(IGameState currentGameState);

        //IGameState MoveOnToAttackPhase(GameData gameData);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBattle _battle;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IArmyDraftUpdater _armyDraftUpdater;

        public GameStateFactory(IBattle battle, IArmyDraftCalculator armyDraftCalculator, IArmyDraftUpdater armyDraftUpdater)
        {
            _battle = battle;
            _armyDraftCalculator = armyDraftCalculator;
            _armyDraftUpdater = armyDraftUpdater;
        }

        public IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(this, _armyDraftUpdater, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(GameData gameData)
        {
            return new AttackGameState(this, _battle, gameData);
        }

        public IGameState CreateSendInArmiesToOccupyGameState(GameData gameData)
        {
            return new SendInArmiesToOccupyGameState(this, gameData);
        }

        public IGameState CreateFortifyState(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            return new FortifyGameState(this, gameData);
        }

        public IGameState CreateNextTurnGameState(IGameState currentGameState)
        {
            var players = currentGameState.Players;
            var nextPlayer = NextPlayer(players, currentGameState.CurrentPlayer);
            var territories = currentGameState.Territories;
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, territories);

            var gameData = new GameData(nextPlayer, players, territories, currentGameState.Deck);

            return CreateDraftArmiesGameState(gameData, numberOfArmiesToDraft);
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }

        public IGameState CreateGameOverGameState(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}