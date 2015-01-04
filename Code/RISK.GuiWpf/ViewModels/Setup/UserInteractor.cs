﻿using System.Threading;
using RISK.Application.Entities;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritory GetLocation(ITerritorySelectorParameter territorySelector);
        void SelectLocation(ITerritory location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritory _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritory GetLocation(ITerritorySelectorParameter territorySelector)
        {
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectLocation(ITerritory location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}