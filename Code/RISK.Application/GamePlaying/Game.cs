using System.Collections.Generic;
using System.Linq;
using RISK.Application.Entities;
using RISK.Application.Extensions;

namespace RISK.Application.GamePlaying
{
    public class Game : IGame
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly ICardFactory _cardFactory;
        private readonly IList<IPlayer> _players;
        private IStateController _stateController;

        public Game(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory, IEnumerable<IPlayer> players, IWorldMap worldMap, ICardFactory cardFactory)
        {
            WorldMap = worldMap;
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _cardFactory = cardFactory;
            _players = players.ToList();

            MoveToNextPlayer();
        }

        public IWorldMap WorldMap { get; private set; }
        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get { return _stateController.CurrentState.SelectedTerritory; }}

        private void MoveToNextPlayer()
        {
            Player = _players.GetNextOrFirst(Player);

            _stateController = _stateControllerFactory.Create(Player);
            _stateController.SetInitialState();
        } 

        public void EndTurn()
        {
            if (_stateController.PlayerShouldReceiveCardWhenTurnEnds)
            {
                Player.AddCard(_cardFactory.Create());
            }

            MoveToNextPlayer();
        }

        public bool IsGameOver()
        {
            return WorldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
        }

        public void Fortify()
        {
            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, Player);
        }

        public void OnClick(ITerritory territory)
        {
            _stateController.CurrentState.OnClick(territory);
        }

        public bool CanClick(ITerritory territory)
        {
            return _stateController.CurrentState.CanClick(territory);
        }
    }
}