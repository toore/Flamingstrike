using GuiWpf.Properties;

namespace GuiWpf.ViewModels.Setup
{
    public class NeutralPlayerType : PlayerTypeBase
    {
        public override string Name
        {
            get { return Resources.NEUTRAL; }
        }
    }
}