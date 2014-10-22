using System;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public abstract class TurnStateTestsBase 
    {
        protected ITurnState Sut;
        protected readonly IPlayer Player;
        protected readonly IBattleCalculator BattleCalculator;
        protected readonly ILocation SelectedLocation;
        protected readonly ITerritory SelectedTerritory;
        protected ILocation OtherLocation;
        protected ITerritory OtherTerritory;
        protected readonly StateController StateController;
        protected readonly IWorldMap WorldMap;
        protected readonly ICardFactory CardFactory;

        protected TurnStateTestsBase()
        {
            StateController = new StateController();
            Player = Substitute.For<IPlayer>();
            WorldMap = Substitute.For<IWorldMap>();
            BattleCalculator = Substitute.For<IBattleCalculator>();
            CardFactory = Substitute.For<ICardFactory>();
            SelectedLocation = Substitute.For<ILocation>();
            SelectedTerritory = WorldMap.HasTerritory(SelectedLocation, Player);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            StateController.PlayerShouldReceiveCardWhenTurnEnds = true;

            Sut.EndTurn();

            Player.Received().AddCard(null);
        }
    }

    static class TurnTestsExtensions
    {
        public static void IsConnectedTo(this ILocation from, params ILocation[] to)
        {
            from.Connections.Returns(to);
        }

        public static void AttackerAlwaysWins(this IBattleCalculator battleCalculator, ITerritory from, ITerritory to)
        {
            battleCalculator
                .When(x => x.Attack(from, to))
                .Do(x => to.Occupant = from.Occupant);
        }

        public static ITerritory HasTerritory(this IWorldMap worldMap, ILocation location, IPlayer owner)
        {
            var territory = Substitute.For<ITerritory>();
            territory.Location.Returns(location);
            territory.Occupant = owner;

            worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }
    }

    class TurnStateStub : ITurnState
    {
        public IPlayer Player { get; set; }
        public ITerritory SelectedTerritory { get; set; }
        public bool IsTerritorySelected { get; set; }

        #region Not implemented methods
        public bool CanSelect(ILocation location)
        {
            throw new NotImplementedException();
        }

        public bool CanAttack(ILocation location)
        {
            throw new NotImplementedException();
        }

        public bool IsFortificationAllowedInTurn()
        {
            throw new NotImplementedException();
        }

        public bool CanFortify(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Fortify(ILocation location, int armies)
        {
            throw new NotImplementedException();
        }

        public void Select(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Attack(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}