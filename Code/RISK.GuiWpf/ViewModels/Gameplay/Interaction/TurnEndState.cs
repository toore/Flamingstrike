﻿using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class TurnEndState : IInteractionState
    {
        public IPlayerId PlayerId { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }
        public bool CanClick(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }

        public void OnClick(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }
    }
}