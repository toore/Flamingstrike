﻿using System.Linq;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using StructureMap;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class game_setup
    {
        private Locations _locations;
        private IPlayer _player1;
        private IPlayer _player2;
        private IMainGameViewModel _mainGameViewModel;
        private IWorldMap _worldMap;
        private Players _players;

        public game_setup()
        {
            new PluginConfiguration().Configure();
        }

        [Fact]
        public void game_is_started()
        {
            Given
                .a_new_game();

            When
                .two_human_players_are_confirmed()
                .all_armies_are_placed_on_the_map();

            Then
                .the_game_is_started();
        }

        private game_setup a_new_game()
        {
            InjectPlayerRepository();
            InjectLocationProvider();
            InjectWorldMapFactory();

            _mainGameViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

            return this;
        }

        private game_setup two_human_players_are_confirmed()
        {
            var gameSetupViewModel = (IGameSettingsViewModel)_mainGameViewModel.MainViewModel;

            gameSetupViewModel.Players.First().IsEnabled = true;
            gameSetupViewModel.Players.Second().IsEnabled = true;

            gameSetupViewModel.Confirm();

            return this;
        }

        private game_setup all_armies_are_placed_on_the_map()
        {
            _mainGameViewModel.MainViewModel.Should().BeOfType<GameSetupViewModel>();

            const int numberOfArmiesToPlace = (40 - 21) * 2;

            for (int i = 0; i < numberOfArmiesToPlace; i++)
            {
                var gameSetupViewModel = (GameSetupViewModel)_mainGameViewModel.MainViewModel;
                var firstEnabledterritoryViewModel = gameSetupViewModel.WorldMapViewModel.WorldMapViewModels
                    .OfType<TerritoryLayoutViewModel>()
                    .First(x => x.IsEnabled);

                gameSetupViewModel.SelectLocation(firstEnabledterritoryViewModel.Location);
            }

            return this;
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.MainViewModel.Should().BeOfType<GameboardViewModel>();
        }

        private void InjectPlayerRepository()
        {
            _players = new Players();
            ObjectFactory.Inject<IPlayers>(_players);
        }

        private void InjectLocationProvider()
        {
            _locations = new Locations(new Continents());
            ObjectFactory.Inject(_locations);
        }

        private void InjectWorldMapFactory()
        {
            _worldMap = new WorldMap(_locations);

            var worldMapFactory = Substitute.For<IWorldMapFactory>();
            worldMapFactory.Create().Returns(_worldMap);

            ObjectFactory.Inject(worldMapFactory);
        }

        private game_setup Given
        {
            get { return this; }
        }

        private game_setup When
        {
            get { return this; }
        }

        private game_setup Then
        {
            get { return this; }
        }
    }
}