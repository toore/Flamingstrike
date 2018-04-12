using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public class SendArmiesToOccupyPhase : ISendArmiesToOccupyPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;

        public SendArmiesToOccupyPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players,
            IDeck deck,
            Region attackingRegion,
            Region occupiedRegion,
            ITerritoryOccupier territoryOccupier)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _territoryOccupier = territoryOccupier;
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
            var updatedTerritories = _territoryOccupier.SendInAdditionalArmiesToOccupy(Territories, AttackingRegion, OccupiedRegion, numberOfArmies);
            var updatedGameData = new GameData(updatedTerritories, Players, CurrentPlayerName, Deck);

            _gamePhaseConductor.ContinueWithAttackPhase(
                ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                updatedGameData);
        }
    }
}