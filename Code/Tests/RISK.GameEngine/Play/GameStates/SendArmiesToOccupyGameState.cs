using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using Tests.Builders;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace Tests.RISK.GameEngine.Play.GameStates
{
    public class SendArmiesToOccupyGameStateTests
    {
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly ITerritory _attackingTerritory;
        private readonly ITerritory _occupiedTerritory;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly IDeck _deck;

        public SendArmiesToOccupyGameStateTests()
        {
            _territoriesContext = Substitute.For<ITerritoriesContext>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _territoryOccupier = Substitute.For<ITerritoryOccupier>();

            _attackingTerritory = Substitute.For<ITerritory>();
            _occupiedTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Make.Player.Build();
            _anotherPlayer = Make.Player.Build();

            _attackingRegion = Substitute.For<IRegion>();
            _occupiedRegion = Substitute.For<IRegion>();
            _attackingTerritory.Region.Returns(_attackingRegion);
            _attackingTerritory.Player.Returns(_currentPlayer);
            _occupiedTerritory.Region.Returns(_occupiedRegion);
            _occupiedTerritory.Player.Returns(_currentPlayer);

            _deck = Substitute.For<IDeck>();
        }

        private SendArmiesToOccupyGameState Sut => new SendArmiesToOccupyGameState(
            _territoriesContext, 
            _gamePhaseConductor, 
            _territoryOccupier, 
            _attackingRegion, 
            _occupiedRegion);

        [Fact]
        public void Sending_armies_to_occupy_continues_with_attack_state()
        {
            var updatedTerritoriesAfterAdditionalArmiesHaveBeenSentToOccupy = new ITerritory[] { Make.Territory.Build(), Make.Territory.Build() };
            _territoryOccupier.SendInAdditionalArmiesToOccupy(
                _territoriesContext.Territories,
                _attackingRegion,
                _occupiedRegion,
                1).Returns(updatedTerritoriesAfterAdditionalArmiesHaveBeenSentToOccupy);

            Sut.SendAdditionalArmiesToOccupy(1);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory);
        }
    }
}