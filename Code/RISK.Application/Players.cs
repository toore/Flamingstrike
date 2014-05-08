using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain
{
    public interface IPlayers
    {
        IEnumerable<IPlayer> GetAll();
    }

    public interface IPlayersInitializer
    {
        void SetPlayers(IEnumerable<IPlayer> players);
    }

    public class Players : IPlayers, IPlayersInitializer
    {
        private List<IPlayer> _players = new List<IPlayer>();

        public IEnumerable<IPlayer> GetAll()
        {
            return _players;
        }

        public void SetPlayers(IEnumerable<IPlayer> players)
        {
            _players = players.ToList();
        }
    }
}