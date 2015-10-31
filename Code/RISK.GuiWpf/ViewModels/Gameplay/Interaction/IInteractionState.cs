﻿using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IStateController stateController, ITerritoryId territoryId);
        void OnClick(IStateController stateController,ITerritoryId territoryId);
    }
}