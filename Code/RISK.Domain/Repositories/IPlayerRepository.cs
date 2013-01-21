using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<IPlayer> GetAll();
    }
}