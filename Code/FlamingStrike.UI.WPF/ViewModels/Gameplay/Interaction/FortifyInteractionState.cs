using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IFortifyInteractionStateObserver
    {
        void DeselectRegion();
    }

    public class FortifyInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IFortifyInteractionStateObserver _fortifyInteractionStateObserver;

        public FortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _fortifyInteractionStateObserver = fortifyInteractionStateObserver;

            var regionsThatCanBeInteractedWith = attackPhase.GetRegionsThatCanBeFortified(selectedRegion)
                .Concat(new[] { selectedRegion }).ToList();

            EnabledRegions = regionsThatCanBeInteractedWith;
        }

        public override string Title => Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_TO;

        public override bool CanEndTurn => true;
        public override IReadOnlyList<IRegion> EnabledRegions { get; }
        public override Maybe<IRegion> SelectedRegion => Maybe<IRegion>.Create(_selectedRegion);

        public override void OnRegionClicked(IRegion region)
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

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
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