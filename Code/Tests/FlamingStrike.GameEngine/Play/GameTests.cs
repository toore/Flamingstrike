using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Play.GameStates;
using FlamingStrike.GameEngine.Setup;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Setup;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Play
{
    public class GameTests
    {
        private readonly GameFactory _factory;
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;
        private readonly IGameObserver _gameObserver;

        public GameTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();
            _deckFactory = Substitute.For<IDeckFactory>();

            _factory = new GameFactory(_gameStateFactory, _armyDraftCalculator, _deckFactory);

            _gameObserver = Substitute.For<IGameObserver>();
        }

        [Fact]
        public void When_game_is_created_draft_armies_phase_starts()
        {
            var game = Create(new GamePlaySetupBuilder().Build());

            _gameObserver.Received().DraftArmies(Arg.Any<IDraftArmiesPhase>());
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _factory.Create(_gameObserver, gamePlaySetup);
        }
    }
}