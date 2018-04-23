using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class SendArmiesToOccupyPhase : ISendArmiesToOccupyPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;

        public SendArmiesToOccupyPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players,
            IDeck deck,
            Region attackingRegion,
            Region occupiedRegion)
        {
            _gamePhaseConductor = gamePhaseConductor;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            Deck = deck;
            AttackingRegion = attackingRegion;
            OccupiedRegion = occupiedRegion;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IDeck Deck { get; }
        public Region AttackingRegion { get; }
        public Region OccupiedRegion { get; }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            var attackingTerritory = Territories.Single(x => x.Region == AttackingRegion);
            var occupiedTerritory = Territories.Single(x => x.Region == OccupiedRegion);

            attackingTerritory.RemoveArmies(numberOfArmies);
            occupiedTerritory.AddArmies(numberOfArmies);

            _gamePhaseConductor.ContinueWithAttackPhase(ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory);
        }
    }
}