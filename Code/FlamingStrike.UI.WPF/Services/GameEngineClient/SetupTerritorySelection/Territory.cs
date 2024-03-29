﻿namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection
{
    public class Territory
    {
        public Territory(Region region, string player, int armies, bool isSelectable)
        {
            Region = region;
            Player = player;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public Region Region { get; }
        public string Player { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}