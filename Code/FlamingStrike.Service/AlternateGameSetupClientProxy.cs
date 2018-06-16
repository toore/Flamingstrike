using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    public class AlternateGameSetupClientProxy : IAlternateGameSetupObserver
    {
        private readonly IClientProxy _clientProxy;
        private ITerritorySelector _territorySelector;

        public AlternateGameSetupClientProxy(IClientProxy clientProxy)
        {
            _clientProxy = clientProxy;
        }

        public void SelectRegion(ITerritorySelector territorySelector)
        {
            _territorySelector = territorySelector;
            _clientProxy.SendAsync(
                "SelectRegion", new
                    {
                        Player = (string)territorySelector.Player,
                        territorySelector.ArmiesLeftToPlace,
                        Territories = territorySelector.GetTerritories().Select(x => x.MapToDto())
                    });
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _clientProxy.SendAsync(
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