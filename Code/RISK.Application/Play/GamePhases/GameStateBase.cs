using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameState
    {
        IPlayer CurrentPlayer { get; }
        ITerritory GetTerritory(IRegion region);

        int GetNumberOfArmiesToDraft();
        bool CanPlaceDraftArmies(IRegion region);
        IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        IGameState Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendArmiesToOccupy();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
        IGameState SendArmiesToOccupy(int numberOfArmies);
        bool CanFortify(IRegion sourceTerritory, IRegion destinationTerritory);
        IGameState Fortify(IRegion sourceTerritory, IRegion destinationFortify, int armies);
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
        protected IReadOnlyList<IPlayer> Players => _gameData.Players;
        protected IReadOnlyList<ITerritory> Territories => _gameData.Territories;

        public ITerritory GetTerritory(IRegion region)
        {
            return Territories.GetTerritory(region);
        }

        public virtual int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
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

        public virtual bool CanSendArmiesToOccupy()
        {
            return false;
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState SendArmiesToOccupy(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanFortify(IRegion sourceTerritory, IRegion destinationTerritory)
        {
            return false;
        }

        public virtual IGameState Fortify(IRegion sourceTerritory, IRegion destinationFortify, int armies)
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