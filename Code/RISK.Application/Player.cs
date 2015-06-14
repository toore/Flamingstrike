namespace RISK.Application
{
    public interface IPlayer
    {
        IPlayerId PlayerId { get; }
    }

    public class Player : IPlayer
    {
        private readonly int _armiesToPlace;

        public Player(IPlayerId playerId, int armiesToPlace)
        {
            PlayerId = playerId;
            _armiesToPlace = armiesToPlace;
        }

        public IPlayerId PlayerId { get; }

        public bool HasArmiesLeftToPlace()
        {
            return _armiesToPlace > 0;
        }

        public int GetNumberOfArmiesLeftToPlace()
        {
            return _armiesToPlace;
        }
    }
}