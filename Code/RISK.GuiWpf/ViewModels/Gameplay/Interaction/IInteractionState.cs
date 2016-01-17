﻿using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IStateController stateController, ITerritoryGeography selectedTerritoryGeography);
        void OnClick(IStateController stateController,ITerritoryGeography territoryGeography);
    }
}