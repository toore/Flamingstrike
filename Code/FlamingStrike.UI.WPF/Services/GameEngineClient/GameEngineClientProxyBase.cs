using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public abstract class GameEngineClientProxyBase
    {
        protected readonly ISubject<ITerritorySelector> _territorySelectorSubject;
        protected readonly ISubject<IGamePlaySetup> _gamePlaySetupSubject;

        protected GameEngineClientProxyBase()
        {
            _territorySelectorSubject = new Subject<ITerritorySelector>();
            _gamePlaySetupSubject = new Subject<IGamePlaySetup>();

            OnSelectRegion = _territorySelectorSubject.AsObservable();
            OnGamePlaySetup = _gamePlaySetupSubject.AsObservable();
        }

        public IObservable<IGamePlaySetup> OnGamePlaySetup { get; }

        public IObservable<ITerritorySelector> OnSelectRegion { get; }
    }
}