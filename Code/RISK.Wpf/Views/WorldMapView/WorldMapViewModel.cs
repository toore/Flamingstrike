﻿using System.Collections.Generic;
using GuiWpf.Views.WorldMapView.Territories;

namespace GuiWpf.Views.WorldMapView
{
    public class WorldMapViewModel
    {
        public IEnumerable<IWorldMapViewModel> WorldMapViewModels { get; set; }
    }
}