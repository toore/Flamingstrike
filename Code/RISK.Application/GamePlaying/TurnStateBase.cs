using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnStateBase : ITurnState
    {
        private readonly StateController _stateController;
        private readonly ICardFactory _cardFactory;

        protected TurnStateBase(StateController stateController, ICardFactory cardFactory, IPlayer player)
        {
            _stateController = stateController;
            _cardFactory = cardFactory;
            Player = player;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; protected set; }

        public bool IsTerritorySelected
        {
            get { return SelectedTerritory != null; }
        }
         
        public virtual bool CanSelect(ILocation location)
        {
            return false;
        }

        public virtual void Select(ILocation location)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanAttack(ILocation location)
        {
            return false;
        }

        public virtual void Attack(ILocation location)
        {
            throw new NotSupportedException();
        }

        public bool CanStartFortifying()
        {
            return true;
        }

        public void StartFortifying()
        {
            throw new NotImplementedException();
        }

        public virtual bool CanFortify(ILocation location)
        {
            return false;
        }

        public virtual void Fortify(ILocation location, int armies)
        {
            throw new NotSupportedException();
        }
    }
}