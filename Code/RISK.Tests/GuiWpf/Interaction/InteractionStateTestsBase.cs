﻿using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.Play;

namespace RISK.Tests.GuiWpf.Interaction
{
    public abstract class InteractionStateTestsBase
    {
        protected readonly StateController _sut;
        protected readonly IGame _game;
        protected readonly IInteractionStateFactory _interactionStateFactory;

        protected InteractionStateTestsBase()
        {
            _game = Substitute.For<IGame>();
            _sut = new StateController(_game);

            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
        }
    }
}