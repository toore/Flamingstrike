using System;
using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class FortifyGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly GameData _gameData;

        public FortifyGameState(IGameStateConductor gameStateConductor, GameData gameData)

        {
            _gameStateConductor = gameStateConductor;
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

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
        }

        public int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            throw new InvalidOperationException();
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return false;
        }

        public IGameState Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            throw new InvalidOperationException();
        }

        public bool CanSendAdditionalArmiesToOccupy()
        {
            return false;
        }

        public int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            throw new InvalidOperationException();
        }

        public IGameState SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return false;
        }

        public IGameState Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            throw new InvalidOperationException();
        }

        public bool CanEndTurn()
        {
            return false;
        }

        public IGameState EndTurn()
        {
            throw new InvalidOperationException();
        }
    }
}