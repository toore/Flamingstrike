using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IRandomizedPlayerRepository
    {
        IEnumerable<IPlayer> GetAllInRandomizedOrder();
    }
}