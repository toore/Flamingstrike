using System;

namespace RISK.Domain
{
    public class Game : IGame
    {
        private readonly IWorldMap _worldMap;

        public Game(IWorldMap worldMap)
        {
            _worldMap = worldMap;
        }

        public IWorldMap GetWorldMap()
        {
            return _worldMap;
        }

        public ITurn GetNextTurn()
        {
            throw new NotImplementedException();
        }
    }
}