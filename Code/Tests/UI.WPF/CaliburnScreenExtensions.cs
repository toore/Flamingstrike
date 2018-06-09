using System;
using Caliburn.Micro;

namespace Tests.UI.WPF
{
    public static class CaliburnScreenExtensions
    {
        public static void Activate(this IScreen screen)
        {
            IActivate parent = new Parent();
            screen.ActivateWith(parent);
            parent.Activate();
        }
    }

    public class Parent : IActivate
    {
        public void Activate()
        {
            Activated?.Invoke(this, new ActivationEventArgs());
            IsActive = true;
        }

        public bool IsActive { get; private set; }

        public event EventHandler<ActivationEventArgs> Activated;
    }
}