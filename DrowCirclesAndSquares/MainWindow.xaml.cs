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

namespace DrowCirclesAndSquares
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Square square1 = new Square(DrawCanvas);
            Circle circle1 = new Circle(DrawCanvas);
            square1.Draw();
            circle1.Draw();

            for (int i = 0; i < 100; i++)
            {
                Draw(new Square(DrawCanvas));
                Draw(new Circle(DrawCanvas));
            }
        }

        private void Draw(Shape shape)
        {
            shape.Draw();
        }
    }
}
