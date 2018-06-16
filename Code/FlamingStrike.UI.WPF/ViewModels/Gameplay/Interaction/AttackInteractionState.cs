using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IAttackInteractionStateObserver
    {
        void DeselectRegion();
    }

    public class AttackInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly Region _selectedRegion;
        private readonly IAttackInteractionStateObserver _attackInteractionStateObserver;

        public AttackInteractionState(IAttackPhase attackPhase, Region selectedRegion, IReadOnlyList<Region> enabledRegions, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _attackInteractionStateObserver = attackInteractionStateObserver;
            EnabledRegions = enabledRegions;
        }

        public override string Title => Resources.SELECT_TERRITORY_TO_ATTACK;

        public override bool CanEndTurn => true;

        public override IReadOnlyList<Region> EnabledRegions { get; }

        public override Maybe<Region> SelectedRegion => Maybe<Region>.Create(_selectedRegion);

        public override bool CanUserSelectNumberOfArmies => true;

        public override int DefaultNumberOfUserSelectedArmies => MaxNumberOfUserSelectableArmies;

        public override int MaxNumberOfUserSelectableArmies => GetSelectedTerritory().GetMaxNumberOfPossibleAttackingArmies();

        public override void OnRegionClicked(Region region)
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

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
        }

        private Services.GameEngineClient.Play.Territory GetSelectedTerritory()
        {
            return _attackPhase.Territories.Single(x => x.Region == _selectedRegion);
        }

        private bool CanDeselect(Region region)
        {
            return region == _selectedRegion;
        }

        private void Deselect()
        {
            _attackInteractionStateObserver.DeselectRegion();
        }

        private void Attack(Region attackedRegion)
        {
            _attackPhase.Attack(_selectedRegion, attackedRegion);
        }
    }
}