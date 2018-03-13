using FlamingStrike.GameEngine;
using NSubstitute;
using IPlayer = FlamingStrike.GameEngine.IPlayer;

namespace Tests.FlamingStrike.GameEngine
{
    public class TerritoryBuilder
    {
        private IRegion _region = Substitute.For<IRegion>();
        private IPlayer _player = Substitute.For<IPlayer>();
        private int _armies = 1;

        public Territory Build()
        {
            return new Territory(_region, _player, _armies);
        }

        public TerritoryBuilder Region(IRegion region)
        {
            _region = region;
            return this;
        }

        public TerritoryBuilder Player(IPlayer player)
        {
            _player = player;
            return this;
        }

        public TerritoryBuilder Armies(int armies)
        {
            _armies = armies;
            return this;
        }
    }
}