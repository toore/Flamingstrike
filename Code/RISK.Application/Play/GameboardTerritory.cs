using System;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameboardTerritory
    {
        ITerritory Territory { get; }
        int Armies { get; }
        int GetNumberOfAttackingArmies();
        int GetNumberOfDefendingArmies();
    }

    public class GameboardTerritory : IGameboardTerritory
    {
        private readonly IPlayer _player;

        public GameboardTerritory(ITerritory territory, IPlayer player, int armies)
        {
            Territory = territory;
            _player = player;
            Armies = armies;
        }

        public ITerritory Territory { get; }
        public int Armies { get; }

        public int GetNumberOfAttackingArmies()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfDefendingArmies()
        {
            return Armies;
        }
    }
}