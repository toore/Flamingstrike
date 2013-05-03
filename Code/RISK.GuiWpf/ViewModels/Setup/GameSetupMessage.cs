using System.Collections.Generic;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupMessage
    {
        public IEnumerable<IPlayer> Players { get; set; }
    }
}