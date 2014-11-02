using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class AttackState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IBattleCalculator _battleCalculator;
        private readonly IWorldMap _worldMap;

        public AttackState(
            StateController stateController, 
            IInteractionStateFactory interactionStateFactory, 
            IBattleCalculator battleCalculator, 
            IPlayer player, 
            IWorldMap worldMap, 
            ITerritory selectedTerritory)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            _battleCalculator = battleCalculator;
            _worldMap = worldMap;
            SelectedTerritory = selectedTerritory;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ILocation location)
        {
            return CanSelect(location)
                   ||
                   CanAttack(location);
        }

        public void OnClick(ILocation location)
        {
            if (CanSelect(location))
            {
                Select(location);
            }
            else if (CanAttack(location))
            {
                Attack(location);
            }
        }

        private bool CanSelect(ILocation location)
        {
            return location == SelectedTerritory.Location;
        }

        private void Select(ILocation location)
        {
            if (CanSelect(location))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateSelectState(_stateController, Player, _worldMap);
            }
        }

        private bool CanAttack(ILocation location)
        {
            var territoryToAttack = _worldMap.GetTerritory(location);
            var isTerritoryOccupiedByEnemy = territoryToAttack.Occupant != Player;
            var isBordering = SelectedTerritory.Location.IsBordering(territoryToAttack.Location);
            var hasArmiesToAttackWith = SelectedTerritory.HasArmiesAvailableForAttack();

            var canAttack = isBordering
                            &&
                            isTerritoryOccupiedByEnemy
                            &&
                            hasArmiesToAttackWith;

            return canAttack;
        }

        private void Attack(ILocation location)
        {
            var canAttack = CanAttack(location);

            if (!canAttack)
            {
                return;
            }

            var territory = _worldMap.GetTerritory(location);
            Attack(territory);
        }

        private void Attack(ITerritory territory)
        {
            _battleCalculator.Attack(SelectedTerritory, territory);

            if (HasPlayerOccupiedTerritory(territory))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, _worldMap, territory);
                _stateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
        }

        //public bool CanFortify(ILocation location)
        //{
        //    return SelectedTerritory.Location.IsBordering(location) 
        //        && 
        //        _worldMap.GetTerritory(location).Occupant == Player;
        //}

        //public void Fortify(ILocation location, int armies)
        //{
        //    _worldMap.GetTerritory(location).Armies += armies;
        //    SelectedTerritory.Armies -= armies;

        //    _stateController.CurrentState = _interactionStateFactory.CreateFortifiedState(Player, _worldMap);
        //}
    }
}