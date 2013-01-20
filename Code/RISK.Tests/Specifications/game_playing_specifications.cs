using System;
using System.Linq;
using FluentAssertions;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;
using StructureMap;

namespace RISK.Tests.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        private IAreaDefinitionProvider _areaDefinitionProvider;
        private HumanPlayer _player1;
        private HumanPlayer _player2;
        private Game _game;
        private WorldMap _worldMap;
        private ITurn _currentTurn;

        public void before_all()
        {
            Configure();

            _areaDefinitionProvider = ObjectFactory.GetInstance<IAreaDefinitionProvider>();
        }

        public void Configure()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.For<IContinentProvider>().Use<ContinentProvider>();
                    x.For<IAreaDefinitionProvider>().Use<AreaDefinitionProvider>();
                });
        }

        public void attacking_an_area_and_winning_moves_armies_into_area_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            before = () =>
                {
                    _player1 = new HumanPlayer();
                    _player2 = new HumanPlayer();

                    _game = new Game();

                    _worldMap = new WorldMap();

                    UpdateArea(_areaDefinitionProvider.NorthAfrica, _player1, 5);
                    UpdateAllAreasWithoutOwner(_player2, 1);

                    // warEvaluater - user1 should always win

                    _currentTurn = _game.GetNextTurn();
                };

            act = () =>
                {
                    _currentTurn.SelectArea(_areaDefinitionProvider.NorthAfrica);
                    _currentTurn.AttackArea(_areaDefinitionProvider.Brazil);
                };

            it["user 1 should own North Africa"] = () => _worldMap.GetArea(_areaDefinitionProvider.NorthAfrica).Owner.Should().Be(_player1);
            it["North Africa should have 1 army"] = () => _worldMap.GetArea(_areaDefinitionProvider.NorthAfrica).Armies.Should().Be(1);
            it["user 1 should own Brazil"] = () => _worldMap.GetArea(_areaDefinitionProvider.Brazil).Owner.Should().Be(_player1);
            it["Brazil should have 4 armies"] = () => _worldMap.GetArea(_areaDefinitionProvider.Brazil).Armies.Should().Be(4);
            it["user 1 should receive a card when turn ends"] = () => _currentTurn.PlayerShouldReceiveCardWhenTurnEnds();
        }

        private void UpdateArea(IAreaDefinition areaDefinition, HumanPlayer owner, int armies)
        {
            var fakeArea = _worldMap.GetArea(areaDefinition);
            fakeArea.Owner = owner;
            fakeArea.Armies = armies;
        }

        private void UpdateAllAreasWithoutOwner(HumanPlayer owner, int armies)
        {
            var fakeAreas = _areaDefinitionProvider.GetAll().Select(x => _worldMap.GetArea(x)).Where(x => x.HasOwner);

            throw new NotImplementedException();
        }
    }
}