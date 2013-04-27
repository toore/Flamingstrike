using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public IEnumerable<IPlayer> GetAll()
        {
            return _players;
        }

        public void Add(IPlayer player)
        {
            _players.Add(player);
        }
    }
}