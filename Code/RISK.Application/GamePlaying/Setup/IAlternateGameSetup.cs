namespace RISK.Domain.GamePlaying.Setup
{
    public interface IAlternateGameSetup
    {
        IWorldMap Initialize(ILocationSelector locationSelector);
    }
}