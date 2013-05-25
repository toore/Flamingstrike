namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractionSynchronizer
    {
        void WaitForUserInteractionRequest();
        void RequestUserInteraction();
        void WaitForUserToBeDoneWithInteracting();
        void UserIsDoneInteracting();
    }
}