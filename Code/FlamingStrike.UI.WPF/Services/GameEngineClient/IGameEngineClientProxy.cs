using System;
using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public interface IGameEngineClientProxy
    {
        IObservable<IGamePlaySetup> OnGamePlaySetup { get; }
        IObservable<ITerritorySelector> OnSelectRegion { get; }

        IObservable<IDraftArmiesPhase> OnDraftArmies { get; }
        IObservable<IAttackPhase> OnAttack { get; }
        IObservable<ISendArmiesToOccupyPhase> OnSendArmiesToOccupy { get; }
        IObservable<IEndTurnPhase> OnEndTurn { get; }
        IObservable<IGameOverState> OnGameOver { get; }

        void Setup(IEnumerable<string> players);
        void StartGame(IGamePlaySetup gamePlaySetup);
    }
}