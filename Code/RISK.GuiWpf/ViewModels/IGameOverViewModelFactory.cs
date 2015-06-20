﻿using RISK.Application;

namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModelFactory
    {
        GameOverViewModel Create(IPlayer winner);
    }
}