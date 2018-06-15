using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IFortifyInteractionStateObserver
    {
        void DeselectRegion();
    }

    public class FortifyInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly Region _selectedRegion;
        private readonly IFortifyInteractionStateObserver _fortifyInteractionStateObserver;

        public FortifyInteractionState(IAttackPhase attackPhase, Region selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _fortifyInteractionStateObserver = fortifyInteractionStateObserver;

            var regionsThatCanBeInteractedWith = attackPhase.GetRegionsThatCanBeFortified(selectedRegion)
                .Concat(new[] { selectedRegion }).ToList();

            EnabledRegions = regionsThatCanBeInteractedWith;
        }

        public override string Title => Resources.SELECT_TERRITORY_TO_MOVE_TO;

        public override bool CanEndTurn => true;

        public override IReadOnlyList<Region> EnabledRegions { get; }

        public override Maybe<Region> SelectedRegion => Maybe<Region>.Create(_selectedRegion);

        public override bool CanUserSelectNumberOfArmies => true;

        public bool CanShowCards => true;

        public override void OnRegionClicked(Region region)
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

        private bool CanDeselect(Region region)
        {
            return region == _selectedRegion;
        }

        private void Deselect()
        {
            _fortifyInteractionStateObserver.DeselectRegion();
        }

        private void Fortify(Region regionToFortify)
        {
            _attackPhase.Fortify(_selectedRegion, regionToFortify, 1);
        }
    }
}