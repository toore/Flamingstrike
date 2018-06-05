using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    //[Authorize]
    public class GameEngineHub : Hub
    {
        private readonly IAlternateGameSetupBootstrapper _alternateGameSetupBootstrapper;
        private readonly IGameBootstrapper _gameBootstrapper;

        private static AlternateGameSetupClientProxy _alternateGameSetupClientProxy;

        public GameEngineHub(IAlternateGameSetupBootstrapper alternateGameSetupBootstrapper, IGameBootstrapper gameBootstrapper)
        {
            _alternateGameSetupBootstrapper = alternateGameSetupBootstrapper;
            _gameBootstrapper = gameBootstrapper;
        }

        public async Task RunSetup(IEnumerable<string> players)
        {
            await Task.Run(
                () =>
                    {
                        _alternateGameSetupClientProxy = new AlternateGameSetupClientProxy(this);
                        _alternateGameSetupBootstrapper.Run(_alternateGameSetupClientProxy, players.Select(x => x.MapToEngine()).ToList());
                    });
        }

        public async Task PlaceArmyInRegion(string regionName)
        {
            await Task.Run(
                () =>
                    {
                        var region = (Region)typeof(Region).GetFields()
                            .Where(x => x.IsPublic && x.IsStatic)
                            .Single(x => x.Name == regionName)
                            .GetValue(typeof(Region));

                        _alternateGameSetupClientProxy.PlaceArmyInRegion(region);
                    });
        }
    }

    public class AlternateGameSetupClientProxy : IAlternateGameSetupObserver
    {
        private readonly GameEngineHub _gameEngineHub;
        private ITerritorySelector _territorySelector;

        public AlternateGameSetupClientProxy(GameEngineHub gameEngineHub)
        {
            _gameEngineHub = gameEngineHub;
        }

        public void SelectRegion(ITerritorySelector territorySelector)
        {
            _territorySelector = territorySelector;
            _gameEngineHub.Clients.All.SendAsync(
                "SelectRegion", new
                    {
                        Player = (string)territorySelector.Player,
                        territorySelector.ArmiesLeftToPlace,
                        Territories = territorySelector.GetTerritories().Select(x => x.MapToDto())
                    });
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _gameEngineHub.Clients.All.SendAsync(
                "NewGamePlaySetup", new
                    {
                        Players = gamePlaySetup.GetPlayers().Select(x => (string)x),
                        Territories = gamePlaySetup.GetTerritories().Select(x => x.MapToDto())
                    });
        }

        public void PlaceArmyInRegion(Region region)
        {
            _territorySelector.PlaceArmyInRegion(region);
        }
    }
}