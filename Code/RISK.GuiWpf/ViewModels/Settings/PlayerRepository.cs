using System.Collections.Generic;
using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerRepository
    {
        IList<IPlayerId> GetAll();
        void Add(IPlayerId playerId);
        void Clear();
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<IPlayerId> _players = new List<IPlayerId>();

        public IList<IPlayerId> GetAll()
        {
            return _players;
        }

        public void Add(IPlayerId playerId)
        {
            _players.Add(playerId);
        }

        public void Clear()
        {
            _players.Clear();
        }
    }
}