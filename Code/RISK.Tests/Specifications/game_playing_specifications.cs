using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.GuiDefinitions;
using GuiWpf.Infrastructure;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using RISK.Domain.Repositories;
using Rhino.Mocks;
using StructureMap;

namespace RISK.Tests.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        private ILocationProvider _locationProvider;
        private IPlayer _player1;
        private IPlayer _player2;
        private IMainGameViewModel _mainGameBoardViewModel;
        private IWorldMap _worldMap;
        private PlayerRepository _playerRepository;

        public void before_all()
        {
            ObjectFactory.Configure(x =>
                {
                    x.For<IMainGameViewModel>().Use<MainGameViewModel>();
                    x.For<IGameSetupViewModel>().Use<GameSetupViewModel>();
                    x.For<IPlayerFactory>().Use<PlayerFactory>();
                    x.For<IPlayerTypes>().Use<PlayerTypes>();
                    x.For<IGameSetupEventAggregator>().Use<GameSetupEventAggregator>();
                    x.For<IGameboardViewModelFactory>().Use<GameboardViewModelFactory>();
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
                    x.For<ITerritoryGuiDefinitionFactory>().Use<TerritoryGuiDefinitionFactory>();
                    x.For<ITextViewModelFactory>().Use<TextViewModelFactory>();

                    x.RegisterInterceptor(new HandleInterceptor<IGameSetupEventAggregator>());
                });
        }

        public void selecting_North_Africa_and_attacking_Brazil_and_win_moves_armies_into_territory_and_flags_that_user_should_receive_a_card_when_turn_ends()
        {
            before = () =>
                {
                    InjectPlayerRepository();
                    InjectLocationProvider();
                    InjectWorldMap();
                    InjectBattleCalculatorWithAttackingFiveDefendingOneDefenderLosesOne();

                    _mainGameBoardViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

                    SelectTwoHumanPlayersAndConfirm();

                    _player1 = _playerRepository.GetAll().First();
                    _player2 = _playerRepository.GetAll().Second();

                    PlayerOneOccupiesNorthAfricaWithFiveArmies();
                    PlayerTwoOccupiesEveryUnoccupiedTerritoryWithOneArmy();
                };

            act = () =>
                {
                    ClickOn(_locationProvider.NorthAfrica);
                    ClickOn(_locationProvider.Brazil);
                };

            it["player 1 should occupy North Africa"] = () => _worldMap.GetTerritory(_locationProvider.NorthAfrica).Owner.Should().Be(_player1);
            it["North Africa should have 1 army"] = () => _worldMap.GetTerritory(_locationProvider.NorthAfrica).Armies.Should().Be(1);
            it["player 1 should occupy Brazil"] = () => _worldMap.GetTerritory(_locationProvider.Brazil).Owner.Should().Be(_player1);
            it["Brazil should have 4 armies"] = () => _worldMap.GetTerritory(_locationProvider.Brazil).Armies.Should().Be(4);
            it["player 1 should have a card when turn ends"] = () => _player1.Cards.Count().Should().Be(1);
        }

        private void InjectWorldMap()
        {
            _worldMap = new WorldMap(_locationProvider);
            ObjectFactory.Inject(_worldMap);
        }

        private void SelectTwoHumanPlayersAndConfirm()
        {
            var gameSetupViewModel = (IGameSetupViewModel)_mainGameBoardViewModel.MainViewModel;

            gameSetupViewModel.Players.First().IsEnabled = true;
            gameSetupViewModel.Players.Second().IsEnabled = true;

            gameSetupViewModel.Confirm();
        }

        private void PlayerOneOccupiesNorthAfricaWithFiveArmies()
        {
            UpdateTerritory(_locationProvider.NorthAfrica, _player1, 5);
        }

        private void PlayerTwoOccupiesEveryUnoccupiedTerritoryWithOneArmy()
        {
            UpdateAllTerritoriesWithoutOwner(_player2, 1);
        }

        private void ClickOn(ILocation territory)
        {
            var gameboardViewModel = (IGameboardViewModel)_mainGameBoardViewModel.MainViewModel;

            gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryViewModel>()
                .Single(x => x.Location == territory)
                .OnClick();
        }

        private void InjectBattleCalculatorWithAttackingFiveDefendingOneDefenderLosesOne()
        {
            var dices = MockRepository.GenerateStub<IDices>();
            var diceResult = MockRepository.GenerateStub<IDicesResult>();
            diceResult.Stub(x => x.DefenderCasualties).Return(1);
            dices.Stub(x => x.Roll(5, 1)).Return(diceResult);

            var battleCalculator = new BattleCalculator(dices);
            ObjectFactory.Inject<IBattleCalculator>(battleCalculator);
        }

        private void InjectLocationProvider()
        {
            _locationProvider = new LocationProvider(new ContinentProvider());
            ObjectFactory.Inject(_locationProvider);
        }

        private void InjectPlayerRepository()
        {
            _playerRepository = new PlayerRepository();
            ObjectFactory.Inject<IPlayerRepository>(_playerRepository);
        }

        private void UpdateTerritory(ILocation location, IPlayer owner, int armies)
        {
            var territory = _worldMap.GetTerritory(location);
            territory.Owner = owner;
            territory.Armies = armies;
        }

        private void UpdateAllTerritoriesWithoutOwner(IPlayer owner, int armies)
        {
            _locationProvider.GetAll()
                .Select(x => _worldMap.GetTerritory(x))
                .Where(x => !x.HasOwner())
                .Apply(x =>
                    {
                        x.Owner = owner;
                        x.Armies = armies;
                    });
        }
    }
}