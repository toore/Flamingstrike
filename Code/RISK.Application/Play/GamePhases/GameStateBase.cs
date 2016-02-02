using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameState
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IPlayer CurrentPlayer { get; }

        int GetNumberOfArmiesToDraft();
        bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory);
        IGameState Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
        bool MustSendInArmiesToOccupyTerritory();
        void SendInArmiesToOccupyTerritory(int numberOfArmies);
        bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory);
        IGameState Fortify(ITerritory sourceTerritory, ITerritory destinationFortify);
        IGameState EndTurn();
        bool IsGameOver();
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
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;

        public virtual int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            throw new InvalidOperationException();
        }

        public virtual bool MustSendInArmiesToOccupyTerritory()
        {
            throw new InvalidOperationException();
        }

        public virtual void SendInArmiesToOccupyTerritory(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }

        public virtual bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory)
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState Fortify(ITerritory sourceTerritory, ITerritory destinationFortify)
        {
            throw new InvalidOperationException();
        }

        public virtual IGameState EndTurn()
        {
            throw new InvalidOperationException();
        }

        public virtual bool IsGameOver()
        {
            var allTerritoriesAreOccupiedBySamePlayer = Territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
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