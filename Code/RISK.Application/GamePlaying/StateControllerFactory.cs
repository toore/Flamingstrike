using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IStateControllerFactory
    {
        StateController Create();
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        public StateController Create()
        {
            return new StateController();
        }
    }
}