using System;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Tests.RISK.GameEngine.Builders;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectAttackInteractionStateTests
    {
        private readonly SelectAttackingRegionInteractionState _sut;
        private readonly ISelectAttackingRegionObserver _selectAttackingRegionObserver;

        public SelectAttackInteractionStateTests()
        {
            _selectAttackingRegionObserver = Substitute.For<ISelectAttackingRegionObserver>();

            _sut = new SelectAttackingRegionInteractionState(_selectAttackingRegionObserver);
        }

        [Fact]
        public void Selects_a_region_to_attack_from()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _selectAttackingRegionObserver.Received().SelectRegionToAttackFrom(region);
        }
    }
}