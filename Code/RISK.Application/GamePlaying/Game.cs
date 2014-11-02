using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace RISK.Domain.GamePlaying
{
    public class Game : IGame
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly ICardFactory _cardFactory;
        private readonly IList<IPlayer> _players;
        private IPlayer _currentPlayer;
        private StateController _stateController;

        public Game(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory, IPlayers players, Territories territories, ICardFactory cardFactory)
        {
            Territories = territories;
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _cardFactory = cardFactory;
            _players = players.GetAll().ToList();

            MoveToNextPlayer();
        }

        public Territories Territories { get; private set; }
        public IInteractionState CurrentInteractionState { get { return _stateController.CurrentState; } }

        private void MoveToNextPlayer()
        {
            _currentPlayer = _players.GetNextOrFirst(_currentPlayer);

            _stateController = _stateControllerFactory.Create();
            _stateController.CurrentState = _interactionStateFactory.CreateSelectState(_stateController, _currentPlayer);
        }

        public void EndTurn()
        {
            if (_stateController.PlayerShouldReceiveCardWhenTurnEnds)
            {
                _currentPlayer.AddCard(_cardFactory.Create());
            }

            MoveToNextPlayer();
        }

        public bool IsGameOver()
        {
            return Territories.GetAllPlayersOccupyingTerritories().Count() == 1;
        }
    }
}