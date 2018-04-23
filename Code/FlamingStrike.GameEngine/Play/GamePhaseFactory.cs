using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGamePhaseFactory
    {
        IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, int numberOfArmiesToDraft);
        IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, ConqueringAchievement conqueringAchievement);
        IEndTurnPhase CreateEndTurnPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck);
        ISendArmiesToOccupyPhase CreateSendArmiesToOccupyPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, Region attackingRegion, Region occupiedRegion);
        IGameOverState CreateGameOverState(PlayerName winner);
    }

    public class GamePhaseFactory : IGamePhaseFactory
    {
        private readonly IAttackService _attackService;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private readonly IWorldMap _worldMap;

        public GamePhaseFactory(
            IAttackService attackService,
            IPlayerEliminationRules playerEliminationRules,
            IWorldMap worldMap)
        {
            _attackService = attackService;
            _playerEliminationRules = playerEliminationRules;
            _worldMap = worldMap;
        }

        public IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, int numberOfArmiesToDraft)
        {
            return new DraftArmiesPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                players,
                deck,
                numberOfArmiesToDraft);
        }

        public IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, ConqueringAchievement conqueringAchievement)
        {
            return new AttackPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                players,
                deck,
                conqueringAchievement,
                _attackService,
                _playerEliminationRules,
                _worldMap);
        }

        public IEndTurnPhase CreateEndTurnPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck)
        {
            return new EndTurnPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                players,
                deck);
        }

        public ISendArmiesToOccupyPhase CreateSendArmiesToOccupyPhase(IGamePhaseConductor gamePhaseConductor, PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayer> players, IDeck deck, Region attackingRegion, Region occupiedRegion)
        {
            return new SendArmiesToOccupyPhase(
                gamePhaseConductor,
                currentPlayerName,
                territories,
                players,
                deck,
                attackingRegion,
                occupiedRegion);
        }

        public IGameOverState CreateGameOverState(PlayerName winner)
        {
            return new GameOverState(winner);
        }
    }
}