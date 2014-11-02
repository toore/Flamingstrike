using System.Collections.Generic;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Messages
{
    public class GameSetupMessage
    {
        public IEnumerable<IPlayer> Players { get; set; }
    }
}