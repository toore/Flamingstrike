using System;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameboardTerritory
    {
        ITerritory Territory { get; }
        IPlayer Player { get; }
        int Armies { get; }
        int GetNumberOfAttackingArmies();
        int GetNumberOfDefendingArmies();
    }

    public class GameboardTerritory : IGameboardTerritory
    {
        public GameboardTerritory(ITerritory territory, IPlayer player, int armies)
        {
            Territory = territory;
            Player = player;
            Armies = armies;
        }

        public IPlayer Player { get; }
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