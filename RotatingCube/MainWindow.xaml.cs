namespace RotatingCube
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private IModel model;
        private Projection projection = new Projection(500, 0, 0) { SizeFactor = 2 };
        private int angle;
        private bool showTriangles = true;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += OnTimer;
            timer.Start();
        }

        private void OnTimer(object sender, object state)
        {
            angle++;
            this.model = new TransformModel
            {
                Original = new TransformModel
                {
                    Original = new CubeModel(5, "x")
                    {
                        Front = Colors.Blue,
                        Back = Colors.Gray,
                        Bottom = Colors.Red,
                        Top = Colors.Purple,
                        Left = Colors.Yellow,
                        Right = Colors.Brown,
                    },
                    Transform = new XYRotationTransform { Angle = angle / 100.0 }
                },
                Transform = new XZRotationTransform { Angle = angle / 100.0 }
            };
            this.Dispatcher.BeginInvoke(new Action(() => { this.Render(this.model); }));
        }

        private void Render(IModel model)
        {
            this.projection.Width = this.ActualWidth;
            this.projection.Height = this.ActualHeight;
            this.Canvas.Children.Clear();
            this.Canvas.Children.Add(new Rectangle { Width = 500, Height = 500, Fill = new SolidColorBrush(Colors.Black) });

            BinarySpacePartitionTreeNode tree = BinarySpacePartitionTreeNode.Build(model.Triangles);

            foreach (Triangle triangle in tree.RenderingOrder(500, 0, 0))
            {
                DrawTriangle(triangle);
            }
        }

        private void DrawTriangle(Triangle triangle)
        {
            double vx1, vy1; this.projection.Project(triangle.X1, triangle.Y1, triangle.Z1, out vx1, out vy1);
            double vx2, vy2; this.projection.Project(triangle.X2, triangle.Y2, triangle.Z2, out vx2, out vy2);
            double vx3, vy3; this.projection.Project(triangle.X3, triangle.Y3, triangle.Z3, out vx3, out vy3);
            Path path = new Path
            {
                Fill = new SolidColorBrush(triangle.Fill),
                StrokeThickness = 1,
                Stroke = (showTriangles ? new SolidColorBrush(Colors.Beige) : new SolidColorBrush(triangle.Fill)),
                Data = new PathGeometry
                {
                    Figures =
                    {
                        new PathFigure 
                        {
                            StartPoint = new Point(vx1, vy1),
                            IsFilled = true,
                            Segments =
                            {
                                new LineSegment { Point = new Point(vx2, vy2), },
                                new LineSegment { Point = new Point(vx3, vy3), },
                            },
                            IsClosed = true
                        }
                    }
                },
            };

            if (triangle.Render12)
            {
                this.Canvas.Children.Add(new Line { X1 = vx1, Y1 = vy1, X2 = vx2, Y2 = vy2, StrokeThickness = 3, Stroke = new SolidColorBrush(Colors.Cyan) });
            }

            if (triangle.Render23)
            {
                this.Canvas.Children.Add(new Line { X1 = vx2, Y1 = vy2, X2 = vx3, Y2 = vy3, StrokeThickness = 3, Stroke = new SolidColorBrush(Colors.Cyan) });
            }

            if (triangle.Render13)
            {
                this.Canvas.Children.Add(new Line { X1 = vx1, Y1 = vy1, X2 = vx3, Y2 = vy3, StrokeThickness = 3, Stroke = new SolidColorBrush(Colors.Cyan) });
            }

            this.Canvas.Children.Add(path);
        }
    }
}