﻿using Caliburn.Micro;
using GuiWpf.ViewModels.AlternateSetup;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Preparation;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<StartGameSetupMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IUserInteractorFactory _userInteractorFactory;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModel()
            : this(new Root()) {}

        protected MainGameViewModel(Root root)
            : this(
                root.PlayerRepository,
                root.AlternateGameSetupFactory,
                root.GamePreparationViewModelFactory,
                root.GameboardViewModelFactory,
                root.AlternateGameSetupViewModelFactory,
                root.UserInteractorFactory)
        {
            root.EventAggregator.Subscribe(this);
        }

        protected MainGameViewModel(
            IPlayerRepository playerRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IUserInteractorFactory userInteractorFactory)
        {
            _playerRepository = playerRepository;
            _alternateGameSetupFactory = alternateGameSetupFactory;
            _gamePreparationViewModelFactory = gamePreparationViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _alternateGameSetupViewModelFactory = alternateGameSetupViewModelFactory;
            _userInteractorFactory = userInteractorFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeNewGame();
        }

        public override string DisplayName
        {
            get { return "R!SK"; }
            set { }
        }

        public void Handle(NewGameMessage newGameMessage)
        {
            InitializeNewGame();
        }

        public void Handle(StartGameSetupMessage startGameSetupMessage)
        {
            StartGameSetup();
        }

        public void Handle(StartGameplayMessage startGameplayMessage)
        {
            StartGamePlay(startGameplayMessage.Game);
        }

        private void InitializeNewGame()
        {
            var gamePreparationViewModel = _gamePreparationViewModelFactory.Create();

            ActivateItem(gamePreparationViewModel);
        }

        private void StartGameSetup()
        {
            var players = _playerRepository.GetAll();
            var alternateGameSetup = _alternateGameSetupFactory.Create(players);
            var gameSetupViewModel = _alternateGameSetupViewModelFactory.Create(alternateGameSetup);

            var userInteractor = _userInteractorFactory.Create(gameSetupViewModel);
            alternateGameSetup.TerritoryResponder = userInteractor;

            ActivateItem(gameSetupViewModel);
        }

        private void StartGamePlay(IGame game)
        {
            var gameboardViewModel = _gameboardViewModelFactory.Create(game);

            ActivateItem(gameboardViewModel);
        }
    }
}