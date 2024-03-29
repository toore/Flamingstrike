﻿using System.Collections.Generic;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectAttackingRegionInteractionStateObserver
    {
        void Select(Region selectedRegion);
    }

    public class SelectAttackingRegionInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingRegionInteractionState(IAttackPhase attackPhase, ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectAttackingRegionInteractionStateObserver = selectAttackingRegionInteractionStateObserver;
        }

        public override string Title => Resources.SELECT_TERRITORY_TO_START_THE_ATTACK_FROM;

        public override bool CanEnterFortifyMode => true;

        public override bool CanEndTurn => true;

        public override IReadOnlyList<Region> EnabledRegions => _attackPhase.RegionsThatCanBeSourceForAttackOrFortification;

        public override void OnRegionClicked(Region region)
        {
            _selectAttackingRegionInteractionStateObserver.Select(region);
        }

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
        }
    }
}