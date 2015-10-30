using System.Windows;
using GuiWpf.Properties;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class AlaskaModel : TerritoryModelBase
    {
        public AlaskaModel(ITerritoryId alaska)
            : base(alaska) {}

        public override string Name => Resources.ALASKA;
        public override Point NamePosition => new Point(50, 40);
        public override string Path => "m 12.121866 139.68511 41.67913 -42.115279 12.86906 -25.564934 50.513114 -8.563276 40.40058 8.563276 -45.57044 44.381753 5.76544 11.7349 -44.064802 -14.83465 z";
    }
}