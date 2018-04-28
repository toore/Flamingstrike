using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<string> GetPlayers();
        IReadOnlyList<Territory> GetTerritories();
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        private readonly IReadOnlyList<string> _players;
        private readonly IReadOnlyList<Territory> _territories;

        public GamePlaySetup(IReadOnlyList<string> players, IReadOnlyList<Territory> territories)
        {
            _players = players;
            _territories = territories;
        }

        public IReadOnlyList<string> GetPlayers()
        {
            return _players;
        }

        public IReadOnlyList<Territory> GetTerritories()
        {
            return _territories;
        }
    }
}