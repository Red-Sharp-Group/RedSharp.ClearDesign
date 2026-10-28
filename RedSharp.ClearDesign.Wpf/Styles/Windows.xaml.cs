using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using RedSharp.ClearDesign.Wpf.Helpers;

namespace RedSharp.ClearDesign.Wpf.Styles
{
    public partial class Windows : ResourceDictionary
    {
        private const int MoveLimit = 5;

        //Not a best solution, possible bad because it is the same for all windows,
        //but I don't think that your level of magic so high that you can move two windows at the same time
        private Point? _lastPosition;

        private void MinimizeButtonClick(object sender, RoutedEventArgs arguments)
        {
            var window = ControlsHelper.FindParent<Window>(sender as Button);

            if (window.WindowState != WindowState.Minimized)
                window.WindowState = WindowState.Minimized;
        }

        private void MaximizeButtonClick(object sender, RoutedEventArgs arguments)
        {
            var window = ControlsHelper.FindParent<Window>(sender as Button);

            TryMaximizeMinimize(window);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs arguments)
        {
            var window = ControlsHelper.FindParent<Window>(sender as Button);

            window.Close();
        }

        private void TitleMouseDown(object sender, MouseButtonEventArgs arguments)
        {
            if (arguments.LeftButton != MouseButtonState.Pressed || arguments.ClickCount != 2)
                return;

            var window = ControlsHelper.FindParent<Window>(sender as Rectangle);

            TryMaximizeMinimize(window);
        }

        private void TitleMouseMove(object sender, MouseEventArgs arguments)
        {
            if (arguments.LeftButton != MouseButtonState.Pressed)
                return;

            var window = ControlsHelper.FindParent<Window>(sender as Rectangle);

            if (!_lastPosition.HasValue)
            {
                _lastPosition = arguments.GetPosition(window);
            }
            else if (Math.Abs(_lastPosition.Value.X) > MoveLimit || Math.Abs(_lastPosition.Value.Y) > MoveLimit)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    var windowPosition = window.PointToScreen(new Point(0, 0));

                    var source = PresentationSource.FromVisual(window);

                    var dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                    var dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;

                    windowPosition.X = windowPosition.X * 96.0 / dpiX;
                    windowPosition.Y = windowPosition.Y * 96.0 / dpiY;

                    var windowSize = new Size(window.ActualWidth, window.ActualHeight);
                    var mousePosition = arguments.GetPosition(window);

                    var globalMousePosition = new Point(mousePosition.X + windowPosition.X, mousePosition.Y + windowPosition.Y);

                    window.WindowState = WindowState.Normal;

                    var newWindowSize = new Size(window.ActualWidth, window.ActualHeight);

                    window.Left = globalMousePosition.X - (mousePosition.X * newWindowSize.Width / windowSize.Width);
                    window.Top = windowPosition.Y + 7;//this magic number is actually padding that is used in maximized state
                }

                window.DragMove();
            }
        }

        private void TitleMouseUp(object sender, MouseButtonEventArgs arguments)
        {
            _lastPosition = null;
        }

        private bool TryMaximizeMinimize(Window window)
        {
            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowState = WindowState.Normal;

            return true;
        }
    }
}
