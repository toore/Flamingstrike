using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play.GamePhases
{
    public interface IGameState
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IDeck Deck { get; }
        bool CanPlaceDraftArmies(IRegion region);
        int GetNumberOfArmiesToDraft();
        void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendAdditionalArmiesToOccupy();
        int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
        bool CanFreeMove();
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        bool CanEndTurn();
        void EndTurn();
    }
}