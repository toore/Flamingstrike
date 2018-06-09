using System;
using System.Linq;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class AlternateGameSetupObserverAdapter : GameEngine.Setup.IAlternateGameSetupObserver, IArmyPlacer
    {
        private readonly IObserver<ITerritorySelector> _territorySelectorObserver;
        private readonly IObserver<IGamePlaySetup> _gamePlaySetupObserver;
        private GameEngine.Setup.TerritorySelection.ITerritorySelector _territorySelector;

        public AlternateGameSetupObserverAdapter(
            IObserver<ITerritorySelector> territorySelectorObserver,
            IObserver<IGamePlaySetup> gamePlaySetupObserver)
        {
            _territorySelectorObserver = territorySelectorObserver;
            _gamePlaySetupObserver = gamePlaySetupObserver;
        }

        public void SelectRegion(GameEngine.Setup.TerritorySelection.ITerritorySelector territorySelector)
        {
            _territorySelector = territorySelector;

            var territories = territorySelector.GetTerritories()
                .Select(x => new Territory(x.Region.MapFromEngine(), (string)x.Name, x.Armies, x.IsSelectable))
                .ToList();
            var selector = new TerritorySelector(this, (string)territorySelector.Player, territorySelector.ArmiesLeftToPlace, territories);

            _territorySelectorObserver.OnNext(selector);
        }

        public void NewGamePlaySetup(GameEngine.Setup.Finished.IGamePlaySetup gamePlaySetup)
        {
            var players = gamePlaySetup.GetPlayers().Select(x => (string)x).ToList();
            var territories = gamePlaySetup.GetTerritories()
                .Select(x => new SetupFinished.Territory(x.Region.MapFromEngine(), (string)x.Name, x.Armies))
                .ToList();

            _gamePlaySetupObserver.OnNext(new GamePlaySetup(players, territories));
        }

        public void PlaceArmyInRegion(Region selectedRegion)
        {
            _territorySelector.PlaceArmyInRegion(selectedRegion.MapToEngine());
        }
    }
}