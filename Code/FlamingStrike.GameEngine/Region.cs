namespace FlamingStrike.GameEngine
{
    public class Region
    {
        public static readonly Region Alaska = new Region(Continent.NorthAmerica);

        public static readonly Region Alberta = new Region(Continent.NorthAmerica);

        public static readonly Region CentralAmerica = new Region(Continent.NorthAmerica);

        public static readonly Region EasternUnitedStates = new Region(Continent.NorthAmerica);

        public static readonly Region Greenland = new Region(Continent.NorthAmerica);

        public static readonly Region NorthwestTerritory = new Region(Continent.NorthAmerica);

        public static readonly Region Ontario = new Region(Continent.NorthAmerica);

        public static readonly Region Quebec = new Region(Continent.NorthAmerica);

        public static readonly Region WesternUnitedStates = new Region(Continent.NorthAmerica);

        public static readonly Region Argentina = new Region(Continent.SouthAmerica);

        public static readonly Region Brazil = new Region(Continent.SouthAmerica);

        public static readonly Region Peru = new Region(Continent.SouthAmerica);

        public static readonly Region Venezuela = new Region(Continent.SouthAmerica);

        public static readonly Region GreatBritain = new Region(Continent.Europe);

        public static readonly Region Iceland = new Region(Continent.Europe);

        public static readonly Region NorthernEurope = new Region(Continent.Europe);

        public static readonly Region Scandinavia = new Region(Continent.Europe);

        public static readonly Region SouthernEurope = new Region(Continent.Europe);

        public static readonly Region Ukraine = new Region(Continent.Europe);

        public static readonly Region WesternEurope = new Region(Continent.Europe);

        public static readonly Region Congo = new Region(Continent.Africa);

        public static readonly Region EastAfrica = new Region(Continent.Africa);

        public static readonly Region Egypt = new Region(Continent.Africa);

        public static readonly Region Madagascar = new Region(Continent.Africa);

        public static readonly Region NorthAfrica = new Region(Continent.Africa);

        public static readonly Region SouthAfrica = new Region(Continent.Africa);

        public static readonly Region Afghanistan = new Region(Continent.Asia);

        public static readonly Region China = new Region(Continent.Asia);

        public static readonly Region India = new Region(Continent.Asia);

        public static readonly Region Irkutsk = new Region(Continent.Asia);

        public static readonly Region Japan = new Region(Continent.Asia);

        public static readonly Region Kamchatka = new Region(Continent.Asia);

        public static readonly Region MiddleEast = new Region(Continent.Asia);

        public static readonly Region Mongolia = new Region(Continent.Asia);

        public static readonly Region Siam = new Region(Continent.Asia);

        public static readonly Region Siberia = new Region(Continent.Asia);

        public static readonly Region Ural = new Region(Continent.Asia);

        public static readonly Region Yakutsk = new Region(Continent.Asia);

        public static readonly Region EasternAustralia = new Region(Continent.Australia);

        public static readonly Region Indonesia = new Region(Continent.Australia);

        public static readonly Region NewGuinea = new Region(Continent.Australia);

        public static readonly Region WesternAustralia = new Region(Continent.Australia);

        private Region(Continent continent)
        {
            Continent = continent;
        }

        public Continent Continent { get; }
    }
}