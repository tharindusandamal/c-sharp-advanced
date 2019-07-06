using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrowCirclesAndSquares
{
    public class Shape
    {
        Canvas _canvas;
        static Random _rand = new Random();
        protected UIElement _uIElement;

        public Shape(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Draw()
        {
            double left = _canvas.ActualWidth * _rand.NextDouble();
            double top = _canvas.ActualHeight * _rand.NextDouble();
            _uIElement.SetValue(Canvas.LeftProperty, left);
            _uIElement.SetValue(Canvas.TopProperty, top);
            _canvas.Children.Add(_uIElement);
        }
    }

    public class Circle : Shape
    {
        public Circle(Canvas canvas) 
            : base(canvas)
        {
            _uIElement = new Ellipse
            {
                Width = 100,
                Height = 100,
                Fill = new SolidColorBrush(Colors.Green),
                Stroke = new SolidColorBrush(Colors.Black)
            };
        }
    }

    public class Square : Shape
    {
        public Square(Canvas canvas)
            : base(canvas)
        {
            _uIElement = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = new SolidColorBrush(Colors.Green),
                Stroke = new SolidColorBrush(Colors.Black)
            };
        }
    }
}
