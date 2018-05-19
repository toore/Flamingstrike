using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using GamePlaySetup = FlamingStrike.GameEngine.Setup.Finished.GamePlaySetup;
using Territory = FlamingStrike.GameEngine.Setup.Finished.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class GameEngineAdapter : IGameEngineClientProxy
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

        public void StartGame(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            var gameObserverAdapter = new GameObserverAdapter(gameObserver);

            var players = gamePlaySetup.GetPlayers().Select(x => new PlayerName(x)).ToList();
            var territories = gamePlaySetup.GetTerritories().Select(x => new Territory(x.Region.MapToEngine(), new PlayerName(x.PlayerName), x.Armies)).ToList();
            var setup = new GamePlaySetup(players, territories);

            _gameBootstrapper.Run(gameObserverAdapter, setup);
        }
    }
}