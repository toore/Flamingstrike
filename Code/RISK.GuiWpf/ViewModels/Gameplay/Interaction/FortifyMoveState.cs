﻿using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveState : IInteractionState
    {
        public FortifyMoveState(InteractionStateFactory interactionStateFactory)
        {
            throw new NotImplementedException();
        }

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
        }

        public void OnClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
        }

        //public FortifyMoveState(ITerritory selectedTerritory)
        //{
        //    SelectedTerritory = selectedTerritory;
        //}

        //public ITerritory SelectedTerritory { get; }

        //public bool CanClick(ITerritory territory)
        //{
        //    return true;

        //    //return
        //    //    SelectedTerritory.IsBordering(territory)
        //    //        &&
        //    //    territory.Occupant == Player;
        //}

        //public void OnClick(ITerritory territory)
        //{
        //    throw new NotImplementedException();
        //}

        ////public bool CanFortify(ILocation location)
        ////{
        ////    return SelectedTerritory.Location.IsBordering(location) 
        ////        && 
        ////        _worldMap.GetTerritory(location).Occupant == Player;
        ////}

        ////public void Fortify(ILocation location, int armies)
        ////{
        ////    _worldMap.GetTerritory(location).Armies += armies;
        ////    SelectedTerritory.Armies -= armies;

        ////    _stateController.CurrentState = _interactionStateFactory.CreateFortifiedState(Player, _worldMap);
        ////}
    }
}