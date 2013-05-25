using System.Collections.Generic;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels
{
    public class GameSetupMessage
    {
        public IEnumerable<IPlayer> Players { get; set; }
    }
}