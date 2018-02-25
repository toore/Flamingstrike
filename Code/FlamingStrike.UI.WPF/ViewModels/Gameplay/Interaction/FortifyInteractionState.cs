using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IFortifyInteractionStateObserver
    {
        void DeselectRegion();
    }

    public class FortifyInteractionState : IInteractionState
    {
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IFortifyInteractionStateObserver _fortifyInteractionStateObserver;

        public FortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _fortifyInteractionStateObserver = fortifyInteractionStateObserver;
        }

        public void OnRegionClicked(IRegion region)
        {
            if (CanDeselect(region))
            {
                Deselect();
            }
            else
            {
                Fortify(region);
            }
        }

        private bool CanDeselect(IRegion region)
        {
            return region == _selectedRegion;
        }

        private void Deselect()
        {
            _fortifyInteractionStateObserver.DeselectRegion();
        }

        private void Fortify(IRegion regionToFortify)
        {
            _attackPhase.Fortify(_selectedRegion, regionToFortify, 1);
        }
    }
}