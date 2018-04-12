using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

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

        public AttackInteractionState(IAttackPhase attackPhase, Region selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectedRegion = selectedRegion;
            _attackInteractionStateObserver = attackInteractionStateObserver;

            var regionsThatCanBeInteractedWith = attackPhase.GetRegionsThatCanBeAttacked(selectedRegion)
                .Concat(new[] { selectedRegion }).ToList();

            EnabledRegions = regionsThatCanBeInteractedWith;
        }

        public override string Title => Resources.ATTACK_SELECT_TERRITORY_TO_ATTACK;

        public override bool CanEndTurn => true;

        public override IReadOnlyList<Region> EnabledRegions { get; }

        public override Maybe<Region> SelectedRegion => Maybe<Region>.Create(_selectedRegion);

        public override bool CanUserSelectNumberOfArmies => true;

        public override int DefaultNumberOfUserSelectedArmies => MaxNumberOfUserSelectableArmies;

        public override int MaxNumberOfUserSelectableArmies => GetSelectedTerritory().GetNumberOfArmiesUsedInAnAttack();

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

        private ITerritory GetSelectedTerritory()
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