using System.Linq;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using GamePlaySetup = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished.GamePlaySetup;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;
using TerritorySelector = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.TerritorySelector;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public partial class GameEngineAdapter
    {
        private class AlternateGameSetupObserverAdapter : GameEngine.Setup.IAlternateGameSetupObserver, IArmyPlacer
        {
            private readonly IAlternateGameSetupObserver _alternateGameSetupObserver;
            private ITerritorySelector _territorySelector;

            public AlternateGameSetupObserverAdapter(IAlternateGameSetupObserver alternateGameSetupObserver)
            {
                _alternateGameSetupObserver = alternateGameSetupObserver;
            }

            public void SelectRegion(ITerritorySelector territorySelector)
            {
                _territorySelector = territorySelector;

                var territories = territorySelector.GetTerritories()
                    .Select(x => new Territory(x.Region.MapFromEngine(), (string)x.Name, x.Armies, x.IsSelectable))
                    .ToList();
                var selector = new TerritorySelector(this, (string)territorySelector.Player, territorySelector.ArmiesLeftToPlace, territories);

                _alternateGameSetupObserver.SelectRegion(selector);
            }

            public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
            {
                var players = gamePlaySetup.GetPlayers().Select(x => (string)x).ToList();
                var territories = gamePlaySetup.GetTerritories()
                    .Select(x => new SetupFinished.Territory(x.Region.MapFromEngine(), (string)x.Name, x.Armies))
                    .ToList();

                _alternateGameSetupObserver.NewGamePlaySetup(new GamePlaySetup(players, territories));
            }

            public void PlaceArmyInRegion(Region selectedRegion)
            {
                _territorySelector.PlaceArmyInRegion(selectedRegion.MapToEngine());
            }
        }
    }
}