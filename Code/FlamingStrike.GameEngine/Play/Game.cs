using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;

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
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;

        public Game(
            IGameObserver gameObserver,
            IGamePhaseFactory gamePhaseFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players)
        {
            _gameObserver = gameObserver;
            _armyDraftCalculator = armyDraftCalculator;
            _gamePhaseFactory = gamePhaseFactory;

            var playerGameDatas = players.Select(player => new PlayerGameData(player, new List<ICard>())).ToList();
            var currentPlayer = players.First();
            var gameData = new GameData(territories, playerGameDatas, currentPlayer, deckFactory.Create());
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(currentPlayer, territories);

            ContinueToDraftArmies(numberOfArmiesToDraft, gameData);
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft, GameData gameData)
        {
            var draftArmiesPhase = _gamePhaseFactory.CreateDraftArmiesPhase(
                this,
                gameData.CurrentPlayer,
                gameData.Territories,
                gameData.PlayerGameDatas,
                gameData.Deck,
                numberOfArmiesToDraft);

            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement, GameData gameData)
        {
            var attackPhase = _gamePhaseFactory.CreateAttackPhase(
                this,
                gameData.CurrentPlayer,
                gameData.Territories,
                gameData.PlayerGameDatas,
                gameData.Deck,
                conqueringAchievement);

            _gameObserver.Attack(attackPhase);
        }

        public void WaitForTurnToEnd(GameData gameData)
        {
            var endTurnPhase = _gamePhaseFactory.CreateEndTurnPhase(
                this,
                gameData.CurrentPlayer,
                gameData.Territories,
                gameData.PlayerGameDatas,
                gameData.Deck);

            _gameObserver.EndTurn(endTurnPhase);
        }

        public void SendArmiesToOccupy(IRegion attackingRegion, IRegion occupiedRegion, GameData gameData)
        {
            var sendArmiesToOccupyPhase = _gamePhaseFactory.CreateSendArmiesToOccupyPhase(
                this,
                gameData.CurrentPlayer,
                gameData.Territories,
                gameData.PlayerGameDatas,
                gameData.Deck,
                attackingRegion,
                occupiedRegion);

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
            var gameOverState = _gamePhaseFactory.CreateGameOverState(winner);

            _gameObserver.GameOver(gameOverState);
        }
    }
}