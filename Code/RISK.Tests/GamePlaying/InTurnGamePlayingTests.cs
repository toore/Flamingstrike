using System;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;

namespace RISK.Tests.GamePlaying
{
    [TestFixture]
    public class InTurnGamePlayingTests : GamePlayingTestsBase
    {
        [Test]
        public void Attacking_an_area_and_winning_moves_armies_into_area_and_flags_that_user_should_receive_a_card_when_ending_turn()
        {
            Given.
                new_game_with(humanUsers: 2).
                first_user_deploys_armies_in_area(NorthAfrica, 5).
                second_user_deploys_armies_in_available_areas(1).
                deployment_phase_is_finished();

            When.User.
                selects_area(NorthAfrica).
                attacks_area(Brazil).
                attacker_wins();

            Then.
                first_user_should_have_armies_in_area(NorthAfrica, 1).
                first_user_should_have_armies_in_area(Brazil, 4).
                first_user_should_receive_a_card_when_turn_ends();
        }

        private InTurnGamePlayingTests first_user_deploys_armies_in_area(IArea area, int armies)
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests second_user_deploys_armies_in_available_areas(int armies)
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests deployment_phase_is_finished()
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests selects_area(IArea area)
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests attacks_area(IArea area)
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests attacker_wins()
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests first_user_should_have_armies_in_area(IArea area, int armies)
        {
            throw new NotImplementedException();
        }

        private InTurnGamePlayingTests first_user_should_receive_a_card_when_turn_ends()
        {
            throw new NotImplementedException();
        }
    }
}