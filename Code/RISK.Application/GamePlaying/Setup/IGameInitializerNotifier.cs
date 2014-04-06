namespace RISK.Domain.GamePlaying.Setup
{
    public interface IGameInitializerNotifier
    {
        void InitializationFinished(IGame game);
    }
}