using System;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public static class GameEngineProxyExtensions
    {
        public static string MapToDto(this Region region)
        {
            return Enum.GetName(typeof(Region), region);
        }
    }
}