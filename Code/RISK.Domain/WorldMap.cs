using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Area> _areas;

        public WorldMap(IAreaDefinitionRepository areaDefinitionRepository)
        {
            _areas = areaDefinitionRepository.GetAll()
                .Select(x => new Area(x))
                .ToList();
        }

        public IArea GetArea(IAreaDefinition areaDefinition)
        {
            return _areas.Single(x => x.AreaDefinition == areaDefinition);
        }
    }
}