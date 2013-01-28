using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        void Select(ILocation location);
        void Attack(ILocation location);

        bool PlayerShouldReceiveCardWhenTurnEnds();
    }
}