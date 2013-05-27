using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface IPlayerProvider
    {
        IEnumerable<IPlayer> All { get; set; }
    }
}