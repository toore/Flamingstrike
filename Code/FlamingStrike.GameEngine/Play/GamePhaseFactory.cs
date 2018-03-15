using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGamePhaseFactory
    {
        IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, int numberOfArmiesToDraft);
        IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, ConqueringAchievement conqueringAchievement);
    }

    public class GamePhaseFactory : IGamePhaseFactory
    {
        private readonly IArmyDrafter _armyDrafter;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;

        public GamePhaseFactory(
            IArmyDrafter armyDrafter, 
            IAttacker attacker, 
            IFortifier fortifier,
            IPlayerEliminationRules playerEliminationRules)
        {
            _armyDrafter = armyDrafter;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
        }

        public IDraftArmiesPhase CreateDraftArmiesPhase(IGamePhaseConductor gamePhaseConductor, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, int numberOfArmiesToDraft)
        {
            return new DraftArmiesPhase(
                gamePhaseConductor,
                currentPlayer,
                territories,
                playerGameDatas,
                deck,
                numberOfArmiesToDraft,
                _armyDrafter);
        }

        public IAttackPhase CreateAttackPhase(IGamePhaseConductor gamePhaseConductor, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck, ConqueringAchievement conqueringAchievement)
        {
            return new AttackPhase(
                gamePhaseConductor,
                currentPlayer,
                territories,
                playerGameDatas,
                deck,
                conqueringAchievement,
                _attacker,
                _fortifier,
                _playerEliminationRules);
        }
    }
}