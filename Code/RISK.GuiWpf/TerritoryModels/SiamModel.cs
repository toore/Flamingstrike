using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class SiamModel : TerritoryModelBase
    {
        public SiamModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.SIAM; }
        }

        public override Point NamePosition
        {
            get { return new Point(1060, 320); }
        }

        public override string Path
        {
            get { return "m 1052.0738 293.73338 55.0534 33.33503 l 13.132 56.06347 -17.6777 14.64721 -24.2437 -24.24366 23.2336 63.13453 -19.1929 -11.11167 -50.0026 -102.53049 z"; }
        }
    }
}