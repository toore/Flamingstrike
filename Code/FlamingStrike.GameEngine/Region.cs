namespace FlamingStrike.GameEngine
{
    public class Region
    {
        public static readonly Region Alaska = new Region(nameof(Alaska), Continent.NorthAmerica);

        public static readonly Region Alberta = new Region(nameof(Alberta), Continent.NorthAmerica);

        public static readonly Region CentralAmerica = new Region(nameof(CentralAmerica), Continent.NorthAmerica);

        public static readonly Region EasternUnitedStates = new Region(nameof(EasternUnitedStates), Continent.NorthAmerica);

        public static readonly Region Greenland = new Region(nameof(Greenland), Continent.NorthAmerica);

        public static readonly Region NorthwestTerritory = new Region(nameof(NorthwestTerritory), Continent.NorthAmerica);

        public static readonly Region Ontario = new Region(nameof(Ontario), Continent.NorthAmerica);

        public static readonly Region Quebec = new Region(nameof(Quebec), Continent.NorthAmerica);

        public static readonly Region WesternUnitedStates = new Region(nameof(WesternUnitedStates), Continent.NorthAmerica);

        public static readonly Region Argentina = new Region(nameof(Argentina), Continent.SouthAmerica);

        public static readonly Region Brazil = new Region(nameof(Brazil), Continent.SouthAmerica);

        public static readonly Region Peru = new Region(nameof(Peru), Continent.SouthAmerica);

        public static readonly Region Venezuela = new Region(nameof(Venezuela), Continent.SouthAmerica);

        public static readonly Region GreatBritain = new Region(nameof(GreatBritain), Continent.Europe);

        public static readonly Region Iceland = new Region(nameof(Iceland), Continent.Europe);

        public static readonly Region NorthernEurope = new Region(nameof(NorthernEurope), Continent.Europe);

        public static readonly Region Scandinavia = new Region(nameof(Scandinavia), Continent.Europe);

        public static readonly Region SouthernEurope = new Region(nameof(SouthernEurope), Continent.Europe);

        public static readonly Region Ukraine = new Region(nameof(Ukraine), Continent.Europe);

        public static readonly Region WesternEurope = new Region(nameof(WesternEurope), Continent.Europe);

        public static readonly Region Congo = new Region(nameof(Congo), Continent.Africa);

        public static readonly Region EastAfrica = new Region(nameof(EastAfrica), Continent.Africa);

        public static readonly Region Egypt = new Region(nameof(Egypt), Continent.Africa);

        public static readonly Region Madagascar = new Region(nameof(Madagascar), Continent.Africa);

        public static readonly Region NorthAfrica = new Region(nameof(NorthAfrica), Continent.Africa);

        public static readonly Region SouthAfrica = new Region(nameof(SouthAfrica), Continent.Africa);

        public static readonly Region Afghanistan = new Region(nameof(Afghanistan), Continent.Asia);

        public static readonly Region China = new Region(nameof(China), Continent.Asia);

        public static readonly Region India = new Region(nameof(India), Continent.Asia);

        public static readonly Region Irkutsk = new Region(nameof(Irkutsk), Continent.Asia);

        public static readonly Region Japan = new Region(nameof(Japan), Continent.Asia);

        public static readonly Region Kamchatka = new Region(nameof(Kamchatka), Continent.Asia);

        public static readonly Region MiddleEast = new Region(nameof(MiddleEast), Continent.Asia);

        public static readonly Region Mongolia = new Region(nameof(Mongolia), Continent.Asia);

        public static readonly Region Siam = new Region(nameof(Siam), Continent.Asia);

        public static readonly Region Siberia = new Region(nameof(Siberia), Continent.Asia);

        public static readonly Region Ural = new Region(nameof(Ural), Continent.Asia);

        public static readonly Region Yakutsk = new Region(nameof(Yakutsk), Continent.Asia);

        public static readonly Region EasternAustralia = new Region(nameof(EasternAustralia), Continent.Australia);

        public static readonly Region Indonesia = new Region(nameof(Indonesia), Continent.Australia);

        public static readonly Region NewGuinea = new Region(nameof(NewGuinea), Continent.Australia);

        public static readonly Region WesternAustralia = new Region(nameof(WesternAustralia), Continent.Australia);

        private Region(string name, Continent continent)
        {
            Name = name;
            Continent = continent;
        }

        public string Name { get; }

        public Continent Continent { get; }
    }
}