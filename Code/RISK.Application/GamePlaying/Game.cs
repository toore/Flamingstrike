using System.Collections.Generic;
using System.Linq;
using RISK.Base.Extensions;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace RISK.Domain.GamePlaying
{
    public class Game : IGame
    {
        private readonly IWorldMap _worldMap;
        private readonly ITurnFactory _turnFactory;
        private readonly IList<IPlayer> _players;
        private IPlayer _currentPlayer;

        public Game(ITurnFactory turnFactory, IPlayers players, IAlternateGameSetup alternateGameSetup, ILocationSelector locationSelector)
        {
            _turnFactory = turnFactory;
            _players = players.GetAll().ToList();

            _worldMap = alternateGameSetup.Initialize(locationSelector);
        }

        public IWorldMap GetWorldMap()
        {
            return _worldMap;
        }

        public ITurn GetNextTurn()
        {
            _currentPlayer = _players.GetNextOrFirst(_currentPlayer);

            return _turnFactory.Create(_currentPlayer, _worldMap);
        }
    }
}