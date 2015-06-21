using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameState
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<GameboardTerritory> Territories { get; }
    }

    public class GameState : IGameState
    {
        public IPlayer CurrentPlayer { get; set; }
        public IReadOnlyList<GameboardTerritory> Territories { get; set; }

        public IEnumerable<ITerritory> GetAttackCandidates(ITerritory attacker)
        {
            var attackerClosure = Territories
                .Single(x => x.Territory == attacker);

            var attackCandidates = Territories
                .Where(attackeeCandidate => CanAttack(attackerClosure, attackeeCandidate))
                .Select(x => x.Territory)
                .ToList();

            return attackCandidates;
        }

        private bool CanAttack(GameboardTerritory attacker, GameboardTerritory attackeeCandidate)
        {
            var isBordering = attacker.Territory.HasBorderTo(attackeeCandidate.Territory);
            //var hasArmiesToAttackWith = attacker.GetNumberOfAttackingArmies() > 1;

            //var canAttack = isBordering
            //                &&
            //                isTerritoryOccupiedByEnemy
            //                &&
            //                hasArmiesToAttackWith;

            //return canAttack;
            return isBordering;
        }
    }
}