using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class AttackProxy : IAttackPhase
    {
        private readonly HubConnection _hubConnection;

        public AttackProxy(HubConnection hubConnection, string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, IReadOnlyList<Region> regionsThatCanBeSourceForAttackOrFortification)
        {
            _hubConnection = hubConnection;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            RegionsThatCanBeSourceForAttackOrFortification = regionsThatCanBeSourceForAttackOrFortification;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
        public IReadOnlyList<Region> RegionsThatCanBeSourceForAttackOrFortification { get; }

        public void Attack(Region attackingRegion, Region defendingRegion)
        {
            _hubConnection.SendAsync(
                "Attack", new
                    {
                        AttackingRegion = attackingRegion.MapToDto(),
                        DefendingRegion = defendingRegion.MapToDto()
                    });
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            _hubConnection.SendAsync(
                "Fortify", new
                    {
                        SourceRegion = sourceRegion.MapToDto(),
                        DestinationRegion = destinationRegion.MapToDto(),
                        Armies = armies
                    });
        }

        public async Task<IEnumerable<Region>> GetRegionsThatCanBeAttacked(Region sourceRegion)
        {
            return await _hubConnection.InvokeAsync<IEnumerable<Region>>("GetRegionsThatCanBeAttacked", sourceRegion.MapToDto());
        }

        public async Task<IEnumerable<Region>> GetRegionsThatCanBeFortified(Region sourceRegion)
        {
            return await _hubConnection.InvokeAsync<IEnumerable<Region>>("GetRegionsThatCanBeFortified", sourceRegion.MapToDto());
        }

        public void EndTurn()
        {
            _hubConnection.SendAsync("EndTurn");
        }
    }
}