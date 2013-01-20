using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;

namespace RISK.Domain
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Area> _areas;

        public WorldMap(IAreaDefinitionProvider areaDefinitionProvider)
        {
            _areas = areaDefinitionProvider.GetAll()
                .Select(x => new Area(x))
                .ToList();
        }

        public IArea GetArea(IAreaDefinition areaDefinition)
        {
            return _areas.Single(x => x.AreaDefinition == areaDefinition);
        }
    }
}