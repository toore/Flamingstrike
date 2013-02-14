using System.Collections.Generic;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels
{
    public class GameSetup
    {
        public IEnumerable<IPlayer> Players { get; set; }
    }
}