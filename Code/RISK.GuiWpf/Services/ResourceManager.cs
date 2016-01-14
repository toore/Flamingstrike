using GuiWpf.Properties;

namespace GuiWpf.Services
{
    public interface IResourceManager
    {
        string GetString(string key);
    }

    public class ResourceManager : IResourceManager
    {
        private static IResourceManager _instance;
        public static IResourceManager Instance
        {
            get { return _instance ?? (_instance = new ResourceManager()); }
            set { _instance = value; }
        }

        public string GetString(string key)
        {
            return Resources.ResourceManager.GetString(key);
        }
    }
}