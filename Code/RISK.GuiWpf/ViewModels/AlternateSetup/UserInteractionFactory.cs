namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IUserInteractionFactory
    {
        IUserInteraction Create();
    }

    public class UserInteractionFactory : IUserInteractionFactory
    {
        public IUserInteraction Create()
        {
            return new UserInteraction();
        }
    }
}