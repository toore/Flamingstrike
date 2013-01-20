using System;

namespace RISK.Domain
{
    public class Game
    {
        public ITurn GetNextTurn()
        {
            throw new NotImplementedException();
        }

        public IWorldMap GetWorldMap()
        {
            throw new NotImplementedException();
        }
    }
}