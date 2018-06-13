using Microsoft.AspNetCore.SignalR.Client;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class ArmyPlacerProxy : IArmyPlacer
    {
        private readonly HubConnection _connection;

        public ArmyPlacerProxy(HubConnection connection)
        {
            _connection = connection;
        }

        public async void PlaceArmyInRegion(Region selectedRegion)
        {
            await _connection.SendAsync("PlaceArmyInRegion", selectedRegion.MapToDto());
        }
    }
}