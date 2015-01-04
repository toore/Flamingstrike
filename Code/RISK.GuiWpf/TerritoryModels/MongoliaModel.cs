using System.Windows;
using GuiWpf.Properties;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public class MongoliaModel : TerritoryModelBase
    {
        public MongoliaModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.MONGOLIA; }
        }

        public override Point NamePosition
        {
            get { return new Point(1100, 170); }
        }

        public override string Path
        {
            get { return "m 1110.6627 214.4364 67.6802 41.92134 -5.5558 -75.76145 -184.35283 -16.66751 -9.09138 11.61675 69.70051 51.51778 z"; }
        }
    }
}