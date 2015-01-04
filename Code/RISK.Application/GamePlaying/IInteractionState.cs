using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IInteractionState
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }

        bool CanClick(ITerritory territory);
        void OnClick(ITerritory territory);
    }

    public class TurnEndState : IInteractionState
    {
        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }
        public bool CanClick(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }

        public void OnClick(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }
    }
}