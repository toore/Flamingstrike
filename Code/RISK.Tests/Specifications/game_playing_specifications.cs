using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.WorldMapViewModels;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using RISK.Domain.Repositories;
using Rhino.Mocks;
using StructureMap;

namespace RISK.Tests.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        private ILocationRepository _locationRepository;
        private HumanPlayer _player1;
        private HumanPlayer _player2;
        private IMainViewModel _mainViewModel;
        private IWorldMap _worldMap;

        public void before_all()
        {
            new PluginConfiguration().Configure();
        }

        public void selecting_North_Africa_and_attacking_Brazil_and_win_moves_armies_into_territory_and_flags_that_user_should_receive_a_card_when_turn_ends()
        {
            before = () =>
                {
                    InjectPlayerRepositoryWithTwoPlayers();
                    InjectLocationRepository();
                    InjectWorldMap();
                    InjectBattleCalculatorWithAttackingFiveDefendingOneDefenderLosesOne();

                    _mainViewModel = ObjectFactory.GetInstance<IMainViewModel>();

                    PlayerOneOccupiesNorthAfricaWithFiveArmies();
                    PlayerTwoOccupiesEveryFreTerritoryWithOneArmy();
                };

            act = () =>
                {
                    ClickOn(_locationRepository.NorthAfrica);
                    ClickOn(_locationRepository.Brazil);
                };

            it["player 1 should occupy North Africa"] = () => _worldMap.GetTerritory(_locationRepository.NorthAfrica).Owner.Should().Be(_player1);
            it["North Africa should have 1 army"] = () => _worldMap.GetTerritory(_locationRepository.NorthAfrica).Armies.Should().Be(1);
            it["player 1 should occupy Brazil"] = () => _worldMap.GetTerritory(_locationRepository.Brazil).Owner.Should().Be(_player1);
            it["Brazil should have 4 armies"] = () => _worldMap.GetTerritory(_locationRepository.Brazil).Armies.Should().Be(4);
            //it["player 1 should receive a card when turn ends"] = () => _currentTurn.PlayerShouldReceiveCardWhenTurnEnds();
        }

        private void InjectWorldMap()
        {
            _worldMap = new WorldMap(_locationRepository);
            ObjectFactory.Inject(_worldMap);
        }

        private void PlayerOneOccupiesNorthAfricaWithFiveArmies()
        {
            UpdateTerritory(_locationRepository.NorthAfrica, _player1, 5);
        }

        private void PlayerTwoOccupiesEveryFreTerritoryWithOneArmy()
        {
            UpdateAllTerritoriesWithoutOwner(_player2, 1);
        }

        private void ClickOn(ILocation territory)
        {
            _mainViewModel.WorldMapViewModel.WorldMapViewModels
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

        private void InjectLocationRepository()
        {
            _locationRepository = new LocationRepository(new ContinentRepository());
            ObjectFactory.Inject(_locationRepository);
        }

        private void InjectPlayerRepositoryWithTwoPlayers()
        {
            var playerRepository = new PlayerRepository();
            ObjectFactory.Inject<IPlayerRepository>(playerRepository);

            _player1 = new HumanPlayer("player 1");
            _player2 = new HumanPlayer("player 2");

            playerRepository.Add(_player1);
            playerRepository.Add(_player2);
        }

        private void UpdateTerritory(ILocation location, IPlayer owner, int armies)
        {
            var territory = _worldMap.GetTerritory(location);
            territory.Owner = owner;
            territory.Armies = armies;
        }

        private void UpdateAllTerritoriesWithoutOwner(IPlayer owner, int armies)
        {
            _locationRepository.GetAll()
                .Select(x => _worldMap.GetTerritory(x))
                .Where(x => !x.HasOwner)
                .Apply(x =>
                    {
                        x.Owner = owner;
                        x.Armies = armies;
                    });
        }
    }
}