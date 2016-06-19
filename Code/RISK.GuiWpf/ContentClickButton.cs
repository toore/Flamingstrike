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
                // When mouse event args has been handled by ButtonBase, it means that 
                // IsPressed has been updated. 
                // Because ButtonBase only checks a rectangle, we need to check if 
                // mouse is really over any child, and update IsPressed accordingly.
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