using GuiWpf.Properties;

namespace GuiWpf.ViewModels.Setup
{
    public class HumanPlayerType : PlayerTypeBase
    {
        public override string Name
        {
            get { return Resources.HUMAN; }
        }
    }
}