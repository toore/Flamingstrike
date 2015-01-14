using System;
using System.Collections.Generic;

namespace RISK.Application.Entities
{
    public interface ITerritory
    {
        Continent Continent { get; }
        bool IsBordering(ITerritory territory);

        IPlayer Occupant { get; set; }
        int Armies { get; set; }

        bool IsOccupied();
        bool HasArmiesAvailableForAttack();
        int GetArmiesAvailableForAttack();
    }

    public class Territory : ITerritory
    {
        private readonly List<ITerritory> _borders;

        public Territory(string name, Continent continent)
        {
            Continent = continent;
            _borders = new List<ITerritory>();
        }

        public Continent Continent { get; private set; }

        public bool IsBordering(ITerritory territory)
        {
            return _borders.Contains(territory);
        }

        public void AddBorders(params ITerritory[] locations)
        {
            _borders.AddRange(locations);
        }

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