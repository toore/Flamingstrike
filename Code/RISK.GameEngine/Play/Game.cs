using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IGame
    {
        IPlayerGameData CurrentPlayerGameData { get; }
        IEnumerable<IPlayerGameData> PlayerGameDatas { get; }
        IReadOnlyList<ITerritory> Territories { get; }
    }

    public interface IGamePhaseConductor
    {
        void ContinueToDraftArmies(int numberOfArmiesToDraft);
        void ContinueWithAttackPhase(TurnConqueringAchievement turnConqueringAchievement);
        void SendArmiesToOccupy(IRegion sourceRegion, IRegion destinationRegion);
        void WaitForTurnToEnd();
        void PassTurnToNextPlayer();
        void PlayerIsTheWinner(IPlayer winner);
    }

    public class Game : IGame, IGamePhaseConductor
    {
        private readonly IGameObserver _gameObserver;
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeck _deck;
        private readonly IReadOnlyList<PlayerGameData> _playerGameDatas;
        private readonly ITerritoriesContext _territoriesContext = new TerritoriesContext();
        private PlayerGameData _currentPlayerGameData;
        private readonly CircularBuffer<PlayerGameData> _playerGameDatasCircularBuffer;

        public Game(
            IGameObserver gameObserver,
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeck deck,
            IReadOnlyList<IPlayer> players,
            IReadOnlyList<ITerritory> territories)
        {
            _gameObserver = gameObserver;
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _deck = deck;
            _playerGameDatas = players.Select(player => new PlayerGameData(player)).ToList();
            _playerGameDatasCircularBuffer = _playerGameDatas.ToCircularBuffer();

            InitializeNewGame(territories);
        }

        public IPlayerGameData CurrentPlayerGameData => _currentPlayerGameData;
        public IEnumerable<IPlayerGameData> PlayerGameDatas => _playerGameDatas;

        public IReadOnlyList<ITerritory> Territories => _territoriesContext.Territories;

        private void InitializeNewGame(IReadOnlyList<ITerritory> territories)
        {
            _territoriesContext.Set(territories);

            _currentPlayerGameData = _playerGameDatasCircularBuffer.Next();

            NewGame(this);

            PlayerStartsNewTurn(_currentPlayerGameData);
        }

        private void NewGame(IGame game)
        {
            _gameObserver.NewGame(game);
        }

        private void PlayerStartsNewTurn(PlayerGameData playerGameData)
        {
            _currentPlayerGameData = playerGameData;
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(_currentPlayerGameData.Player, _territoriesContext.Territories);

            ContinueToDraftArmies(numberOfArmiesToDraft);
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft)
        {
            var draftArmiesGamePhase = _gameStateFactory.CreateDraftArmiesGameState(_currentPlayerGameData.Player, _territoriesContext, this, numberOfArmiesToDraft);

            var regionsAllowedToDraftArmies = _territoriesContext.Territories
                .Where(x => draftArmiesGamePhase.CanPlaceDraftArmies(x.Region))
                .Select(x => x.Region).ToList();

            var draftArmiesPhase = new DraftArmiesPhase(draftArmiesGamePhase, _currentPlayerGameData.Player, _territoriesContext.Territories, numberOfArmiesToDraft, regionsAllowedToDraftArmies);
            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(TurnConqueringAchievement turnConqueringAchievement)
        {
            var attackPhaseGameState = _gameStateFactory.CreateAttackPhaseGameState(_currentPlayerGameData, _playerGameDatas.ToList(), _deck, _territoriesContext, this, turnConqueringAchievement);

            var regionsThatCanBeSourceForAttackOrFortification = _territoriesContext.Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(x.Region))
                .Select(x => x.Region).ToList();

            _gameObserver.Attack(new AttackPhase(attackPhaseGameState, regionsThatCanBeSourceForAttackOrFortification));
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _currentPlayerGameData.Player == _territoriesContext.Territories.Single(x => x.Region == region).Player;
        }

        public void WaitForTurnToEnd()
        {
            var endTurnGameState = _gameStateFactory.CreateEndTurnGameState(this);

            _gameObserver.EndTurn(new EndTurnPhase(endTurnGameState));
        }

        public void SendArmiesToOccupy(IRegion attackingRegion, IRegion occupiedRegion)
        {
            var sendArmiesToOccupyGameState = _gameStateFactory.CreateSendArmiesToOccupyGameState(_territoriesContext, this, attackingRegion, occupiedRegion);

            _gameObserver.SendArmiesToOccupy(new SendArmiesToOccupyPhase(sendArmiesToOccupyGameState, attackingRegion, occupiedRegion));
        }

        public void PassTurnToNextPlayer()
        {
            var nextPlayer = _playerGameDatasCircularBuffer.Next();

            PlayerStartsNewTurn(nextPlayer);
        }

        public void PlayerIsTheWinner(IPlayer winner)
        {
            var gameOverGameState = _gameStateFactory.CreateGameOverGameState(winner);

            _gameObserver.GameOver(new GameIsOver(gameOverGameState, winner));
        }
    }
}