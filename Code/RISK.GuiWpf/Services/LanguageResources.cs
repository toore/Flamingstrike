using GuiWpf.Properties;

namespace GuiWpf.Services
{
    public interface ILanguageResources
    {
        string GetString(string key);
    }

    public class LanguageResources : ILanguageResources
    {
        private static ILanguageResources _instance;
        public static ILanguageResources Instance
        {
            get { return _instance ?? (_instance = new LanguageResources()); }
            set { _instance = value; }
        }

        public string GetString(string key)
        {
            var provideValue = Resources.ResourceManager.GetString(key);

            return provideValue;
        }
    }
}