using System.Collections.Generic;

namespace FlamingStrike.Service.Play
{
    public class GamePlaySetup
    {
        private readonly IReadOnlyList<string> _players;
        private readonly IReadOnlyList<Territory> _territories;

        public GamePlaySetup(IReadOnlyList<string> players, IReadOnlyList<Territory> territories)
        {
            _players = players;
            _territories = territories;
        }

        public IEnumerable<string> GetPlayers()
        {
            return _players;
        }

        public IEnumerable<Territory> GetTerritories()
        {
            return _territories;
        }
    }
}