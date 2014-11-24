﻿using System;
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

    public static class PropertyChangedBaseExtensions
    {
        public static void NotifyOfPropertyChange<TParent, TProperty>(this TParent source, TProperty value, Expression<Func<TProperty>> propertyGetter, Action<TProperty> propertySetter)
            where TParent : PropertyChangedBase
        {
            var currentValue = propertyGetter.Compile().Invoke();
            if (Equals(value, currentValue))
            {
                return;
            }
            propertySetter(value);

            source.NotifyOfPropertyChange(propertyGetter);
        }
    }
}