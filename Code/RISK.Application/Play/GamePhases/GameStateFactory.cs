using System.Collections.Generic;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateNextTurnGameState(GameData gameData);
        IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft);
        IGameState CreateAttackGameState(GameData gameData);
        IGameState CreateSendInArmiesToOccupyGameState(GameData gameData);
        IGameState CreateFortifyState(GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
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

        public IGameState CreateNextTurnGameState(GameData gameData)
        {
            var nextPlayer = NextPlayer(gameData.Players, gameData.CurrentPlayer);
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, gameData.Territories);

            var nextPlayerGameData = new GameData(
                nextPlayer, 
                gameData.Players, 
                gameData.Territories, 
                gameData.Deck);

            return CreateDraftArmiesGameState(nextPlayerGameData, numberOfArmiesToDraft);
        }

        private static IPlayer NextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToSequence();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
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

        public IGameState CreateGameOverGameState(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}