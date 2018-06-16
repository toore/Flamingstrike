using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface IAttackPhase
    {
        string CurrentPlayerName { get; }
        IReadOnlyList<Territory> Territories { get; }
        IReadOnlyList<Player> Players { get; }
        IReadOnlyList<Region> RegionsThatCanBeSourceForAttackOrFortification { get; }
        void Attack(Region attackingRegion, Region defendingRegion);
        void Fortify(Region sourceRegion, Region destinationRegion, int armies);
        Task<IEnumerable<Region>> GetRegionsThatCanBeAttacked(Region sourceRegion);
        Task<IEnumerable<Region>> GetRegionsThatCanBeFortified(Region sourceRegion);
        void EndTurn();
    }
}