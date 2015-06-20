using System;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveState : IInteractionState
    {
        public FortifyMoveState(IPlayer player, ITerritory selectedTerritory)
        {
            Player = player;
            SelectedTerritory = selectedTerritory;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }
        public bool CanClick(ITerritory territory)
        {
            return true;

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