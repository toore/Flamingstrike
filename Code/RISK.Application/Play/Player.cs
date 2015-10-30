namespace RISK.Application.Play
{
    public interface IPlayer
    {
        IPlayerId PlayerId { get; }
    }

    public class Player : IPlayer
    {
        public Player(IPlayerId playerId)
        {
            PlayerId = playerId;
        }

        public IPlayerId PlayerId { get; }
    }
}