using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TickTackToe
{
    public partial class Field : UserControl
    {
        bool _circle = false, _cross = false;

        MainWindow Game;

        //Changes shape when field has circle
        public bool Circle {
            get
            {
                return _circle;
            }
            set
            {
                if (Cross)
                    return;

                _circle = value;

                Shape shape = new Ellipse()
                {
                    Width = 100,
                    Height = 100,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 5,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Btn.Content = shape;
            }
        }

        //Changes shape to cross
        public bool Cross {
            get { return _cross; }
            set
            {
                if (Circle)
                    return;

                _cross = value;

                GeometryGroup geometry = new GeometryGroup();
                geometry.Children.Add(
                    new LineGeometry()
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(140, 140),
                    }
                    );
                geometry.Children.Add(
                    new LineGeometry()
                    {
                        StartPoint = new Point(140, 0),
                        EndPoint = new Point(0, 140),
                    }
                    );
                Path path = new Path()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 3
                };
                path.Data = geometry;
                Btn.Content = path;

            }
        }

        public Field()
        {
            InitializeComponent();
            Game = (MainWindow)Application.Current.MainWindow;

        }

        //Player makes a move and ai follows
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cross = true;
            Game.MakeMove();
        }
    }
}
