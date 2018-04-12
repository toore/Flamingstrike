using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class GameData
    {
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public PlayerName CurrentPlayerName { get; }
        public IDeck Deck { get; }

        public GameData(IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, PlayerName currentPlayerName, IDeck deck)
        {
            Territories = territories;
            Players = players;
            CurrentPlayerName = currentPlayerName;
            Deck = deck;
        }
    }

    public static class GameDataExtensions
    {
        public static IPlayer GetCurrentPlayer(this GameData gameData)
        {
            return gameData.Players.Single(x => x.PlayerName == gameData.CurrentPlayerName);
        }
    }

    public interface IGamePhaseConductor
    {
        void ContinueToDraftArmies(int numberOfArmiesToDraft, GameData gameData);
        void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement, GameData gameData);
        void SendArmiesToOccupy(Region sourceRegion, Region destinationRegion, GameData gameData);
        void WaitForTurnToEnd(GameData gameData);
        void PassTurnToNextPlayer(GameData gameData);
        void PlayerIsTheWinner(PlayerName winner);
    }

    public class Game : IGamePhaseConductor
    {
        private readonly IGameObserver _gameObserver;
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;

        public Game(
            IGameObserver gameObserver,
            IGamePhaseFactory gamePhaseFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory)
        {
            _gameObserver = gameObserver;
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
            _gamePhaseFactory = gamePhaseFactory;
        }

        public void Run(IReadOnlyList<ITerritory> territories, IReadOnlyList<PlayerName> players)
        {
            var playerGameDatas = players.Select(player => new Player(player, new List<ICard>())).ToList();
            var currentPlayer = players.First();

            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(currentPlayer, territories);

            var gameData = new GameData(territories, playerGameDatas, currentPlayer, _deckFactory.Create());
            ContinueToDraftArmies(numberOfArmiesToDraft, gameData);
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft, GameData gameData)
        {
            var draftArmiesPhase = _gamePhaseFactory.CreateDraftArmiesPhase(
                this,
                gameData.CurrentPlayerName,
                gameData.Territories,
                gameData.Players,
                gameData.Deck,
                numberOfArmiesToDraft);

            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement, GameData gameData)
        {
            var attackPhase = _gamePhaseFactory.CreateAttackPhase(
                this,
                gameData.CurrentPlayerName,
                gameData.Territories,
                gameData.Players,
                gameData.Deck,
                conqueringAchievement);

            _gameObserver.Attack(attackPhase);
        }

        public void WaitForTurnToEnd(GameData gameData)
        {
            var endTurnPhase = _gamePhaseFactory.CreateEndTurnPhase(
                this,
                gameData.CurrentPlayerName,
                gameData.Territories,
                gameData.Players,
                gameData.Deck);

            _gameObserver.EndTurn(endTurnPhase);
        }

        public void SendArmiesToOccupy(Region attackingRegion, Region occupiedRegion, GameData gameData)
        {
            var sendArmiesToOccupyPhase = _gamePhaseFactory.CreateSendArmiesToOccupyPhase(
                this,
                gameData.CurrentPlayerName,
                gameData.Territories,
                gameData.Players,
                gameData.Deck,
                attackingRegion,
                occupiedRegion);

            _gameObserver.SendArmiesToOccupy(sendArmiesToOccupyPhase);
        }

        public void PassTurnToNextPlayer(GameData gameData)
        {
            var currentPlayerIndex = gameData.Players.Select(x => x.PlayerName)
                .ToList()
                .IndexOf(gameData.CurrentPlayerName);
            var nextPlayer = gameData.Players[(currentPlayerIndex + 1) % gameData.Players.Count].PlayerName;

            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(nextPlayer, gameData.Territories);
            var updatedGameData = new GameData(gameData.Territories, gameData.Players, nextPlayer, gameData.Deck);

            ContinueToDraftArmies(numberOfArmiesToDraft, updatedGameData);
        }

        public void PlayerIsTheWinner(PlayerName winner)
        {
            var gameOverState = _gamePhaseFactory.CreateGameOverState(winner);

            _gameObserver.GameOver(gameOverState);
        }
    }
}