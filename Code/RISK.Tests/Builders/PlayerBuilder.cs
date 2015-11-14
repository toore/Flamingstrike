using RISK.Application;
using RISK.Application.Play;

namespace RISK.Tests.Builders
{
    public class PlayerBuilder
    {
        private IPlayerId _playerId;

        public Player Build()
        {
            return new Player(_playerId);
        }

        public PlayerBuilder PlayerId(IPlayerId playerId)
        {
            _playerId = playerId;
            return this;
        }
    }
}