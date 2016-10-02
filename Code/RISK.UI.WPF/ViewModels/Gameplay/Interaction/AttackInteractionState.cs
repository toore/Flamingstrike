using RISK.Core;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IDeselectAttackingRegionObserver
    {
        void DeselectRegion();
    }

    public class AttackInteractionState : IInteractionState
    {
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IDeselectAttackingRegionObserver _deselectAttackingRegionObserver;

        public AttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectAttackingRegionObserver deselectAttackingRegionObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _deselectAttackingRegionObserver = deselectAttackingRegionObserver;
        }

        public void OnClick(IRegion region)
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
            _deselectAttackingRegionObserver.DeselectRegion();
        }

        private void Attack(IRegion attackedRegion)
        {
            _attackPhase.Attack(_selectedRegion, attackedRegion);
        }
    }
}