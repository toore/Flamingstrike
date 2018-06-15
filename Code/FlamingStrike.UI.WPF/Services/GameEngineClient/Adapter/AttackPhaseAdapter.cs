using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Adapter
{
    public class AttackPhaseAdapter : IAttackPhase
    {
        private readonly GameEngine.Play.IAttackPhase _attackPhase;

        public AttackPhaseAdapter(GameEngine.Play.IAttackPhase attackPhase)
        {
            _attackPhase = attackPhase;
        }

        public string CurrentPlayerName => (string)_attackPhase.CurrentPlayerName;
        public IReadOnlyList<Territory> Territories => _attackPhase.Territories.Select(x => x.MapFromEngine()).ToList();
        public IReadOnlyList<Player> Players => _attackPhase.Players.Select(x => x.MapFromEngine()).ToList();

        public IReadOnlyList<Region> RegionsThatCanBeSourceForAttackOrFortification
        {
            get { return _attackPhase.RegionsThatCanBeSourceForAttackOrFortification.Select(x => x.MapFromEngine()).ToList(); }
        }

        public void Attack(Region attackingRegion, Region defendingRegion)
        {
            _attackPhase.Attack(attackingRegion.MapToEngine(), defendingRegion.MapToEngine());
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            _attackPhase.Fortify(sourceRegion.MapToEngine(), destinationRegion.MapToEngine(), armies);
        }

        public Task<IEnumerable<Region>> GetRegionsThatCanBeAttacked(Region sourceRegion)
        {
            return Task.FromResult(_attackPhase.GetRegionsThatCanBeAttacked(sourceRegion.MapToEngine()).Select(x => x.MapFromEngine()));
        }

        public IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion)
        {
            return _attackPhase.GetRegionsThatCanBeFortified(sourceRegion.MapToEngine()).Select(x => x.MapFromEngine());
        }

        public void EndTurn()
        {
            _attackPhase.EndTurn();
        }
    }
}