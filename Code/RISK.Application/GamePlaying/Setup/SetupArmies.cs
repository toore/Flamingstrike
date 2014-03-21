using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class SetupArmies
    {
        private readonly IPlayer _player;
        private int _armies;

        public SetupArmies(IPlayer player, int armies)
        {
            _player = player;
            _armies = armies;
        }

        public int GetArmies()
        {
            return _armies;
        }

        public bool HasArmies()
        {
            return _armies > 0;
        }

        public IPlayer GetPlayer()
        {
            return _player;
        }

        public void Decrease()
        {
            _armies--;
        }
    }
}