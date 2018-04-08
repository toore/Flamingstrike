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
            IReadOnlyList<IPlayerGameData> playerGameDatas,
            IDeck deck,
            IRegion attackingRegion,
            IRegion occupiedRegion,
            ITerritoryOccupier territoryOccupier)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _territoryOccupier = territoryOccupier;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            Deck = deck;
            AttackingRegion = attackingRegion;
            OccupiedRegion = occupiedRegion;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public IDeck Deck { get; }
        public IRegion AttackingRegion { get; }
        public IRegion OccupiedRegion { get; }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            var updatedTerritories = _territoryOccupier.SendInAdditionalArmiesToOccupy(Territories, AttackingRegion, OccupiedRegion, numberOfArmies);
            var updatedGameData = new GameData(updatedTerritories, PlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.ContinueWithAttackPhase(
                ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                updatedGameData);
        }
    }
}