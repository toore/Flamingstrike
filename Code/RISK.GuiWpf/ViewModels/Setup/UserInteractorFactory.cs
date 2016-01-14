using GuiWpf.Services;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractorFactory
    {
        IUserInteractor Create(IGameSetupViewModel gameSetupViewModel);
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

        public IUserInteractor Create(IGameSetupViewModel gameSetupViewModel)
        {
            return new UserInteractor(_userInteractionFactory, _guiThreadDispatcher, gameSetupViewModel);
        }
    }
}