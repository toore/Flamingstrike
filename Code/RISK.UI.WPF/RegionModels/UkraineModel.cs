using System.Windows;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class UkraineModel : RegionModelBase
    {
        public UkraineModel(IRegion region) : base(region) {}

        public override string Name => Resources.UKRAINE;

        public override Point NamePosition => new Point(750, 100);

        public override string Path => "m 708.13725 142.21049 24.74873 -26.26396 -1.01015 -48.992395 112.12693 6.565991 20.70811 91.923884 -63.63959 47.98224 -61.6193 -8.58629 -23.23351 -17.17259 z";
    }
}