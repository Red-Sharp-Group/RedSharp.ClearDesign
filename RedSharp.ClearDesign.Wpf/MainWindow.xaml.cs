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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var color = Color.FromRgb(218, 36, 123);

            Application.Current.Resources["D_DefaultPrimaryColor_"] = color;
            Application.Current.Resources["D_PrimaryBorderColor_"] = ColorHelper.AlphaBlending(color, (Color)Application.Current.Resources["D_Dark5Color_"]);
            Application.Current.Resources["D_FocusBorderColor_"] = ColorHelper.AlphaBlending(color, (Color)Application.Current.Resources["D_Light5Color_"]);
        }
    }
}
