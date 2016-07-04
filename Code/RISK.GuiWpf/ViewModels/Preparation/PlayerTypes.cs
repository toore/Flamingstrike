using System.Collections.Generic;
using System.Linq;

namespace GuiWpf.ViewModels.Preparation
{
    public class PlayerTypes : IPlayerTypes
    {
        public PlayerTypes()
        {
            Values = Create().ToList();
        }

        public List<PlayerTypeBase> Values { get; }

        private static IEnumerable<PlayerTypeBase> Create()
        {
            yield return new HumanPlayerType();
            yield return new NeutralPlayerType();
        }
    }
}