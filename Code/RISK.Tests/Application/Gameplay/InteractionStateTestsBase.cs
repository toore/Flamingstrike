using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

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
    }
}