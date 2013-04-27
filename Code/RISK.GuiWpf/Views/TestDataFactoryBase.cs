using GuiWpf.Infrastructure;
using StructureMap;

namespace GuiWpf.Views
{
    public class TestDataFactoryBase
    {
        public TestDataFactoryBase()
        {
            new PluginConfiguration().Configure();
        }

        protected T Create<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}