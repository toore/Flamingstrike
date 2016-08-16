using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class AttackInteractionState : IInteractionState
    {
        private readonly IInteractionContext _interactionContext;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;

        public AttackInteractionState(IInteractionContext interactionContext, IInteractionStateFactory interactionStateFactory, IGame game, IRegion selectedRegion)
        {
            _interactionContext = interactionContext;
            _interactionStateFactory = interactionStateFactory;
            _game = game;
            SelectedRegion = selectedRegion;
        }

        public IRegion SelectedRegion { get; }

        public bool CanClick(IRegion region)
        {
            return CanAttack(region)
                   ||
                   CanDeselect(region);
        }

        public void OnClick(IRegion region)
        {
            if (CanDeselect(region))
            {
                Deselect();
            }
            else if (CanAttack(region))
            {
                Attack(region);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private bool CanDeselect(IRegion region)
        {
            return region == SelectedRegion;
        }

        private void Deselect()
        {
            EnterSelectState();
        }

        private void EnterSelectState()
        {
            var selectState = _interactionStateFactory.CreateSelectInteractionState(_game);
            _interactionContext.Set(selectState);
        }

        private bool CanAttack(IRegion attackeeRegion)
        {
            var canAttack = _game.CanAttack(SelectedRegion, attackeeRegion);

            return canAttack;
        }

        private void Attack(IRegion attackedRegion)
        {
            _game.Attack(SelectedRegion, attackedRegion);

            if (_game.CanSendArmiesToOccupy())
            {
                EnterSendArmiesToOccupyState();
            }
        }

        private void EnterSendArmiesToOccupyState()
        {
            var sendArmiesToOccupyInteractionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(_game);
            _interactionContext.Set(sendArmiesToOccupyInteractionState);
        }
    }
}