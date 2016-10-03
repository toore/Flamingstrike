using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface ITerritoriesContext
    {
        void Set(IReadOnlyList<ITerritory> territories);
        IReadOnlyList<ITerritory> Territories { get; }
    }

    public class TerritoriesContext : ITerritoriesContext
    {
        public void Set(IReadOnlyList<ITerritory> territories)
        {
            Territories = territories;
        }

        public IReadOnlyList<ITerritory> Territories { get; private set; }
    }
}