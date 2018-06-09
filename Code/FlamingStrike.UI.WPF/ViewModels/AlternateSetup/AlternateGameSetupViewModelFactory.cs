﻿using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModelFactory
    {
        IAlternateGameSetupViewModel Create();
    }

    public class AlternateGameSetupViewModelFactory : IAlternateGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IGameEngineClientProxy _gameEngineClientProxy;

        public AlternateGameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IGameEngineClientProxy gameEngineClientProxy)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _gameEngineClientProxy = gameEngineClientProxy;
        }

        public IAlternateGameSetupViewModel Create()
        {
            return new AlternateGameSetupViewModel(
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                _gameEngineClientProxy);
        }
    }
}