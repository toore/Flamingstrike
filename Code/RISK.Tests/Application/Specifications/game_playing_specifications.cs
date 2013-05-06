﻿using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Base.Extensions;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;
using StructureMap;
using WindowManager = Caliburn.Micro.WindowManager;

namespace RISK.Tests.Application.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        private ILocationProvider _locationProvider;
        private IPlayer _player1;
        private IPlayer _player2;
        private IMainGameViewModel _mainGameBoardViewModel;
        private IWorldMap _worldMap;
        private PlayerRepository _playerRepository;
        private IWindowManager _windowManager;

        public void before_all()
        {
            ObjectFactory.Configure(x =>
                {
                    x.For<IMainGameViewModel>().Use<MainGameViewModel>();
                    x.For<IGameSettingsViewModel>().Use<GameSettingsViewModel>();
                    x.For<IPlayerFactory>().Use<PlayerFactory>();
                    x.For<IPlayerTypes>().Use<PlayerTypes>();
                    x.For<IGameSettingsEventAggregator>().Use<GameSettingsEventAggregator>();
                    x.For<IGameboardViewModelFactory>().Use<GameboardViewModelFactory>();
                    x.For<IGameFactoryWorker>().Use<GameFactoryWorker>();
                    x.For<IGameFactory>().Use<GameFactory>();
                    x.For<ITurnFactory>().Use<TurnFactory>();
                    x.For<IAlternateGameSetup>().Use<AlternateGameSetup>();
                    x.For<IRandomSorter>().Use<RandomSorter>();
                    x.For<IRandomWrapper>().Use<RandomWrapper>();
                    x.For<IMainGameViewModel>().Use<MainGameViewModel>();
                    x.For<IWorldMapFactory>().Use<WorldMapFactory>();
                    x.For<IWorldMapViewModelFactory>().Use<WorldMapViewModelFactory>();
                    x.For<ITerritoryViewModelFactory>().Use<TerritoryViewModelFactory>();
                    x.For<ITerritoryViewModelUpdater>().Use<TerritoryViewModelUpdater>();
                    x.For<ITerritoryColorsFactory>().Use<TerritoryColorsFactory>();
                    x.For<IColorService>().Use<ColorService>();
                    x.For<ITerritoryGuiFactory>().Use<TerritoryGuiFactory>();
                    x.For<ITerritoryTextViewModelFactory>().Use<TerritoryTextViewModelFactory>();
                    x.For<ICardFactory>().Use<CardFactory>();
                    x.For<IInitialArmyCountProvider>().Use<InitialArmyCountProvider>();
                    x.For<IBattleCalculator>().Use<BattleCalculator>();
                    x.For<IDices>().Use<Dices>();
                    x.For<ICasualtyEvaluator>().Use<CasualtyEvaluator>();
                    x.For<IDiceRoller>().Use<DiceRoller>();
                    x.For<IGameSetupViewModelFactory>().Use<GameSetupViewModelFactory>();
                    x.For<IInputRequestHandler>().Use<InputRequestHandler>();
                    x.For<IGameOverEvaluater>().Use<GameOverEvaluater>();
                    x.For<IWindowManager>().Use<WindowManager>();
                    x.For<IGameOverViewModelFactory>().Use<GameOverViewModelFactory>();

                    x.RegisterInterceptor(new HandleInterceptor<IGameSettingsEventAggregator>());
                });
        }

        public void selecting_North_Africa_and_attacking_Brazil_and_win_moves_armies_into_territory_and_flags_that_user_should_receive_a_card_when_turn_ends()
        {
            before = () =>
                {
                    InjectPlayerRepository();
                    InjectLocationProvider();
                    InjectWorldMapFactory();
                    InjectDiceRollerWithReturningSixFiveFourAndThenFiveForTwoAttacks();
                    InjectWindowManager();

                    _mainGameBoardViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

                    SelectTwoHumanPlayersAndConfirm();

                    _player1 = _playerRepository.GetAll().First();
                    _player2 = _playerRepository.GetAll().Second();

                    PlaceArmies();

                    PlayerOneOccupiesNorthAfricaWithFiveArmies();
                    PlayerTwoOccupiesBrazilAndVenezuela();
                    PlayerOneOccupiesEveryTerritoryExceptBrazilVenezuelaAndNorthAfrica();
                };

            act = () =>
                {
                    ClickOn(_locationProvider.NorthAfrica);
                    ClickOn(_locationProvider.Brazil);
                };

            it["when player 1 attacks Brazil from North Africa"] = () =>
                {
                    _worldMap.GetTerritory(_locationProvider.NorthAfrica).AssignedPlayer.Should().Be(_player1, "player 1 should occupy North Africa");
                    _worldMap.GetTerritory(_locationProvider.NorthAfrica).Armies.Should().Be(1, "North Africa should have 1 army");
                    _worldMap.GetTerritory(_locationProvider.Brazil).AssignedPlayer.Should().Be(_player1, "player 1 should occupy Brazil");
                    GetTerritoryViewModel(_locationProvider.Brazil).IsSelected.Should().BeTrue("selected territory should be Brazil");
                    _worldMap.GetTerritory(_locationProvider.Brazil).Armies.Should().Be(4, "Brazil should have 4 armies");
                };

            context["when player 1 turn ends"] = () =>
                {
                    act = () => { EndTurn(); };

                    it["player 1 should have a card when turn ends"] = () => _player1.Cards.Count().Should().Be(1);
                };

            context["when player 1 attacks again"] = () =>
                {
                    act = () => ClickOn(_locationProvider.Venezuela);

                    it["player 1 should occupy Venezuela with 3 armies"] = () =>
                        {
                            _worldMap.GetTerritory(_locationProvider.Venezuela).AssignedPlayer.Should().Be(_player1, "player 1 should occupy Venezuela");
                            _worldMap.GetTerritory(_locationProvider.Venezuela).Armies.Should().Be(3, "Venezuela should have 3 armies");

                            //TODO
                            //_windowManager.Received().ShowDialog()
                        };
                };
        }

        private void PlaceArmies()
        {
            const int numberOfArmiesToPlace = (40 - 21) * 2;

            for (int i = 0; i < numberOfArmiesToPlace; i++)
            {
                var gameSetupViewModel = (GameSetupViewModel)_mainGameBoardViewModel.MainViewModel;
                var firstEnabledterritoryViewModel = gameSetupViewModel.WorldMapViewModel.WorldMapViewModels
                    .OfType<TerritoryLayoutViewModel>()
                    .First(x => x.IsEnabled);

                gameSetupViewModel.SelectLocation(firstEnabledterritoryViewModel.Location);
            }
        }

        private void EndTurn()
        {
            var gameboardViewModel = (IGameboardViewModel)_mainGameBoardViewModel.MainViewModel;
            gameboardViewModel.EndTurn();
        }

        private void SelectTwoHumanPlayersAndConfirm()
        {
            var gameSetupViewModel = (IGameSettingsViewModel)_mainGameBoardViewModel.MainViewModel;

            gameSetupViewModel.Players.First().IsEnabled = true;
            gameSetupViewModel.Players.Second().IsEnabled = true;

            gameSetupViewModel.Confirm();
        }

        private void PlayerOneOccupiesNorthAfricaWithFiveArmies()
        {
            UpdateTerritory(_locationProvider.NorthAfrica, _player1, 5);
        }

        private void PlayerTwoOccupiesBrazilAndVenezuela()
        {
            UpdateTerritory(_locationProvider.Brazil, _player2, 1);
            UpdateTerritory(_locationProvider.Venezuela, _player2, 1);
        }

        private void UpdateTerritory(ILocation location, IPlayer owner, int armies)
        {
            var territory = _worldMap.GetTerritory(location);
            territory.AssignedPlayer = owner;
            territory.Armies = armies;
        }

        private void PlayerOneOccupiesEveryTerritoryExceptBrazilVenezuelaAndNorthAfrica()
        {
            var excludedTerritories = new[]
                {
                    _locationProvider.Brazil,
                    _locationProvider.Venezuela,
                    _locationProvider.NorthAfrica
                };

            _locationProvider.GetAll()
                .Where(x => !excludedTerritories.Contains(x))
                .Select(x => _worldMap.GetTerritory(x))
                .Apply(x =>
                    {
                        x.AssignedPlayer = _player1;
                        x.Armies = 1;
                    });
        }

        private void ClickOn(ILocation location)
        {
            GetTerritoryViewModel(location)
                .OnClick();
        }

        private ITerritoryLayoutViewModel GetTerritoryViewModel(ILocation location)
        {
            var gameboardViewModel = (IGameboardViewModel)_mainGameBoardViewModel.MainViewModel;

            return gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryLayoutViewModel>()
                .Single(x => x.Location == location);
        }

        private void InjectPlayerRepository()
        {
            _playerRepository = new PlayerRepository();
            ObjectFactory.Inject<IPlayerRepository>(_playerRepository);
        }

        private void InjectLocationProvider()
        {
            _locationProvider = new LocationProvider(new ContinentProvider());
            ObjectFactory.Inject(_locationProvider);
        }

        private void InjectWorldMapFactory()
        {
            _worldMap = new WorldMap(_locationProvider);

            var worldMapFactory = Substitute.For<IWorldMapFactory>();
            worldMapFactory.Create().Returns(_worldMap);

            ObjectFactory.Inject(worldMapFactory);
        }

        private void InjectDiceRollerWithReturningSixFiveFourAndThenFiveForTwoAttacks()
        {
            var diceRoller = Substitute.For<IDiceRoller>();
            diceRoller.Roll().Returns(
                DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five,
                DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five
                );
            ObjectFactory.Inject(diceRoller);
        }

        public void InjectWindowManager()
        {
            _windowManager = Substitute.For<IWindowManager>();
            ObjectFactory.Inject(_windowManager);
        }
    }
}