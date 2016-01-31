﻿using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using RISK.Tests.Builders;

namespace RISK.Tests.GuiWpf.Interaction
{
    public abstract class InteractionStateTestsBase
    {
        protected readonly StateController _sut;
        protected readonly IGame _game;
        protected readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IPlayer _currentPlayer;

        protected InteractionStateTestsBase()
        {
            _game = Substitute.For<IGame>();
            _sut = new StateController(_game);

            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();

            _currentPlayer = Substitute.For<IPlayer>();
            _game.CurrentPlayer.Returns(_currentPlayer);
        }

        protected IRegion AddTerritoryOccupiedByCurrentPlayer()
        {
            var region = Substitute.For<IRegion>();
            var territory = Make.Territory.Player(_currentPlayer).Build();

            _game.GetTerritory(region).Returns(territory);

            return region;
        }
    }
}