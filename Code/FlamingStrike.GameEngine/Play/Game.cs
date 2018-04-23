using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGamePhaseConductor
    {
        void ContinueToDraftArmies(int numberOfArmiesToDraft);
        void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement);
        void SendArmiesToOccupy(Region sourceRegion, Region destinationRegion);
        void WaitForTurnToEnd();
        void PassTurnToNextPlayer();
        void PlayerIsTheWinner(PlayerName winner);
    }

    public class Game : IGamePhaseConductor
    {
        private readonly IGameObserver _gameObserver;
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly List<Territory> _territories;
        private readonly List<Player> _players;
        private readonly IDeck _deck;
        private int _currentPlayerIndex;

        public Game(
            IGameObserver gameObserver,
            IGamePhaseFactory gamePhaseFactory,
            IArmyDraftCalculator armyDraftCalculator,
            List<Territory> territories,
            List<Player> players,
            IDeck deck)
        {
            _gameObserver = gameObserver;
            _armyDraftCalculator = armyDraftCalculator;
            _territories = territories;
            _players = players;
            _deck = deck;
            _gamePhaseFactory = gamePhaseFactory;

            _currentPlayerIndex = 0;
        }

        public void Start()
        {
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(GetCurrentPlayerName(), _territories);

            ContinueToDraftArmies(numberOfArmiesToDraft);
        }

        private PlayerName GetCurrentPlayerName()
        {
            return _players[_currentPlayerIndex].Name;
        }

        public void ContinueToDraftArmies(int numberOfArmiesToDraft)
        {
            var draftArmiesPhase = _gamePhaseFactory.CreateDraftArmiesPhase(
                this,
                GetCurrentPlayerName(),
                _territories,
                _players,
                _deck,
                numberOfArmiesToDraft);

            _gameObserver.DraftArmies(draftArmiesPhase);
        }

        public void ContinueWithAttackPhase(ConqueringAchievement conqueringAchievement)
        {
            var attackPhase = _gamePhaseFactory.CreateAttackPhase(
                this,
                GetCurrentPlayerName(),
                _territories,
                _players,
                _deck,
                conqueringAchievement);

            _gameObserver.Attack(attackPhase);
        }

        public void WaitForTurnToEnd()
        {
            var endTurnPhase = _gamePhaseFactory.CreateEndTurnPhase(
                this,
                GetCurrentPlayerName(),
                _territories,
                _players,
                _deck);

            _gameObserver.EndTurn(endTurnPhase);
        }

        public void SendArmiesToOccupy(Region attackingRegion, Region occupiedRegion)
        {
            var sendArmiesToOccupyPhase = _gamePhaseFactory.CreateSendArmiesToOccupyPhase(
                this,
                GetCurrentPlayerName(),
                _territories,
                _players,
                _deck,
                attackingRegion,
                occupiedRegion);

            _gameObserver.SendArmiesToOccupy(sendArmiesToOccupyPhase);
        }

        public void PassTurnToNextPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(GetCurrentPlayerName(), _territories);

            ContinueToDraftArmies(numberOfArmiesToDraft);
        }

        public void PlayerIsTheWinner(PlayerName winner)
        {
            var gameOverState = _gamePhaseFactory.CreateGameOverState(winner);

            _gameObserver.GameOver(gameOverState);
        }
    }
}