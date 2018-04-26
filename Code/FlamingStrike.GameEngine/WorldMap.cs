using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine
{
    public interface IWorldMap
    {
        bool HasBorder(Region a, Region b);
        void AddBorder(Region a, Region b);
        IEnumerable<Region> GetBorders(Region region);
        List<Region> GetAll();
    }

    public class WorldMap : IWorldMap
    {
        private readonly Dictionary<Region, List<Region>> _map = new Dictionary<Region, List<Region>>();

        public bool HasBorder(Region a, Region b)
        {
            return _map[a].Contains(b);
        }

        public void AddBorder(Region a, Region b)
        {
            SafeAddValue(a, b);
            SafeAddValue(b, a);
        }

        public IEnumerable<Region> GetBorders(Region region)
        {
            return _map[region].ToList();
        }

        public List<Region> GetAll()
        {
            return _map.Keys.ToList();
        }

        private void SafeAddValue(Region key, Region value)
        {
            if (key == value)
            {
                throw new ArgumentException($"Region can't have border to itself. {nameof(key)} and {nameof(value)} are equal.");
            }

            if (_map.ContainsKey(key) && _map[key].Contains(value))
            {
                throw new InvalidOperationException("Regions already has a border");
            }

            if (!_map.ContainsKey(key))
            {
                _map[key] = new List<Region>();
            }

            _map[key].Add(value);
        }
    }
}