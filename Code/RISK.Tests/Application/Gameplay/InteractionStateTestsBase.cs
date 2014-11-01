using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public abstract class InteractionStateTestsBase
    {
        protected IInteractionState Sut;
        protected readonly IPlayer Player;
        protected readonly IBattleCalculator BattleCalculator;
        protected readonly StateController StateController;
        protected readonly IInteractionStateFactory InteractionStateFactory;
        protected readonly IWorldMap WorldMap;
        protected readonly ICardFactory CardFactory;

        protected InteractionStateTestsBase()
        {
            StateController = new StateController();
            InteractionStateFactory = Substitute.For<IInteractionStateFactory>();
            Player = Substitute.For<IPlayer>();
            WorldMap = Substitute.For<IWorldMap>();
            BattleCalculator = Substitute.For<IBattleCalculator>();
            CardFactory = Substitute.For<ICardFactory>();
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            StateController.PlayerShouldReceiveCardWhenTurnEnds = true;

            Sut.EndTurn();

            Player.ReceivedWithAnyArgs().AddCard(null);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            StateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            Sut.EndTurn();

            Player.DidNotReceiveWithAnyArgs().AddCard(null);
        }
    }
}