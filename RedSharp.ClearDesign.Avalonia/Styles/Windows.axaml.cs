using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Rendering;
using Avalonia.Win32;

namespace RedSharp.ClearDesign.Styles
{
    public partial class Windows : ResourceDictionary
    {
        public void OnTitlePointerPressed(Object? sender, PointerPressedEventArgs arguments)
        {
            if (!IsLeftButtonPressed(sender, arguments))
                return;

            if (!TryGetWindow(sender, out Window window))
                return;

            var state = window.WindowState;

            window.BeginMoveDrag(arguments);

            if (state != window.WindowState)
                window.InvalidateVisual();
        }

        public void OnMinimizeClick(Object? sender, RoutedEventArgs arguments)
        {
            if (TryGetWindow(sender, out Window window))
                window.WindowState = WindowState.Minimized;
        }

        public void OnRestoreClick(Object? sender, RoutedEventArgs arguments)
        {
            if (!TryGetWindow(sender, out Window window))
                return;

            switch (window.WindowState)
            {
                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;

                case WindowState.Normal:
                    window.WindowState = WindowState.Maximized;
                    break;
            }
        }

        public void OnCloseClick(Object? sender, RoutedEventArgs arguments)
        {
            if (TryGetWindow(sender, out Window window))
                window.Close();
        }

        private bool IsLeftButtonPressed(Object sender, PointerPressedEventArgs arguments)
        {
            var inputElement = sender as InputElement;

            if (inputElement == null)
                return false;

            return arguments.GetCurrentPoint(sender as InputElement).Properties.IsLeftButtonPressed;
        }

        private bool TryGetWindow(Object sender, out Window window)
        {
            window = null;

            var inputElement = sender as InputElement;

            if (inputElement == null)
                return false;

            window = inputElement.FindLogicalAncestorOfType<Window>();

            if (window == null)
                return false;

            return true;
        }
    }
}
