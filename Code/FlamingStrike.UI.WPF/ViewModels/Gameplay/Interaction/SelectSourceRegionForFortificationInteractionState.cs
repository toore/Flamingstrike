﻿using System.Collections.Generic;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectSourceRegionForFortificationInteractionStateObserver
    {
        void Select(Region region);
    }

    public class SelectSourceRegionForFortificationInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectSourceRegionForFortificationInteractionStateObserver _selectSourceRegionForFortificationInteractionStateObserver;

        public SelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectSourceRegionForFortificationInteractionStateObserver = selectSourceRegionForFortificationInteractionStateObserver;
        }

        public override string Title => Resources.SELECT_TERRITORY_TO_MOVE_FROM;

        public override bool CanEnterAttackMode => true;

        public override bool CanEndTurn => true;

        public override IReadOnlyList<Region> EnabledRegions => _attackPhase.RegionsThatCanBeSourceForAttackOrFortification;

        public override void OnRegionClicked(Region region)
        {
            _selectSourceRegionForFortificationInteractionStateObserver.Select(region);
        }

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
        }
    }
}