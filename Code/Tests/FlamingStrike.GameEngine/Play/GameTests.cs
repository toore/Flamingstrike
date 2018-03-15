using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Setup;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Play
{
    public class GameTests
    {
        private readonly GameBootstrapper _bootstrapper;
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;
        private readonly IGameObserver _gameObserver;

        public GameTests()
        {
            _gamePhaseFactory = Substitute.For<IGamePhaseFactory>();
            _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();
            _deckFactory = Substitute.For<IDeckFactory>();

            _bootstrapper = new GameBootstrapper(_gamePhaseFactory, _armyDraftCalculator, _deckFactory);

            _gameObserver = Substitute.For<IGameObserver>();
        }

        [Fact]
        public void When_game_is_created_draft_armies_phase_starts()
        {
            Run(new GamePlaySetupBuilder().Build());

            _gameObserver.Received().DraftArmies(Arg.Any<IDraftArmiesPhase>());
        }

        private void Run(IGamePlaySetup gamePlaySetup)
        {
            _bootstrapper.Run(_gameObserver, gamePlaySetup);
        }
    }
}