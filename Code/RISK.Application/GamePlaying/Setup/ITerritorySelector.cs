﻿using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public interface ITerritorySelector
    {
        ITerritory SelectTerritory(ITerritorySelectorParameter territorySelectorParameter);
    }
}