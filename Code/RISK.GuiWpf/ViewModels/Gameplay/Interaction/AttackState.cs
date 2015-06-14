using System;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class AttackState : IInteractionState
    {
        private readonly IStateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public AttackState(
            IStateController stateController, 
            IInteractionStateFactory interactionStateFactory, 
            IPlayerId playerId, 
            ITerritory selectedTerritory)
        {
            PlayerId = playerId;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            SelectedTerritory = selectedTerritory;
        }

        public IPlayerId PlayerId { get; }
        public ITerritory SelectedTerritory { get; }

        public bool CanClick(ITerritory territory)
        {
            return CanSelect(territory)
                   ||
                   CanAttack(territory);
        }

        public void OnClick(ITerritory territory)
        {
            if (!CanSelect(territory) && !CanAttack(territory))
            {
                throw new InvalidOperationException();                
            }

            if (CanSelect(territory))
            {
                Select(territory);
            }
            else if (CanAttack(territory))
            {
                Attack(territory);
            }
        }

        private bool CanSelect(ITerritory territory)
        {
            return territory == SelectedTerritory;
        }

        private void Select(ITerritory location)
        {
            if (CanSelect(location))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateSelectState(_stateController, PlayerId);
            }
        }

        private bool CanAttack(ITerritory territory)
        {
            return _stateController.Game.CanAttack(SelectedTerritory, territory);
        }

        private void Attack(ITerritory territory)
        {
            //var canAttack = CanAttack(territory);

            //if (!canAttack)
            //{
            //    return;
            //}

            var attack = _stateController.Game.Attack(SelectedTerritory, territory);

            if (attack == AttackResult.SucceededAndOccupying)
            {
                _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, PlayerId, territory);
            }
        }

        
    }
}