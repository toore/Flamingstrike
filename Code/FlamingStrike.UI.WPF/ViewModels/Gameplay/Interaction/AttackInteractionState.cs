using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IAttackInteractionStateObserver
    {
        void DeselectRegion();
    }

    public class AttackInteractionState : IInteractionState
    {
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IAttackInteractionStateObserver _attackInteractionStateObserver;

        public AttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _attackInteractionStateObserver = attackInteractionStateObserver;
        }

        public void OnRegionClicked(IRegion region)
        {
            if (CanDeselect(region))
            {
                Deselect();
            }
            else
            {
                Attack(region);
            }
        }

        private bool CanDeselect(IRegion region)
        {
            return region == _selectedRegion;
        }

        private void Deselect()
        {
            _attackInteractionStateObserver.DeselectRegion();
        }

        private void Attack(IRegion attackedRegion)
        {
            _attackPhase.Attack(_selectedRegion, attackedRegion);
        }
    }
}