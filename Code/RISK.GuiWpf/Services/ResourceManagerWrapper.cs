using GuiWpf.Properties;

namespace GuiWpf.Services
{
    public class ResourceManagerWrapper : IResourceManagerWrapper
    {
        public string GetString(string key)
        {
            var provideValue = Resources.ResourceManager.GetString(key);

            return provideValue;
        }
    }
}