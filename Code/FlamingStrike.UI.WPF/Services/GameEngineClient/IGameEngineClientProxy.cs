using System;
using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public interface IGameEngineClientProxy
    {
        IObservable<IGamePlaySetup> OnGamePlaySetup { get; }
        IObservable<ITerritorySelector> OnSelectRegion { get; }

        void Setup(IEnumerable<string> players);
        void StartGame(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup);
    }
}