using System.Windows;
using RISK.Properties;

namespace RISK.WorldMap.Territories
{
    public class CentralAmericaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.CENTRAL_AMERICA; }
        }

        public override Point NamePosition
        {
            get { return new Point(97.984797, 251.81205); }
        }

        public override string Path
        {
            get
            {
                return "m 97.984797 251.81205 39.395953 84.0952 120.46069 67.93276 6.81853 -7.32361 -43.6891 -69.44799 -43.6891 2.77792 4.04061 -29.04188 -33.08249 -30.55712 z" +
                       "m 135.360443 67.43783 19.44543 -6.81853 65.40738 29.54697 -19.1929 12.87944 z";
            }
        }
    }
}