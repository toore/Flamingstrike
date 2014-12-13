using System;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class FortifyState : IInteractionState
    {
        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            throw new NotImplementedException();
        }

        public void OnClick(ITerritory territory)
        {
            throw new NotImplementedException();
        }

        //public bool CanFortify(ILocation location)
        //{
        //    return SelectedTerritory.Location.IsBordering(location) 
        //        && 
        //        _worldMap.GetTerritory(location).Occupant == Player;
        //}

        //public void Fortify(ILocation location, int armies)
        //{
        //    _worldMap.GetTerritory(location).Armies += armies;
        //    SelectedTerritory.Armies -= armies;

        //    _stateController.CurrentState = _interactionStateFactory.CreateFortifiedState(Player, _worldMap);
        //}
    }
}