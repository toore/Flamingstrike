using System.Linq;
using FluentAssertions;
using GuiWpf.Infrastructure;
using RISK.Domain;
using RISK.Domain.Caliburn.Micro;
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
        private IGame _game;
        private IWorldMap _worldMap;
        private ITurn _currentTurn;
        private IPlayerRepository _playerRepository;
        private IBattleCalculator _battleCalculator;

        public void before_all()
        {
            PluginConfiguration.Configure();
        }

        public void attacking_an_territory_and_winning_moves_armies_into_territory_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            before = () =>
                {
                    _playerRepository = new PlayerRepository();
                    ObjectFactory.Inject(_playerRepository);

                    _locationRepository = new LocationRepository(new ContinentRepository());
                    ObjectFactory.Inject(_locationRepository);

                    var dices = MockRepository.GenerateStub<IDices>();
                    var diceResult = MockRepository.GenerateStub<IDicesResult>();
                    diceResult.Stub(x => x.DefenderCasualties).Return(1);
                    dices.Stub(x => x.Roll(5, 1)).Return(diceResult);
                    
                    _battleCalculator = new BattleCalculator(dices);
                    ObjectFactory.Inject(_battleCalculator);

                    _player1 = new HumanPlayer("player 1");
                    _player2 = new HumanPlayer("player 2");

                    _playerRepository.Add(_player1);
                    _playerRepository.Add(_player2);

                    _game = ObjectFactory.GetInstance<IGame>();
                    _worldMap = _game.GetWorldMap();

                    UpdateTerritory(_locationRepository.NorthAfrica, _player1, 5);
                    UpdateAllTerritoriesWithoutOwner(_player2, 1);

                    _currentTurn = _game.GetNextTurn();
                };

            act = () =>
                {
                    _currentTurn.Select(_locationRepository.NorthAfrica);
                    _currentTurn.Attack(_locationRepository.Brazil);
                };

            it["player 1 should own North Africa"] = () => _worldMap.GetTerritory(_locationRepository.NorthAfrica).Owner.Should().Be(_player1);
            it["North Africa should have 1 army"] = () => _worldMap.GetTerritory(_locationRepository.NorthAfrica).Armies.Should().Be(1);
            it["player 1 should own Brazil"] = () => _worldMap.GetTerritory(_locationRepository.Brazil).Owner.Should().Be(_player1);
            it["Brazil should have 4 armies"] = () => _worldMap.GetTerritory(_locationRepository.Brazil).Armies.Should().Be(4);
            it["player 1 should receive a card when turn ends"] = () => _currentTurn.PlayerShouldReceiveCardWhenTurnEnds();
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