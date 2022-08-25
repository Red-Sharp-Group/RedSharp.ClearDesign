using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RedSharp.ClearDesign.Styles
{
    public partial class Windows : ResourceDictionary
    {
        private void MinimizeButtonClick(Object sender, RoutedEventArgs arguments)
        {
            var window = FindParent<Window>(sender as Button);

            if (window.WindowState != WindowState.Minimized)
                window.WindowState = WindowState.Minimized;
        }

        private void MaximizeButtonClick(Object sender, RoutedEventArgs arguments)
        {
            var window = FindParent<Window>(sender as Button);

            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowState = WindowState.Normal;
        }

        private void CloseButtonClick(Object sender, RoutedEventArgs arguments)
        {
            var window = FindParent<Window>(sender as Button);

            window.Close();
        }

        private TType FindParent<TType>(DependencyObject child) where TType : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;
            else if (parent is TType)
                return (TType)parent;
            else
                return FindParent<TType>(parent);
        }

        private void TitleMouseDown(Object sender, MouseButtonEventArgs arguments)
        {
            if (arguments.LeftButton != MouseButtonState.Pressed || arguments.ClickCount != 2)
                return;

            var window = FindParent<Window>(sender as Rectangle);

            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowState = WindowState.Normal;
        }

        private void TitleMouseMove(Object sender, MouseEventArgs arguments)
        {
            if (arguments.LeftButton != MouseButtonState.Pressed)
                return;

            var window = FindParent<Window>(sender as Rectangle);


            if (window.WindowState == WindowState.Maximized)
            {
                //TODO this is not ok to move it to zero position.
                var windowPosition = new Point(0, 0);
                var windowSize = new Size(window.ActualWidth, window.ActualHeight);
                var mousePosition = arguments.GetPosition(window);

                var globalMousePosition = new Point(mousePosition.X + windowPosition.X, mousePosition.Y + windowPosition.Y);

                window.WindowState = WindowState.Normal;

                var newWindowSize = new Size(window.ActualWidth, window.ActualHeight);

                window.Left = globalMousePosition.X - (mousePosition.X * newWindowSize.Width / windowSize.Width);
                window.Top = windowPosition.Y;
            }

            window.DragMove();
        }
    }
}
