using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IGame { }

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
        void ContinueWithAttackPhase(TurnConqueringAchievement turnConqueringAchievement, GameData gameData);
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
            var draftArmiesGamePhase = _gameStateFactory.CreateDraftArmiesGameState(gameData, this, numberOfArmiesToDraft);

            var regionsAllowedToDraftArmies = gameData.Territories
                .Where(x => draftArmiesGamePhase.CanPlaceDraftArmies(x.Region))
                .Select(x => x.Region).ToList();

            var draftArmiesPhase = new DraftArmiesPhase(draftArmiesGamePhase, regionsAllowedToDraftArmies);
            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(TurnConqueringAchievement turnConqueringAchievement, GameData gameData)
        {
            var attackPhaseGameState = _gameStateFactory.CreateAttackPhaseGameState(gameData, this, turnConqueringAchievement);

            var regionsThatCanBeSourceForAttackOrFortification = gameData.Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(gameData, x.Region))
                .Select(x => x.Region).ToList();

            _gameObserver.Attack(new AttackPhase(attackPhaseGameState, regionsThatCanBeSourceForAttackOrFortification));
        }

        private static bool IsCurrentPlayerOccupyingRegion(GameData gameData, IRegion region)
        {
            return gameData.CurrentPlayer == gameData.Territories.Single(x => x.Region == region).Player;
        }

        public void WaitForTurnToEnd(GameData gameData)
        {
            var endTurnGameState = _gameStateFactory.CreateEndTurnGameState(gameData, this);

            _gameObserver.EndTurn(new EndTurnPhase(endTurnGameState));
        }

        public void SendArmiesToOccupy(IRegion attackingRegion, IRegion occupiedRegion, GameData gameData)
        {
            var sendArmiesToOccupyGameState = _gameStateFactory.CreateSendArmiesToOccupyGameState(gameData, this, attackingRegion, occupiedRegion);

            _gameObserver.SendArmiesToOccupy(new SendArmiesToOccupyPhase(sendArmiesToOccupyGameState));
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