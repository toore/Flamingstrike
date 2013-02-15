using System.Collections.Generic;

namespace GuiWpf.ViewModels.Setup
{
    public class PlayerTypesFactory : IPlayerTypesFactory
    {
        public IEnumerable<PlayerTypeBase> Create()
        {
            yield return new HumanPlayerType();
            yield return new NeutralPlayerType();
        }
    }
}