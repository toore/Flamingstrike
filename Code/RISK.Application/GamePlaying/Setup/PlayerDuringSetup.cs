using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class PlayerDuringSetup
    {
        private readonly IPlayer _player;

        public PlayerDuringSetup(IPlayer player, int armies)
        {
            _player = player;
            Armies = armies;
        }

        public int Armies { get; set; }

        public IPlayer GetInGamePlayer()
        {
            return _player;
        }
    }
}