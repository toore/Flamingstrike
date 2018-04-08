using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Setup.Finished
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<PlayerName> GetPlayers();
        IReadOnlyList<Territory> GetTerritories();
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        private readonly IReadOnlyList<PlayerName> _players;
        private readonly IReadOnlyList<Territory> _territories;

        public GamePlaySetup(IReadOnlyList<PlayerName> players, IReadOnlyList<Territory> territories)
        {
            _players = players;
            _territories = territories;
        }

        public IReadOnlyList<PlayerName> GetPlayers()
        {
            return _players;
        }

        public IReadOnlyList<Territory> GetTerritories()
        {
            return _territories;
        }
    }
}