using System;
using RISK.Domain.Entities;

namespace RISK.Domain
{
    public class WorldMap : IWorldMap
    {
        public IArea GetArea(IAreaDefinition areaDefinition)
        {
            throw new NotImplementedException();
        }
    }
}