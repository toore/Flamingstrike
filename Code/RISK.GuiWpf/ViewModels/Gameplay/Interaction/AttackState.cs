using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class AttackState : IInteractionState
    {
        public AttackState(IInteractionStateFactory interactionStateFactory)
        {
            throw new NotImplementedException();
        }

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
        }

        public void OnClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
        }

        //    private readonly IStateController _stateController;
        //    private readonly IInteractionStateFactory _interactionStateFactory;
        //    private InteractionStateFactory _interactionStateFactory;

        //    public AttackState(
        //        IStateController stateController,
        //        IInteractionStateFactory interactionStateFactory)
        //    {
        //        _stateController = stateController;
        //        _interactionStateFactory = interactionStateFactory;
        //    }

        //    public AttackState(InteractionStateFactory interactionStateFactory)
        //    {
        //        _interactionStateFactory = interactionStateFactory;
        //    }

        //    public bool CanClick(ITerritory territory)
        //    {
        //        return CanSelect(territory)
        //               ||
        //               CanAttack(territory);
        //    }

        //    public void OnClick(ITerritory territory)
        //    {
        //        if (!CanSelect(territory) && !CanAttack(territory))
        //        {
        //            throw new InvalidOperationException();
        //        }

        //        if (CanSelect(territory))
        //        {
        //            Select(territory);
        //        }
        //        else if (CanAttack(territory))
        //        {
        //            Attack(territory);
        //        }
        //    }

        //    private bool CanSelect(ITerritory territory)
        //    {
        //        return territory == SelectedTerritory;
        //    }

        //    private void Select(ITerritory location)
        //    {
        //        if (CanSelect(location))
        //        {
        //            _stateController.CurrentState = _interactionStateFactory.CreateSelectState();
        //        }
        //    }

        //    private bool CanAttack(ITerritory territory)
        //    {
        //        var canAttack = _stateController.Game.GetAttackeeCandidates(SelectedTerritory).Contains(territory);
        //        return canAttack;
        //    }

        //    private void Attack(ITerritory territory)
        //    {
        //        //var canAttack = CanAttack(territory);

        //        //if (!canAttack)
        //        //{
        //        //    return;
        //        //}

        //        var attack = _stateController.Game.Attack(SelectedTerritory, territory);

        //        if (attack == AttackResult.SucceededAndOccupying)
        //        {
        //            _stateController.CurrentState = _interactionStateFactory.CreateAttackState();
        //        }
        //    }
    }
}