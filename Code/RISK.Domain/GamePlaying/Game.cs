using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class Game : IGame
    {
        private readonly IWorldMap _worldMap;
        private readonly ITurnFactory _turnFactory;
        private readonly IEnumerable<IPlayer> _players;

        public Game(ITurnFactory turnFactory, IPlayerRepository playerRepository, IAlternateGameSetup alternateGameSetup)
        {
            _turnFactory = turnFactory;
            _players = playerRepository.GetAll();

            _worldMap = alternateGameSetup.Initialize();
        }

        public IWorldMap GetWorldMap()
        {
            return _worldMap;
        }

        public ITurn GetNextTurn()
        {
            return _turnFactory.Create(_players.First());
        }
    }
}