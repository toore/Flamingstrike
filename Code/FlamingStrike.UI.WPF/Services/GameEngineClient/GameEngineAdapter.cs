using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public partial class GameEngineAdapter : IGameEngineClientProxy
    {
        private readonly IAlternateGameSetupBootstrapper _alternateGameSetupBootstrapper;
        private readonly IGameBootstrapper _gameBootstrapper;

        public GameEngineAdapter(IAlternateGameSetupBootstrapper alternateGameSetupBootstrapper, IGameBootstrapper gameBootstrapper)
        {
            _alternateGameSetupBootstrapper = alternateGameSetupBootstrapper;
            _gameBootstrapper = gameBootstrapper;
        }

        public void Setup(IAlternateGameSetupObserver alternateGameSetupObserver, IEnumerable<string> players)
        {
            var gameSetupObserverAdapter = new AlternateGameSetupObserverAdapter(alternateGameSetupObserver);
            var playerNames = players.Select(x => new PlayerName(x)).ToList();

            _alternateGameSetupBootstrapper.Run(gameSetupObserverAdapter, playerNames);
        }

        public void StartGame(IGameplayViewModel gameplayViewModel, SetupFinished.IGamePlaySetup gamePlaySetup)
        {
            var gameObserverAdapter = new GameObserverAdapter(gameplayViewModel);

            var players = gamePlaySetup.GetPlayers().Select(x => new PlayerName(x)).ToList();
            var territories = gamePlaySetup.GetTerritories().Select(x => new GameEngine.Setup.Finished.Territory(x.Region.MapToEngine(), new PlayerName(x.PlayerName), x.Armies)).ToList();
            var setup = new GameEngine.Setup.Finished.GamePlaySetup(players, territories);

            _gameBootstrapper.Run(gameObserverAdapter, setup);
        }
    }
}