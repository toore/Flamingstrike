using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GuiWpf
{
    public class ContentClickButton : Button
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Handled)
            {
                // Handled by ButtonBase, means that IsPressed has been updated. 
                // We might need to correct, since ButtonBase checks rectangle-size only
                var isMouseOverChild = IsMouseOverChild(e);

                IsPressed = isMouseOverChild;
            }
        }

        private bool IsMouseOverChild(MouseEventArgs e)
        {
            var visualChild = GetVisualChild(0);
            if (visualChild == null)
            {
                return true;
            }
            var inputElement = (IInputElement)visualChild;
            var position = e.GetPosition(inputElement);

            var isVisualChildHit = VisualTreeHelper.HitTest(visualChild, position) != null;

            return isVisualChildHit;
        }
    }
}