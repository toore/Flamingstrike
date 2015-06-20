﻿using System.Threading;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritory GetSelectedTerritory(ITerritoryRequestParameter territoryRequestParameter);
        void SelectTerritory(ITerritory location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritory _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritory GetSelectedTerritory(ITerritoryRequestParameter territoryRequestParameter)
        {
            // TODO: territoryRequestParameter is not used!
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectTerritory(ITerritory location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}