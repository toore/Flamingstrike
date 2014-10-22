using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying.Setup;

namespace RISK.Domain.GamePlaying
{
    public class Game : IGame
    {
        private readonly IWorldMap _worldMap;
        private readonly ITurnFactory _turnFactory;
        private readonly IList<IPlayer> _players;
        private IPlayer _currentPlayer;

        public Game(ITurnFactory turnFactory, IPlayers players, IAlternateGameSetup alternateGameSetup, IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            _turnFactory = turnFactory;
            _players = players.GetAll().ToList();

            _worldMap = alternateGameSetup.Initialize(gameInitializerLocationSelector);
        }

        public IWorldMap GetWorldMap()
        {
            return _worldMap;
        }

        public ITurn GetNextTurn()
        {
            _currentPlayer = _players.GetNextOrFirst(_currentPlayer);

            return _turnFactory.CreateSelectTurn(_currentPlayer, _worldMap);
        }
    }
}