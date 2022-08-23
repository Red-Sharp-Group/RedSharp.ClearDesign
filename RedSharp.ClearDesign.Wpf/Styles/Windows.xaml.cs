using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

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
    }
}
