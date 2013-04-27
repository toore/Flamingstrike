using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class PlayerInSetup
    {
        private readonly IPlayer _player;

        public int Armies { get; set; }

        public PlayerInSetup(IPlayer player, int armies)
        {
            _player = player;
            Armies = armies;
        }

        public IPlayer GetInGamePlayer()
        {
            return _player;
        }
    }
}