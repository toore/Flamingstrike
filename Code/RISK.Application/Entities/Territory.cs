using System;

namespace RISK.Domain.Entities
{
    public interface ITerritory
    {
        ILocation Location { get; }
        IPlayer Occupant { get; set; }
        int Armies { get; set; }

        bool IsOccupied();
        bool HasArmiesAvailableForAttack();
        int GetArmiesAvailableForAttack();
    }

    public class Territory : ITerritory
    {
        public Territory(ILocation location)
        {
            Location = location;
        }

        public ILocation Location { get; private set; }
        public IPlayer Occupant { get; set; }
        public int Armies { get; set; }

        public bool IsOccupied()
        {
            return Occupant != null;
        }

        public bool HasArmiesAvailableForAttack()
        {
            return GetArmiesAvailableForAttack() > 0;
        }

        public int GetArmiesAvailableForAttack()
        {
            return Math.Max(Armies - 1, 0);
        }
    }
}