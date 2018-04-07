using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
{
    public interface IPlayerUiDataRepository
    {
        void Add(IPlayerUiData playerUiData);
        IPlayerUiData Get(string player);
        IList<IPlayerUiData> GetAll();
        void Clear();
    }

    public interface IPlayerUiData
    {
        string Player { get; }
        Color Color { get; }
    }

    public class PlayerUiData : IPlayerUiData
    {
        public string Player { get; }
        public Color Color { get; }

        public PlayerUiData(string player, Color color)
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

        public IPlayerUiData Get(string player)
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