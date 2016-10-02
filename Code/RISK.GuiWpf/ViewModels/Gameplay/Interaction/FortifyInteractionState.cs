using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IDeselectRegionToFortifyFromObserver
    {
        void DeselectRegion();
    }

    public class FortifyInteractionState : IInteractionState
    {
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IDeselectRegionToFortifyFromObserver _deselectRegionToFortifyFromObserver;

        public FortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectRegionToFortifyFromObserver deselectRegionToFortifyFromObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _deselectRegionToFortifyFromObserver = deselectRegionToFortifyFromObserver;
        }

        public void OnClick(IRegion region)
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
            _deselectRegionToFortifyFromObserver.DeselectRegion();
        }

        private void Fortify(IRegion regionToFortify)
        {
            _attackPhase.Fortify(_selectedRegion, regionToFortify, 1);
        }
    }
}