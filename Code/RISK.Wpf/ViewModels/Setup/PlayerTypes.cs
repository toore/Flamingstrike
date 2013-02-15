using System.Collections.Generic;
using System.Linq;

namespace GuiWpf.ViewModels.Setup
{
    public class PlayerTypes : IPlayerTypes
    {
        public PlayerTypes()
        {
            Values = Create().ToList();
        }

        public List<PlayerTypeBase> Values { get; private set; }

        private static IEnumerable<PlayerTypeBase> Create()
        {
            yield return new HumanPlayerType();
            yield return new NeutralPlayerType();
        }
    }
}