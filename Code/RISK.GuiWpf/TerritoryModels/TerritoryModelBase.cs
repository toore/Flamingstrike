﻿using System.Windows;
using RISK.Application;

namespace GuiWpf.TerritoryModels
{
    public abstract class TerritoryModelBase : ITerritoryModel
    {
        protected TerritoryModelBase(ITerritory territory)
        {
            Territory = territory;
        }

        public ITerritory Territory { get; private set; }

        public abstract string Name { get; }
        public abstract Point NamePosition { get; }

        public abstract string Path { get; }
    }
}