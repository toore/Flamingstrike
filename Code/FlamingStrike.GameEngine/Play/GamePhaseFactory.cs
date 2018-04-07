using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGamePhaseFactory
    {
        IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, int numberOfArmiesToDraft);
        IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, ConqueringAchievement conqueringAchievement);
        IEndTurnPhase CreateEndTurnPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck);
        ISendArmiesToOccupyPhase CreateSendArmiesToOccupyPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, IRegion attackingRegion, IRegion occupiedRegion);
        IGameOverState CreateGameOverState(PlayerName winner);
    }

    public class GamePhaseFactory : IGamePhaseFactory
    {
        private readonly IArmyDrafter _armyDrafter;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private readonly ITerritoryOccupier _territoryOccupier;

        public GamePhaseFactory(
            IArmyDrafter armyDrafter,
            IAttacker attacker,
            IFortifier fortifier,
            IPlayerEliminationRules playerEliminationRules,
            ITerritoryOccupier territoryOccupier)
        {
            _armyDrafter = armyDrafter;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
            _territoryOccupier = territoryOccupier;
        }

        public IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, int numberOfArmiesToDraft)
        {
            return new DraftArmiesPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                playerGameDatas,
                deck,
                numberOfArmiesToDraft,
                _armyDrafter);
        }

        public IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, ConqueringAchievement conqueringAchievement)
        {
            return new AttackPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                playerGameDatas,
                deck,
                conqueringAchievement,
                _attacker,
                _fortifier,
                _playerEliminationRules);
        }

        public IEndTurnPhase CreateEndTurnPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck)
        {
            return new EndTurnPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                playerGameDatas,
                deck);
        }

        public ISendArmiesToOccupyPhase CreateSendArmiesToOccupyPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return new SendArmiesToOccupyPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                playerGameDatas,
                deck,
                attackingRegion,
                occupiedRegion,
                _territoryOccupier);
        }

        public IGameOverState CreateGameOverState(PlayerName winner)
        {
            return new GameOverState(winner);
        }
    }
}