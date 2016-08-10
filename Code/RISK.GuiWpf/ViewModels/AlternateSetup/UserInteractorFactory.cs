using GuiWpf.Services;

namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IUserInteractorFactory
    {
        IUserInteractor Create(IAlternateGameSetupViewModel alternateGameSetupViewModel);
    }

    public class UserInteractorFactory : IUserInteractorFactory
    {
        private readonly IUserInteractionFactory _userInteractionFactory;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;

        public UserInteractorFactory(IUserInteractionFactory userInteractionFactory, IGuiThreadDispatcher guiThreadDispatcher)
        {
            _userInteractionFactory = userInteractionFactory;
            _guiThreadDispatcher = guiThreadDispatcher;
        }

        public IUserInteractor Create(IAlternateGameSetupViewModel alternateGameSetupViewModel)
        {
            return new UserInteractor(_userInteractionFactory, _guiThreadDispatcher, alternateGameSetupViewModel);
        }
    }
}