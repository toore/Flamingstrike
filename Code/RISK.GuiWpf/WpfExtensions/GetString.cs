using System;
using System.Windows.Markup;
using GuiWpf.Properties;

namespace GuiWpf.WpfExtensions
{
    public class GetString : MarkupExtension
    {
        public GetString(string key)
        {
            Key = key;
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValue = Resources.ResourceManager.GetString(Key);

            return provideValue;
        }
    }
}