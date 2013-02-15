using GuiWpf.Infrastructure;

namespace GuiWpf.Views
{
    public class TestDataFactoryBase
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public TestDataFactoryBase()
        {
            _pluginConfiguration = new PluginConfiguration();
            _pluginConfiguration.Configure();
        }

        protected T Create<T>()
        {
            return _pluginConfiguration.GetInstance<T>();
        }
    }
}