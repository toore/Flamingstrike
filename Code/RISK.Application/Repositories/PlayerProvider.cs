using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class PlayerProvider : IPlayerProvider
    {
        public IEnumerable<IPlayer> All { get; set; }
    }
}