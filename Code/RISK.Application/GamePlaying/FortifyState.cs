using System;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class FortifyState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifyState(StateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            Player = player;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            return territory.Occupant == Player;
        }

        public void OnClick(ITerritory territory)
        {
            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, Player, territory);
        }
    }

    public class FortifyToTerritoryState : IInteractionState
    {
        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }
        public bool CanClick(ITerritory territory)
        {
            throw new NotImplementedException();

            //return
            //    SelectedTerritory.IsBordering(territory)
            //        &&
            //    territory.Occupant == Player;
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