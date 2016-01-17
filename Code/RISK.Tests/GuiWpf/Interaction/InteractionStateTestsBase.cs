using System.Collections.Generic;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
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
        private readonly List<ITerritory> _territories;

        protected InteractionStateTestsBase()
        {
            _game = Substitute.For<IGame>();
            _sut = new StateController(_game);

            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();

            _territories = new List<ITerritory>();
            _game.Territories.Returns(_territories);
        }

        protected ITerritoryGeography AddTerritoryOccupiedByCurrentPlayer()
        {
            var territory = AddTerritory();

            _game.IsCurrentPlayerOccupyingTerritory(territory).Returns(true);

            return territory.TerritoryGeography;
        }

        protected Territory AddTerritory()
        {
            var territoryGeography = Substitute.For<ITerritoryGeography>();
            var territory = Make.Territory.TerritoryGeography(territoryGeography).Build();

            _territories.Add(territory);

            return territory;
        }
    }
}