namespace RISK.Application.GamePlaying.Setup
{
    public interface IGameInitializerNotifier
    {
        void InitializationFinished(IGame game);
    }
}