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
using System.Windows.Shapes;

namespace AT03___Model.Views
{
    /// <summary>
    /// Interaction logic for DrawADressView.xaml
    /// </summary>
    public partial class DrawADressView : Window
    {
        private Point _originalPosition;
        private Point _currentPosition;
        private Shape _currentShape;
        private bool _isDrawing = false;
        public DrawADressView()
        {
            InitializeComponent();
            MouseDown += new MouseButtonEventHandler(DrawTree_MouseDown);
            MouseMove += new MouseEventHandler(DrawTree_MouseMove);
            MouseUp += new MouseButtonEventHandler(DrawTree_MouseUp);
        }

        private void DrawTree_MouseDown(object s, MouseButtonEventArgs e)
        {
            _originalPosition = e.GetPosition(canvas);
            _currentShape = new Rectangle();

            Canvas.SetLeft(_currentShape, _originalPosition.X);
            Canvas.SetTop(_currentShape, _originalPosition.Y);
            _currentShape.Width = 1.5;
            _currentShape.Height = 1.5;
            _currentShape.Fill = new SolidColorBrush(Colors.Green);

            CaptureMouse();
            canvas.Children.Add(_currentShape);
            _isDrawing = true;
        }

        private void DrawTree_MouseMove(object s, MouseEventArgs e)
        {
            _currentPosition = e.GetPosition(canvas);
            tbx_XPosition.Text = _currentPosition.X.ToString("F0");
            tbx_YPosition.Text = _currentPosition.Y.ToString("F0");

            if (!_isDrawing) return;

            var width = _currentPosition.X - _originalPosition.X;
            var height = _currentPosition.Y - _originalPosition.Y;

            _currentShape.Width = (width > 1 ? width : 2);
            _currentShape.Height = (height > 1 ? height : 2);
        }

        private void DrawTree_MouseUp(object s, MouseButtonEventArgs e)
        {
            _isDrawing = false;
            ReleaseMouseCapture();
        }
    }
}
