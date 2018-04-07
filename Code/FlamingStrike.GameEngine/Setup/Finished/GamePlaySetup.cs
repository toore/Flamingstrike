using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Setup.Finished
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<Player> GetPlayers();
        IReadOnlyList<Territory> GetTerritories();
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        private readonly IReadOnlyList<Player> _players;
        private readonly IReadOnlyList<Territory> _territories;

        public GamePlaySetup(IReadOnlyList<Player> players, IReadOnlyList<Territory> territories)
        {
            _players = players;
            _territories = territories;
        }

        public IReadOnlyList<Player> GetPlayers()
        {
            return _players;
        }

        public IReadOnlyList<Territory> GetTerritories()
        {
            return _territories;
        }
    }
}