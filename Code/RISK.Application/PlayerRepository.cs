using System.Collections.Generic;
using RISK.Application.Entities;

namespace RISK.Application
{
    public interface IPlayerRepository
    {
        IEnumerable<IPlayer> GetAll();
        void Add(IPlayer player);
        void Clear();
    }

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

        public void Clear()
        {
            _players.Clear();
        }
    }
}