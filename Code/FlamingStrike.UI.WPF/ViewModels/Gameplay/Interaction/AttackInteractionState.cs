using System.Collections.Generic;
using System.Linq;
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
        private readonly IRegion _selectedRegion;
        private readonly IAttackInteractionStateObserver _attackInteractionStateObserver;

        public AttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
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

        public override IReadOnlyList<IRegion> EnabledRegions { get; }
        
        public override Maybe<IRegion> SelectedRegion => Maybe<IRegion>.Create(_selectedRegion);

        public override bool CanUserSelectNumberOfArmies => true;

        public override void OnRegionClicked(IRegion region)
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