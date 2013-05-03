using GuiWpf.Properties;

namespace GuiWpf.ViewModels.Settings
{
    public class NeutralPlayerType : PlayerTypeBase
    {
        public override string Name
        {
            get { return Resources.NEUTRAL; }
        }
    }
}