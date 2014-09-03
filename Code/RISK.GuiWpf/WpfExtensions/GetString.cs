using System;
using System.Windows.Markup;
using GuiWpf.Services;

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
            var provideValue = LanguageResources.Instance.GetString(Key);

            return provideValue;
        }
    }
}