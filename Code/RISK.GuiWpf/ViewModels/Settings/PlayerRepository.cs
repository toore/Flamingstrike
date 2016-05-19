using System.Collections.Generic;
using RISK.GameEngine;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerRepository
    {
        IList<IPlayer> GetAll();
        void Add(IPlayer player);
        void Clear();
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public IList<IPlayer> GetAll()
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