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
        private readonly ITurnStateFactory _turnStateFactory;
        private readonly IList<IPlayer> _players;
        private IPlayer _currentPlayer;

        public Game(ITurnStateFactory turnStateFactory, IPlayers players, IAlternateGameSetup alternateGameSetup, IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            _turnStateFactory = turnStateFactory;
            _players = players.GetAll().ToList();

            _worldMap = alternateGameSetup.Initialize(gameInitializerLocationSelector);
        }

        public IWorldMap GetWorldMap()
        {
            return _worldMap;
        }

        public ITurnState GetNextTurn()
        {
            _currentPlayer = _players.GetNextOrFirst(_currentPlayer);

            return _turnStateFactory.CreateSelectState(_currentPlayer, _worldMap);
        }
    }
}