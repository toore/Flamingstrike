using System.Collections.Generic;

namespace GuiWpf.ViewModels.Setup
{
    public interface IPlayerTypesFactory
    {
        IEnumerable<PlayerTypeBase> Create();
    }
}