﻿using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new HumanPlayer(playerSetupViewModel.Name);
        }
    }
}