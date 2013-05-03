using GuiWpf.Properties;

namespace GuiWpf.ViewModels.Settings
{
    public class HumanPlayerType : PlayerTypeBase
    {
        public override string Name
        {
            get { return Resources.HUMAN; }
        }
    }
}