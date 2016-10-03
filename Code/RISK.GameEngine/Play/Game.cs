using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IGame
    {
        IPlayer CurrentPlayer { get; }
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
        private readonly IReadOnlyList<IPlayer> _players;
        private readonly ITerritoriesContext _territoriesContext = new TerritoriesContext();

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
            _players = players;

            InitializeNewGame(territories);
        }

        public IPlayer CurrentPlayer { get; private set; }

        public IReadOnlyList<ITerritory> Territories => _territoriesContext.Territories;

        private void InitializeNewGame(IReadOnlyList<ITerritory> territories)
        {
            _territoriesContext.Set(territories);

            CurrentPlayer = _players.First();

            NewGame(this);

            PlayerStartsNewTurn(CurrentPlayer);
        }

        private void NewGame(IGame game)
        {
            _gameObserver.NewGame(game);
        }

        private void PlayerStartsNewTurn(IPlayer player)
        {
            CurrentPlayer = player;
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(player, _territoriesContext.Territories);

            ContinueToDraftArmies(numberOfArmiesToDraft);
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft)
        {
            var draftArmiesGamePhase = _gameStateFactory.CreateDraftArmiesGameState(CurrentPlayer, _territoriesContext, this, numberOfArmiesToDraft);

            var regionsAllowedToDraftArmies = _territoriesContext.Territories
                .Where(x => draftArmiesGamePhase.CanPlaceDraftArmies(x.Region))
                .Select(x => x.Region).ToList();

            var draftArmiesPhase = new DraftArmiesPhase(draftArmiesGamePhase, CurrentPlayer, _territoriesContext.Territories, numberOfArmiesToDraft, regionsAllowedToDraftArmies);
            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(TurnConqueringAchievement turnConqueringAchievement)
        {
            var attackPhaseGameState = _gameStateFactory.CreateAttackPhaseGameState(CurrentPlayer, _players, _deck, _territoriesContext, this, turnConqueringAchievement);

            var regionsThatCanBeSourceForAttackOrFortification = _territoriesContext.Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(x.Region))
                .Select(x => x.Region).ToList();

            _gameObserver.Attack(new AttackPhase(attackPhaseGameState, regionsThatCanBeSourceForAttackOrFortification));
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return CurrentPlayer == _territoriesContext.Territories.Single(x => x.Region == region).Player;
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
            var nextPlayer = GetNextPlayer(_players, CurrentPlayer);

            PlayerStartsNewTurn(nextPlayer);
        }

        private static IPlayer GetNextPlayer(IEnumerable<IPlayer> players, IPlayer currentPlayer)
        {
            var sequence = players.ToCircularBuffer();
            while (sequence.Next() != currentPlayer) {}

            return sequence.Next();
        }

        public void PlayerIsTheWinner(IPlayer winner)
        {
            var gameOverGameState = _gameStateFactory.CreateGameOverGameState(winner);

            _gameObserver.GameOver(new GameIsOver(gameOverGameState, winner));
        }
    }
}