using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RedSharp.ClearDesign.Wpf.Test
{
    public class Item1
    {
        public string Name { get; set; }

        public List<Item2> Items { get; set; }
    }
    public class Item2
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> Data { get; } = new List<string>()
        {
            "Bla 1",
            "Bla 2",
            "Bla 3",
            "Bla 4",
            "Bla 5",
            "Bla 6",
            "Bla 7",
            "Bla 8",
            "Bla 9"
        };

        public static List<Item1> HData { get; } = new List<Item1>()
        {
            new Item1()
            {
                Name = "Bla 1",
                Items = new List<Item2>()
                {
                    new Item2() { Name = "Bla 1.1 "},
                    new Item2() { Name = "Bla 1.2 "}
                }
            },
            new Item1()
            {
                Name = "Bla 2",
                Items = new List<Item2>()
                {
                    new Item2() { Name = "Bla 2.1 "},
                    new Item2() { Name = "Bla 2.2 "},
                    new Item2() { Name = "Bla 2.3 "}
                }
            },
            new Item1()
            {
                Name = "Bla 3",
                Items = new List<Item2>()
                {
                    new Item2() { Name = "Bla 3.1 "},
                    new Item2() { Name = "Bla 3.2 "},
                    new Item2() { Name = "Bla 3.3 "},
                    new Item2() { Name = "Bla 3.4 "}
                }
            },
            new Item1()
            {
                Name = "Bla 4",
                Items = new List<Item2>()
                {
                    new Item2() { Name = "Bla 4.1 "},
                    new Item2() { Name = "Bla 4.2 "},
                    new Item2() { Name = "Bla 4.3 "},
                    new Item2() { Name = "Bla 4.4 "},
                    new Item2() { Name = "Bla 4.5 "},
                    new Item2() { Name = "Bla 4.6 "}
                }
            },
            new Item1()
            {
                Name = "Bla 5",
                Items = new List<Item2>()
                {
                    new Item2() { Name = "Bla 5.1 "},
                    new Item2() { Name = "Bla 5.2 "},
                    new Item2() { Name = "Bla 5.3 "}
                }
            }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}