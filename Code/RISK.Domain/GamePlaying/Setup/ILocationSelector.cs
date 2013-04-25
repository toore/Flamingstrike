using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface ILocationSelector
    {
        ILocation Select(IEnumerable<ILocation> locations);
    }
}