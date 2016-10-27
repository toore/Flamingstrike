using System.Collections.Generic;
using System.Windows.Media;
using RISK.GameEngine;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public interface IPlayerUiDataRepository
    {
        IList<IPlayerUiData> GetAll();
        void Add(IPlayerUiData playerUiData);
        void Clear();
    }

    public interface IPlayerUiData
    {
        IPlayer Player { get; }
    }

    public class PlayerUiData : IPlayerUiData
    {
        public IPlayer Player { get; }
        public Color FillColor { get; }

        public PlayerUiData(IPlayer player, Color fillColor)
        {
            Player = player;
            FillColor = fillColor;
        }
    }

    public class PlayerUiDataRepository : IPlayerUiDataRepository
    {
        private readonly List<IPlayerUiData> _players = new List<IPlayerUiData>();

        public IList<IPlayerUiData> GetAll()
        {
            return _players;
        }

        public void Add(IPlayerUiData playerUiData)
        {
            _players.Add(playerUiData);
        }

        public void Clear()
        {
            _players.Clear();
        }
    }
}