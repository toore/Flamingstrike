using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;

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

        protected ITerritoryGeography CreateTerritoryOccupiedByCurrentPlayer()
        {
            var territory = Substitute.For<ITerritoryGeography>();
            _game.IsCurrentPlayerOccupyingTerritory(territory).Returns(true);

            return territory;
        }
    }
}