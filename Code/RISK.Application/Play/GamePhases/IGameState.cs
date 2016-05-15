using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameState
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IDeck Deck { get; }
        ITerritory GetTerritory(IRegion region);
        bool CanPlaceDraftArmies(IRegion region);
        int GetNumberOfArmiesToDraft();
        IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        IGameState Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendAdditionalArmiesToOccupy();
        int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        IGameState SendAdditionalArmiesToOccupy(int numberOfArmies);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        IGameState Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        bool CanEndTurn();
        IGameState EndTurn();
    }
}