using System;
using RISK.Application.World;

namespace RISK.Application.GamePlay
{
    public interface IGameboardTerritory
    {
        int GetNumberOfAttackingArmies();
        int GetNumberOfDefendingArmies();
    }

    public class GameboardTerritory : IGameboardTerritory
    {
        private readonly ITerritory _territory;
        private readonly IPlayerId _playerId;
        private readonly int _armies;

        public GameboardTerritory(ITerritory territory, IPlayerId playerId, int armies)
        {
            _territory = territory;
            _playerId = playerId;
            _armies = armies;
        }

        public int GetNumberOfAttackingArmies()
        {
            return Math.Max(_armies - 1, 0);
        }

        public int GetNumberOfDefendingArmies()
        {
            return _armies;
        }
    }
}