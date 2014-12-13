using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class AttackState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IBattleCalculator _battleCalculator;

        public AttackState(
            StateController stateController, 
            IInteractionStateFactory interactionStateFactory, 
            IBattleCalculator battleCalculator, 
            IPlayer player, 
            ITerritory selectedTerritory)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            _battleCalculator = battleCalculator;
            SelectedTerritory = selectedTerritory;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            return CanSelect(territory)
                   ||
                   CanAttack(territory);
        }

        public void OnClick(ITerritory territory)
        {
            if (CanSelect(territory))
            {
                Select(territory);
            }
            else if (CanAttack(territory))
            {
                Attack(territory);
            }
        }

        private bool CanSelect(ITerritory location)
        {
            return location == SelectedTerritory;
        }

        private void Select(ITerritory location)
        {
            if (CanSelect(location))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateSelectState(_stateController, Player);
            }
        }

        private bool CanAttack(ITerritory territory)
        {
            var isTerritoryOccupiedByEnemy = territory.Occupant != Player;
            var isBordering = SelectedTerritory.IsBordering(territory);
            var hasArmiesToAttackWith = SelectedTerritory.HasArmiesAvailableForAttack();

            var canAttack = isBordering
                            &&
                            isTerritoryOccupiedByEnemy
                            &&
                            hasArmiesToAttackWith;

            return canAttack;
        }

        private void Attack(ITerritory territory)
        {
            var canAttack = CanAttack(territory);

            if (!canAttack)
            {
                return;
            }

            _battleCalculator.Attack(SelectedTerritory, territory);

            if (HasPlayerOccupiedTerritory(territory))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, territory);
                _stateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
        }
    }
}