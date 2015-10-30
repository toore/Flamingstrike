using System.Windows;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public abstract class TerritoryModelBase : ITerritoryModel
    {
        protected TerritoryModelBase(ITerritoryId territoryId)
        {
            TerritoryId = territoryId;
        }

        public ITerritoryId TerritoryId { get; }

        public abstract string Name { get; }
        public abstract Point NamePosition { get; }
        public abstract string Path { get; }
    }
}