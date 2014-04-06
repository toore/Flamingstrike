using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class SetupPlayer
    {
        private readonly IPlayer _player;
        private int _armies;

        public SetupPlayer(IPlayer player, int armies)
        {
            _player = player;
            _armies = armies;
        }

        public int GetArmies()
        {
            return _armies;
        }

        public bool HasArmiesLeft()
        {
            return _armies > 0;
        }

        public IPlayer GetPlayer()
        {
            return _player;
        }

        public void DecreaseArmies()
        {
            _armies--;
        }
    }
}