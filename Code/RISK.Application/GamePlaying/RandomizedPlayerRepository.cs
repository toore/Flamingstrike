using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class RandomizedPlayerRepository : IRandomizedPlayerRepository
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IRandomWrapper _randomWrapper;

        public RandomizedPlayerRepository(IPlayerRepository playerRepository, IRandomWrapper randomWrapper)
        {
            _playerRepository = playerRepository;
            _randomWrapper = randomWrapper;
        }

        public IEnumerable<IPlayer> GetAllInRandomizedOrder()
        {
            var players = _playerRepository.GetAll().ToList();
            var randomizedPlayers = new List<IPlayer>();

            while (players.Any())
            {
                var randomPlayerIndex = _randomWrapper.Next(players.Count - 1);
                var randomPlayer = players.ElementAt(randomPlayerIndex);
                randomizedPlayers.Add(randomPlayer);
                players.Remove(randomPlayer);
            }

            return randomizedPlayers;
        }
    }
}