using System;
using System.Collections.Generic;
using FluentAssertions;
using GuiWpf.ViewModels.AlternateSetup;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class UserInteractorTests
    {
        private readonly UserInteractorFactory _userInteractorFactory;
        private readonly IUserInteraction _userInteraction;

        public UserInteractorTests()
        {
            var userInteractionFactory = Substitute.For<IUserInteractionFactory>();
            var guiThreadDispatcher = new CurrentThreadDispatcher();

            _userInteractorFactory = new UserInteractorFactory(userInteractionFactory, guiThreadDispatcher);

            _userInteraction = Substitute.For<IUserInteraction>();
            userInteractionFactory.Create().Returns(_userInteraction);
        }

        [Fact]
        public void A_territory_request_gets_territory_from_user()
        {
            var territoryRequestParameter = Substitute.For<ITerritoryRequestParameter>();
            var expectedTerritory = Make.Region.Build();
            var gameSetupViewModel = Substitute.For<IAlternateGameSetupViewModel>();

            _userInteraction.WaitForTerritoryToBeSelected(territoryRequestParameter).Returns(expectedTerritory);

            var sut = _userInteractorFactory.Create(gameSetupViewModel);
            var actual = sut.ProcessRequest(territoryRequestParameter);

            actual.Should().Be(expectedTerritory);
        }

        [Fact]
        public void A_territory_request_updates_view()
        {
            var territoryRequestParameter = Substitute.For<ITerritoryRequestParameter>();
            var gameSetupViewModel = Substitute.For<IAlternateGameSetupViewModel>();
            var territories = new List<
                ITerritory>();
            Action<IRegion> selectTerritoryAction = _userInteraction.SelectTerritory;
            var enabledTerritories = new List<IRegion> { Make.Region.Build() };
            const string playerName = "any player";
            const int armiesLeftToPlace = 1;
            territoryRequestParameter.Territories.Returns(territories);
            territoryRequestParameter.EnabledTerritories.Returns(enabledTerritories);
            territoryRequestParameter.Player.Returns(new Player("any player"));
            territoryRequestParameter.GetArmiesLeftToPlace().Returns(1);

            var sut = _userInteractorFactory.Create(gameSetupViewModel);
            sut.ProcessRequest(territoryRequestParameter);

            gameSetupViewModel.Received()
                .UpdateView(territories, selectTerritoryAction, enabledTerritories, playerName, armiesLeftToPlace);
        }
    }
}