using System;
using System.Linq;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;

namespace RISK.Tests.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        public void attacking_an_area_and_winning_moves_armies_into_area_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            before = () =>
                {
                    _user1 = new HumanUser();
                    _user2 = new HumanUser();

                    _game = new Game();

                    _worldMap = new WorldMap();

                    UpdateArea(_areaDefinitionProvider.NorthAfrica, _user1, 5);
                    UpdateAllAreasWithoutOwner(_user2, 1);

                    // warEvaluater - user1 should always win

                    _currentTurn = _game.GetNextTurn();
                };

            act = () =>
                {
                    _currentTurn.SelectArea(_areaDefinitionProvider.NorthAfrica);
                    _currentTurn.AttackArea(_areaDefinitionProvider.Brazil);
                };

            it["user 1 should have 1 army in North Africa"] = todo;
            it["user 1 should have 4 armies in Brazil"] = todo;
            it["user 1 should receive a card when turn ends"] = todo;
        }

        private void UpdateArea(IAreaDefinition areaDefinition, HumanUser owner, int armies)
        {
            var fakeArea = _worldMap.GetArea(areaDefinition);
            fakeArea.Owner = owner;
            fakeArea.Armies = armies;
        }

        private void UpdateAllAreasWithoutOwner(HumanUser owner, int armies)
        {
            var fakeAreas = _areaDefinitionProvider.GetAll().Select(x => _worldMap.GetArea(x)).Where(x => x.HasOwner);

            throw new NotImplementedException();
        }

        private IAreaDefinitionProvider _areaDefinitionProvider;
        private HumanUser _user1;
        private HumanUser _user2;
        private Game _game;
        private WorldMap _worldMap;
        private ITurn _currentTurn;
    }
}