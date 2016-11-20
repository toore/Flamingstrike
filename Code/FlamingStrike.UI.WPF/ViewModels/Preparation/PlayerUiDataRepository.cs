using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
{
    public interface IPlayerUiDataRepository
    {
        void Add(IPlayerUiData playerUiData);
        IPlayerUiData Get(IPlayer player);
        IList<IPlayerUiData> GetAll();
        void Clear();
    }

    public interface IPlayerUiData
    {
        IPlayer Player { get; }
        Color Color { get; }
    }

    public class PlayerUiData : IPlayerUiData
    {
        public IPlayer Player { get; }
        public Color Color { get; }

        public PlayerUiData(IPlayer player, Color color)
        {
            Player = player;
            Color = color;
        }
    }

    public class PlayerUiDataRepository : IPlayerUiDataRepository
    {
        private readonly List<IPlayerUiData> _players = new List<IPlayerUiData>();

        public void Add(IPlayerUiData playerUiData)
        {
            _players.Add(playerUiData);
        }

        public IPlayerUiData Get(IPlayer player)
        {
            return _players.Single(x => x.Player == player);
        }

        public IList<IPlayerUiData> GetAll()
        {
            return _players;
        }

        public void Clear()
        {
            _players.Clear();
        }
    }
}