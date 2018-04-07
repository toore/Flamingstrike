using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using NSubstitute;

namespace Tests.GameEngine
{
    public class TerritoryBuilder
    {
        private IRegion _region = Substitute.For<IRegion>();
        private PlayerName _playerName = new PlayerName("player name");
        private int _armies = 1;

        public Territory Build()
        {
            return new Territory(_region, _playerName, _armies);
        }

        public TerritoryBuilder Region(IRegion region)
        {
            _region = region;
            return this;
        }

        public TerritoryBuilder Player(PlayerName playerName)
        {
            _playerName = playerName;
            return this;
        }

        public TerritoryBuilder Armies(int armies)
        {
            _armies = armies;
            return this;
        }
    }
}