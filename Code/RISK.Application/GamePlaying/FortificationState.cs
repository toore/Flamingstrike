using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class FortificationState: IInteractionState
    {
        private readonly StateController _stateController;

        public FortificationState(StateController stateController, IPlayer player)
        {
            _stateController = stateController;
            Player = player;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }
        public bool IsTerritorySelected { get; private set; }
        public bool CanSelect(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Select(ILocation location)
        {
            throw new NotImplementedException();
        }

        public bool CanAttack(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Attack(ILocation location)
        {
            throw new NotImplementedException();
        }

        public bool CanFortify(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Fortify(ILocation location, int armies)
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }
    }
}