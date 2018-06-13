using System.Collections.Generic;

namespace FlamingStrike.Service.Play
{
    public class GamePlaySetupRequest
    {
        private readonly IReadOnlyList<string> _players;
        private readonly IReadOnlyList<TerritoryDto> _territories;

        public GamePlaySetupRequest(IReadOnlyList<string> players, IReadOnlyList<TerritoryDto> territories)
        {
            _players = players;
            _territories = territories;
        }

        public IEnumerable<string> GetPlayers()
        {
            return _players;
        }

        public IEnumerable<TerritoryDto> GetTerritories()
        {
            return _territories;
        }
    }
}