using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public abstract class GameEngineClientBase : IGameEngineClient
    {
        protected readonly ISubject<ITerritorySelector> _territorySelectorSubject;
        protected readonly ISubject<IGamePlaySetup> _gamePlaySetupSubject;
        protected readonly ISubject<IDraftArmiesPhase> _draftArmiesPhaseSubject;
        protected readonly ISubject<IAttackPhase> _attackPhaseSubject;
        protected readonly ISubject<ISendArmiesToOccupyPhase> _sendArmiesToOccupyPhaseSubject;
        protected readonly ISubject<IEndTurnPhase> _endTurnPhaseSubject;
        protected readonly ISubject<IGameOverState> _gameOverStateSubject;

        protected GameEngineClientBase()
        {
            _territorySelectorSubject = new Subject<ITerritorySelector>();
            _gamePlaySetupSubject = new Subject<IGamePlaySetup>();
            _draftArmiesPhaseSubject = new Subject<IDraftArmiesPhase>();
            _attackPhaseSubject = new Subject<IAttackPhase>();
            _sendArmiesToOccupyPhaseSubject = new Subject<ISendArmiesToOccupyPhase>();
            _endTurnPhaseSubject = new Subject<IEndTurnPhase>();
            _gameOverStateSubject = new Subject<IGameOverState>();

            OnSelectRegion = _territorySelectorSubject.AsObservable();
            OnGamePlaySetup = _gamePlaySetupSubject.AsObservable();
            OnDraftArmies = _draftArmiesPhaseSubject.AsObservable();
            OnAttack = _attackPhaseSubject.AsObservable();
            OnSendArmiesToOccupy = _sendArmiesToOccupyPhaseSubject.AsObservable();
            OnEndTurn = _endTurnPhaseSubject.AsObservable();
            OnGameOver = _gameOverStateSubject.AsObservable();
        }

        public IObservable<IGamePlaySetup> OnGamePlaySetup { get; }
        public IObservable<ITerritorySelector> OnSelectRegion { get; }

        public IObservable<IDraftArmiesPhase> OnDraftArmies { get; }
        public IObservable<IAttackPhase> OnAttack { get; }
        public IObservable<ISendArmiesToOccupyPhase> OnSendArmiesToOccupy { get; }
        public IObservable<IEndTurnPhase> OnEndTurn { get; }
        public IObservable<IGameOverState> OnGameOver { get; }

        public abstract void Setup(IEnumerable<string> players);

        public abstract void StartGame(IGamePlaySetup gamePlaySetup);
    }
}