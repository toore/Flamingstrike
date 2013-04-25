﻿using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;
using RISK.Domain.Extensions;

namespace RISK.Domain.GamePlaying
{
    public class Game : IGame, ILocationSelector
    {
        private readonly IWorldMap _worldMap;
        private readonly ITurnFactory _turnFactory;
        private readonly IList<IPlayer> _players;
        private IPlayer _currentPlayer;

        public Game(ITurnFactory turnFactory, IPlayerRepository playerRepository, IAlternateGameSetup alternateGameSetup)
        {
            _turnFactory = turnFactory;
            _players = playerRepository.GetAll()
                .ToList();

            _worldMap = alternateGameSetup.Initialize(this);
        }

        public ILocation Select(IEnumerable<ILocation> locations)
        {
            //TODO: Select from gui?
            return locations.First();
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