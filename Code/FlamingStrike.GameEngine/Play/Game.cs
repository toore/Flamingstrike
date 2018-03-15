using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGame {}

    public class GameData
    {
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public IPlayer CurrentPlayer { get; }
        public IDeck Deck { get; }

        public GameData(IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IPlayer currentPlayer, IDeck deck)
        {
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            CurrentPlayer = currentPlayer;
            Deck = deck;
        }
    }

    public static class GameDataExtensions
    {
        public static IPlayerGameData GetCurrentPlayerGameData(this GameData gameData)
        {
            return gameData.PlayerGameDatas.Single(x => x.Player == gameData.CurrentPlayer);
        }
    }

    public interface IGamePhaseConductor
    {
        void ContinueToDraftArmies(int numberOfArmiesToDraft, GameData gameData);
        void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement, GameData gameData);
        void SendArmiesToOccupy(IRegion sourceRegion, IRegion destinationRegion, GameData gameData);
        void WaitForTurnToEnd(GameData gameData);
        void PassTurnToNextPlayer(GameData gameData);
        void PlayerIsTheWinner(IPlayer winner);
    }

    public class Game : IGame, IGamePhaseConductor
    {
        private readonly IGameObserver _gameObserver;
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;

        public Game(
            IGameObserver gameObserver,
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players)
            IArmyDrafter armyDrafter,
        {
            _gameObserver = gameObserver;
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;

            var playerGameDatas = players.Select(player => new PlayerGameData(player, new List<ICard>())).ToList();
            var currentPlayer = players.First();
            var gameData = new GameData(territories, playerGameDatas, currentPlayer, deckFactory.Create());
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(currentPlayer, territories);
            
            ContinueToDraftArmies(numberOfArmiesToDraft, gameData);
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft, GameData gameData)
        {
            var draftArmiesPhase = new DraftArmiesPhase(
                this,
                gameData.CurrentPlayer,
                gameData.Territories,
                gameData.PlayerGameDatas,
                gameData.Deck,
                numberOfArmiesToDraft,
                _armyDrafter);

            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement, GameData gameData)
        {
            var attackPhaseGameState = _gameStateFactory.CreateAttackPhaseGameState(gameData, this, conqueringAchievement);

            var regionsThatCanBeSourceForAttackOrFortification = gameData.Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(gameData, x.Region))
                .Select(x => x.Region).ToList();

            var attackPhase = new AttackPhase(attackPhaseGameState.Player, attackPhaseGameState.Territories, attackPhaseGameState.Players, attackPhaseGameState, regionsThatCanBeSourceForAttackOrFortification);
            _gameObserver.Attack(attackPhase);
        }

        private static bool IsCurrentPlayerOccupyingRegion(GameData gameData, IRegion region)
        {
            return gameData.CurrentPlayer == gameData.Territories.Single(x => x.Region == region).Player;
        }

        public void WaitForTurnToEnd(GameData gameData)
        {
            var endTurnGameState = _gameStateFactory.CreateEndTurnGameState(gameData, this);

            var endTurnPhase = new EndTurnPhase(endTurnGameState.Player, endTurnGameState.Territories, endTurnGameState.Players, endTurnGameState);
            _gameObserver.EndTurn(endTurnPhase);
        }

        public void SendArmiesToOccupy(IRegion attackingRegion, IRegion occupiedRegion, GameData gameData)
        {
            var sendArmiesToOccupyGameState = _gameStateFactory.CreateSendArmiesToOccupyGameState(gameData, this, attackingRegion, occupiedRegion);

            var sendArmiesToOccupyPhase = new SendArmiesToOccupyPhase(sendArmiesToOccupyGameState.Player, sendArmiesToOccupyGameState.Territories, sendArmiesToOccupyGameState.Players, sendArmiesToOccupyGameState);
            _gameObserver.SendArmiesToOccupy(sendArmiesToOccupyPhase);
        }

        public void PassTurnToNextPlayer(GameData gameData)
        {
            var nextPlayer = gameData.PlayerGameDatas.Select(x => x.Player).ToList()
                .GetNext(gameData.CurrentPlayer);

            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, gameData.Territories);
            var updatedGameData = new GameData(gameData.Territories, gameData.PlayerGameDatas, nextPlayer, gameData.Deck);

            ContinueToDraftArmies(numberOfArmiesToDraft, updatedGameData);
        }

        public void PlayerIsTheWinner(IPlayer winner)
        {
            var gameOverGameState = _gameStateFactory.CreateGameOverGameState(winner);

            _gameObserver.GameOver(new GameOverState(gameOverGameState));
        }
    }
}