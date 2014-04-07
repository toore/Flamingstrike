﻿using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapViewModel
    {
        public WorldMapViewModel()
        {
            WorldMapViewModels = new ObservableCollection<IWorldMapItemViewModel>();
        }

        public ObservableCollection<IWorldMapItemViewModel> WorldMapViewModels { get; private set; }
    }
}