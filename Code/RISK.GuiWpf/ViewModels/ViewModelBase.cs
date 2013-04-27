using System;
using System.Linq.Expressions;
using Caliburn.Micro;

namespace GuiWpf.ViewModels
{
    public class ViewModelBase : PropertyChangedBase
    {
        protected void NotifyOfPropertyChange<T>(T value, Expression<Func<T>> propertyGetter, Action<T> propertySetter)
        {
            var currentValue = propertyGetter.Compile().Invoke();
            if (Equals(value, currentValue))
            {
                return;
            }
            propertySetter(value);

            NotifyOfPropertyChange(propertyGetter);
        }
    }
}