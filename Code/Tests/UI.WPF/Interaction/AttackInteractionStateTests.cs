﻿using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF.Interaction
{
    public class AttackInteractionStateTests
    {
        private readonly AttackInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly Region _selectedRegion;
        private readonly Region _attackedRegion;
        private readonly IAttackInteractionStateObserver _attackInteractionStateObserver;

        public AttackInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectedRegion = Region.Brazil;
            _attackInteractionStateObserver = Substitute.For<IAttackInteractionStateObserver>();

            _sut = new AttackInteractionState(_attackPhase, _selectedRegion, _attackInteractionStateObserver);

            _attackedRegion = Region.NorthAfrica;
        }

        [Fact]
        public void OnClick_attacks()
        {
            _sut.OnRegionClicked(_attackedRegion);

            _attackPhase.Received().Attack(_selectedRegion, _attackedRegion);
        }

        [Fact]
        public void OnClick_deselects()
        {
            _sut.OnRegionClicked(_selectedRegion);

            _attackInteractionStateObserver.Received().DeselectRegion();
        }
    }
}