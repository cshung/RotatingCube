namespace RotatingCube
{
    using System.Collections.Generic;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class CubeModel : IModel
    {
        private int c;
        private string identity;

        public Color Front { get; set; }
        public Color Back { get; set; }
        public Color Left { get; set; }
        public Color Right { get; set; }
        public Color Top { get; set; }
        public Color Bottom { get; set; }

        public CubeModel(int c, string identity)
        {
            this.c = c;
            this.identity = identity;
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                CubeFace back = new CubeFace(this, "K");
                CubeFace front = new CubeFace(this, "F");
                CubeFace left = new CubeFace(this, "L");
                CubeFace right = new CubeFace(this, "R");
                CubeFace top = new CubeFace(this, "T");
                CubeFace bottom = new CubeFace(this, "B");
                List<Triangle> result = new List<Triangle>
                {
                    new Triangle { X1 = c, Y1 = -c, Z1 = -c, X2 = c, Y2 = c, Z2 = -c, X3 = c, Y3 = c, Z3 = c, Fill = Front, OriginalObject = front},
                    new Triangle { X1 = c, Y1 = -c, Z1 = -c, X2 = c, Y2 = c, Z2 = c, X3 = c, Y3 = -c, Z3 = c, Fill = Front, OriginalObject = front}, 
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c, X2 = -c, Y2 = c, Z2 = -c, X3 = -c, Y3 = c, Z3 = c, Fill = Back, OriginalObject = back},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c, X2 = -c, Y2 = c, Z2 = c, X3 = -c, Y3 = -c, Z3 = c, Fill = Back, OriginalObject = back},
                    
                    new Triangle { X1 = -c, Y1 = c, Z1 = -c, X2 = c, Y2 = c, Z2 = -c, X3 = c, Y3 = c, Z3 = c, Fill = Right, OriginalObject = right},
                    new Triangle { X1 = -c, Y1 = c, Z1 = -c, X2 = c, Y2 = c, Z2 = c, X3 = -c, Y3 = c, Z3 = c, Fill = Right, OriginalObject = right},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c, X2 = c, Y2 = -c, Z2 = -c, X3 = c, Y3 = -c, Z3 = c, Fill = Left, OriginalObject = left},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c, X2 = c, Y2 = -c, Z2 = c, X3 = -c, Y3 = -c, Z3 = c, Fill = Left, OriginalObject = left},

                    new Triangle { X1 = -c, Y1 = -c, Z1 = c , X2 = c, Y2 = -c, Z2 = c , X3 = c, Y3 = c, Z3 = c , Fill = Top, OriginalObject = top},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = c , X2 = c, Y2 = c, Z2 = c , X3 = -c, Y3 = c, Z3 = c , Fill = Top, OriginalObject = top},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c, X2 = c, Y2 = -c, Z2 = -c , X3 = c, Y3 = c, Z3 = -c , Fill = Bottom, OriginalObject = bottom},
                    new Triangle { X1 = -c, Y1 = -c, Z1 = -c , X2 = c, Y2 = c, Z2 = -c , X3 = -c, Y3 = c, Z3 = -c , Fill = Bottom, OriginalObject = bottom},
                };
                foreach (Triangle triangle in result)
                {
                    if ((((triangle.X1 == triangle.X2) ? 1 : 0) + ((triangle.Y1 == triangle.Y2) ? 1 : 0) + ((triangle.Z1 == triangle.Z2) ? 1 : 0)) == 2)
                    {
                        triangle.Render12 = true;
                    }
                    if ((((triangle.X2 == triangle.X3) ? 1 : 0) + ((triangle.Y2 == triangle.Y3) ? 1 : 0) + ((triangle.Z2 == triangle.Z3) ? 1 : 0)) == 2)
                    {
                        triangle.Render23 = true;
                    }
                    if ((((triangle.X1 == triangle.X3) ? 1 : 0) + ((triangle.Y1 == triangle.Y3) ? 1 : 0) + ((triangle.Z1 == triangle.Z3) ? 1 : 0)) == 2)
                    {
                        triangle.Render13 = true;
                    }
                }
                return result;
            }
        }

        public string GetIdentity()
        {
            return this.identity;
        }

        class CubeFace : IObject
        {
            private CubeModel cube;
            private string face;

            public CubeFace(CubeModel cube, string face)
            {
                this.cube = cube;
                this.face = face;
            }

            public string GetIdentity()
            {
                return cube.GetIdentity() + face;
            }
        }
    }
}
