using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
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
            _attackInteractionStateObserver.DeselectRegion();
        }

        private void Attack(IRegion attackedRegion)
        {
            _attackPhase.Attack(_selectedRegion, attackedRegion);
        }
    }
}