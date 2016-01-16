namespace RISK.Application.Play
{
    public interface IInGameplayPlayer
    {
        IPlayer Player { get; }
    }

    public class InGameplayPlayer : IInGameplayPlayer
    {
        public InGameplayPlayer(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; }
    }
}