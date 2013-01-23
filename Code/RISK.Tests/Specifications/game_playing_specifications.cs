using System.Linq;
using FluentAssertions;
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
        private IAreaDefinitionRepository _areaDefinitionRepository;
        private HumanPlayer _player1;
        private HumanPlayer _player2;
        private IGame _game;
        private IWorldMap _worldMap;
        private ITurn _currentTurn;
        private IPlayerRepository _playerRepository;
        private IBattleEvaluater _battleEvaluater;

        public void before_all()
        {
            PluginConfiguration.Configure();
        }

        public void attacking_an_area_and_winning_moves_armies_into_area_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            before = () =>
                {
                    _playerRepository = new PlayerRepository();
                    ObjectFactory.Inject(_playerRepository);

                    _areaDefinitionRepository = new AreaDefinitionRepository(new ContinentRepository());
                    ObjectFactory.Inject(_areaDefinitionRepository);

                    var dices = MockRepository.GenerateStub<IDices>();
                    _battleEvaluater = new BattleEvaluater(dices);
                    ObjectFactory.Inject(_battleEvaluater);

                    _player1 = new HumanPlayer("player 1");
                    _player2 = new HumanPlayer("player 2");

                    _playerRepository.Add(_player1);
                    _playerRepository.Add(_player2);

                    _game = ObjectFactory.GetInstance<IGame>();
                    _worldMap = _game.GetWorldMap();

                    UpdateArea(_areaDefinitionRepository.NorthAfrica, _player1, 5);
                    UpdateAllAreasWithoutOwner(_player2, 1);

                    _currentTurn = _game.GetNextTurn();
                };

            act = () =>
                {
                    _currentTurn.SelectArea(_areaDefinitionRepository.NorthAfrica);
                    _currentTurn.AttackArea(_areaDefinitionRepository.Brazil);
                };

            it["player 1 should own North Africa"] = () => _worldMap.GetArea(_areaDefinitionRepository.NorthAfrica).Owner.Should().Be(_player1);
            it["North Africa should have 1 army"] = () => _worldMap.GetArea(_areaDefinitionRepository.NorthAfrica).Armies.Should().Be(1);
            it["player 1 should own Brazil"] = () => _worldMap.GetArea(_areaDefinitionRepository.Brazil).Owner.Should().Be(_player1);
            it["Brazil should have 4 armies"] = () => _worldMap.GetArea(_areaDefinitionRepository.Brazil).Armies.Should().Be(4);
            it["player 1 should receive a card when turn ends"] = () => _currentTurn.PlayerShouldReceiveCardWhenTurnEnds();
        }

        private void UpdateArea(IAreaDefinition areaDefinition, IPlayer owner, int armies)
        {
            var area = _worldMap.GetArea(areaDefinition);
            area.Owner = owner;
            area.Armies = armies;
        }

        private void UpdateAllAreasWithoutOwner(IPlayer owner, int armies)
        {
            _areaDefinitionRepository.GetAll()
                .Select(x => _worldMap.GetArea(x))
                .Where(x => !x.HasOwner)
                .Apply(x =>
                    {
                        x.Owner = owner;
                        x.Armies = armies;
                    });
        }
    }
}