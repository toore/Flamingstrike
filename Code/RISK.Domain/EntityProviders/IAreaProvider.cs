using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.EntityProviders
{
    public interface IAreaProvider
    {
        IEnumerable<IArea> GetAll();

        IArea Alaska { get; }
        IArea Alberta { get; }
        IArea CentralAmerica { get; }
        IArea EasternUnitedStates { get; }
        IArea Greenland { get; }
        IArea NorthwestTerritory { get; }
        IArea Ontario { get; }
        IArea Quebec { get; }
        IArea WesternUnitedStates { get; }
        IArea Argentina { get; }
        IArea Brazil { get; }
        IArea Peru { get; }
        IArea Venezuela { get; }
        IArea GreatBritain { get; }
        IArea Iceland { get; }
        IArea NorthernEurope { get; }
        IArea Scandinavia { get; }
        IArea SouthernEurope { get; }
        IArea Ukraine { get; }
        IArea WesternEurope { get; }
        IArea Congo { get; }
        IArea EastAfrica { get; }
        IArea Egypt { get; }
        IArea Madagascar { get; }
        IArea NorthAfrica { get; }
        IArea SouthAfrica { get; }
        IArea Afghanistan { get; }
        IArea China { get; }
        IArea India { get; }
        IArea Irkutsk { get; }
        IArea Japan { get; }
        IArea Kamchatka { get; }
        IArea MiddleEast { get; }
        IArea Mongolia { get; }
        IArea Siam { get; }
        IArea Siberia { get; }
        IArea Ural { get; }
        IArea Yakutsk { get; }
        IArea EasternAustralia { get; }
        IArea Indonesia { get; }
        IArea NewGuinea { get; }
        IArea WesternAustralia { get; }
    }
}