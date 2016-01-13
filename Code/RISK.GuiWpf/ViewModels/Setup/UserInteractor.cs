﻿using System.Threading;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritoryId WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter);
        void SelectTerritory(ITerritoryId territoryId);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritoryId _selectedTerritoryId;
        private readonly AutoResetEvent _territoryIdHasBeenSet = new AutoResetEvent(false);

        public ITerritoryId WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter)
        {
            _territoryIdHasBeenSet.WaitOne();
            return _selectedTerritoryId;
        }

        public void SelectTerritory(ITerritoryId territoryId)
        {
            _selectedTerritoryId = territoryId;
            _territoryIdHasBeenSet.Set();
        }
    }
}