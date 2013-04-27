using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface ILocationProvider
    {
        IEnumerable<ILocation> GetAll();

        ILocation Alaska { get; }
        ILocation Alberta { get; }
        ILocation CentralAmerica { get; }
        ILocation EasternUnitedStates { get; }
        ILocation Greenland { get; }
        ILocation NorthwestTerritory { get; }
        ILocation Ontario { get; }
        ILocation Quebec { get; }
        ILocation WesternUnitedStates { get; }
        ILocation Argentina { get; }
        ILocation Brazil { get; }
        ILocation Peru { get; }
        ILocation Venezuela { get; }
        ILocation GreatBritain { get; }
        ILocation Iceland { get; }
        ILocation NorthernEurope { get; }
        ILocation Scandinavia { get; }
        ILocation SouthernEurope { get; }
        ILocation Ukraine { get; }
        ILocation WesternEurope { get; }
        ILocation Congo { get; }
        ILocation EastAfrica { get; }
        ILocation Egypt { get; }
        ILocation Madagascar { get; }
        ILocation NorthAfrica { get; }
        ILocation SouthAfrica { get; }
        ILocation Afghanistan { get; }
        ILocation China { get; }
        ILocation India { get; }
        ILocation Irkutsk { get; }
        ILocation Japan { get; }
        ILocation Kamchatka { get; }
        ILocation MiddleEast { get; }
        ILocation Mongolia { get; }
        ILocation Siam { get; }
        ILocation Siberia { get; }
        ILocation Ural { get; }
        ILocation Yakutsk { get; }
        ILocation EasternAustralia { get; }
        ILocation Indonesia { get; }
        ILocation NewGuinea { get; }
        ILocation WesternAustralia { get; }
    }
}