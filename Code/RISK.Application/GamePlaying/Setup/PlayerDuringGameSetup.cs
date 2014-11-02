using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public class PlayerDuringGameSetup
    {
        private readonly IPlayer _player;
        private int _armies;

        public PlayerDuringGameSetup(IPlayer player, int armies)
        {
            _player = player;
            _armies = armies;
        }

        public int GetArmiesLeft()
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

        public void ArmyHasBeenPlaced()
        {
            _armies--;
        }
    }
}