using System;
using System.Collections.Generic;
using System.Linq;
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

    public class GameStateBase : IGameState
    {
        private readonly GameData _gameData;

        protected GameStateBase(GameData gameData)
        {
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public ITerritory GetTerritory(IRegion region)
        {
            return Territories.GetTerritory(region);
        }

        public virtual bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
        }

        public virtual int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return false;
        }

        public virtual IGameState Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanSendAdditionalArmiesToOccupy()
        {
            return false;
        }

        public virtual int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return false;
        }

        public virtual IGameState Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanEndTurn()
        {
            return false;
        }

        public virtual IGameState EndTurn()
        {
            throw new InvalidOperationException();
        }

        protected void ThrowIfTerritoriesDoesNotContain(ITerritory territory)
        {
            if (!Territories.Contains(territory))
            {
                throw new InvalidOperationException("Territory does not exist in game");
            }
        }
    }
}