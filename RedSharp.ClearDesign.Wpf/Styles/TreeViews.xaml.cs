using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RedSharp.ClearDesign.Wpf.Styles
{
    public partial class TreeViews : ResourceDictionary
    {
        public TreeViews() => InitializeComponent();

        private void OnTreeViewItemInitialized(object sender, EventArgs arguments)
        {
            var layersContainer = sender as FrameworkElement;
            var targetItem = (TreeViewItem)layersContainer.TemplatedParent;

            var level = 0;
            
            var parent = ItemsControl.ItemsControlFromItemContainer(targetItem) as TreeViewItem;

            while (parent != null) 
            {
                parent = ItemsControl.ItemsControlFromItemContainer(parent) as TreeViewItem;

                level++;
            }

            if (level != 0)
                layersContainer.Margin = new Thickness(level * 30, 0, 0, 0);
        }
    }
}
