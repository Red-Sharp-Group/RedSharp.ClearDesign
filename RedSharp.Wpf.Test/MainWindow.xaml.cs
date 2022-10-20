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
using RedSharp.ClearDesign.Utils;
using Definitions = RedSharp.ClearDesign.Definitions;

namespace RedSharp.Wpf.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResourceSchema _testGridResourceSchema;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources[Definitions.Colors.PrimaryColorKey] = Color.FromRgb(218, 36, 123);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(_testGridResourceSchema == null)
            {
                _testGridResourceSchema = new ResourceSchema(TestGrid.Resources);
                _testGridResourceSchema.InitializeFromInternalDictionary();
            }

            TestGrid.Resources[Definitions.Colors.PrimaryColorKey] = Color.FromRgb(218, 36, 123);
        }
    }
}
