using System;
using System.Linq;
using NSpec;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;

namespace RISK.Tests.GamePlaying
{
    internal class game_playing_specifications : nspec
    {
        private void attacking_an_area_and_winning_moves_armies_into_area_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            //Given.
            //   new_game_with(humanUsers: 2).
            //   first_user_deploys_armies_in_area(NorthAfrica, 5).
            //   second_user_deploys_armies_in_available_areas(1).
            //   deployment_phase_is_finished();

            //When.User.
            //    selects_area(NorthAfrica).
            //    attacks_area(Brazil).
            //    attacker_wins();

            //Then.
            //    first_user_should_have_armies_in_area(NorthAfrica, 1).
            //    first_user_should_have_armies_in_area(Brazil, 4).
            //    first_user_should_receive_a_card_when_turn_ends();

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