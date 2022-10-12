using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Utils;

namespace RedSharp.ClearDesign.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ColorsDefinition _colorsDefinition;
        private FontsDefinition _fontsDefinition;
        private SizesDefinition _sizesDefinition;

        public MainWindow()
        {
            InitializeComponent();

            //_colorsDefinition = new ColorsDefinition(Application.Current.Resources);
            //_fontsDefinition = new FontsDefinition(Application.Current.Resources);
            //_sizesDefinition = new SizesDefinition(Application.Current.Resources);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Definitions.Colors.PrimaryColor = Color.FromRgb(218, 36, 123);
        }
    }
}
