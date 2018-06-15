using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.Service.Play;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    //[Authorize]
    public class GameEngineHub : Hub
    {
        private readonly IAlternateGameSetupBootstrapper _alternateGameSetupBootstrapper;
        private readonly IGameBootstrapper _gameBootstrapper;

        private static AlternateGameSetupClientProxy _alternateGameSetupClientProxy;
        private static GameClientProxy _gameClientProxy;

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
                () => { _alternateGameSetupClientProxy.PlaceArmyInRegion(regionName.MapRegionNameToEngine()); });
        }

        public async Task StartGame(GamePlaySetup gamePlaySetup)
        {
            await Task.Run(
                () =>
                    {
                        _gameClientProxy = new GameClientProxy(this);

                        _gameBootstrapper.Run(
                            _gameClientProxy,
                            new GameEngine.Setup.Finished.GamePlaySetup(
                                gamePlaySetup.GetPlayers().Select(x => x.MapToEngine()).ToList(),
                                gamePlaySetup.GetTerritories().Select(x => x.MapToEngine()).ToList()));
                    });
        }

        public async Task PlaceDraftArmies(PlaceDraftArmies placeDraftArmies)
        {
            await Task.Run(
                () => { _gameClientProxy.PlaceDraftArmies(placeDraftArmies.Region.MapRegionNameToEngine(), placeDraftArmies.NumberOfArmies); });
        }

        public async Task Attack(Attack attack)
        {
            await Task.Run(
                () => { _gameClientProxy.Attack(attack.AttackingRegion.MapRegionNameToEngine(), attack.DefendingRegion.MapRegionNameToEngine()); });
        }

        public async Task Fortify(Fortify fortify)
        {
            await Task.Run(
                () => { _gameClientProxy.Fortify(fortify.SourceRegion.MapRegionNameToEngine(), fortify.DestinationRegion.MapRegionNameToEngine(), fortify.Armies); });
        }

        public async Task<IEnumerable<string>> GetRegionsThatCanBeAttacked(string sourceRegion)
        {
            return await Task.Run(
                () => _gameClientProxy.GetRegionsThatCanBeAttacked(sourceRegion.MapRegionNameToEngine()).Select(x => x.Name));
        }

        public async Task GetRegionsThatCanBeFortified(string sourceRegion)
        {
            await Task.Run(
                () => _gameClientProxy.GetRegionsThatCanBeFortified(sourceRegion.MapRegionNameToEngine()));
        }
    }
}