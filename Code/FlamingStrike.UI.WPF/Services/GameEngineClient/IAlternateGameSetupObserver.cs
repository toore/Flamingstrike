using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public interface IAlternateGameSetupObserver
    {
        void SelectRegion(ITerritorySelector territorySelector);
        void NewGamePlaySetup(IGamePlaySetup gamePlaySetup);
    }
}