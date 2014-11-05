using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public class Player
    {
        private readonly IPlayer _player;

        public Player(IPlayer player, int armiesToPlace)
        {
            _player = player;
            ArmiesToPlace = armiesToPlace;
        }

        public int ArmiesToPlace { get; set; }

        public IPlayer GetPlayer()
        {
            return _player;
        }

        public bool HasArmiesToPlace()
        {
            return ArmiesToPlace > 0;
        }
    }
}