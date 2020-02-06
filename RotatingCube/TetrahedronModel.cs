using System.Collections.Generic;
using System.Windows.Media;

namespace RotatingCube
{
    public class TetrahedronModel : IModel
    {
        private int c;
        public TetrahedronModel(int c)
        {
            this.c = c;
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                yield return new Triangle { X1 = c, Y1 = c, Z1 = c, X2 = -c, Y2 = -c, Z2 = c, X3 = -c, Y3 = c, Z3 = -c, Fill = Colors.Red };
                yield return new Triangle { X1 = c, Y1 = c, Z1 = c, X2 = -c, Y2 = c, Z2 = -c, X3 = c, Y3 = -c, Z3 = -c, Fill = Colors.Green };
                yield return new Triangle { X1 = c, Y1 = c, Z1 = c, X2 = -c, Y2 = -c, Z2 = c, X3 = c, Y3 = -c, Z3 = -c, Fill = Colors.Blue };
                yield return new Triangle { X1 = -c, Y1 = -c, Z1 = c, X2 = -c, Y2 = c, Z2 = -c, X3 = c, Y3 = -c, Z3 = -c, Fill = Colors.Yellow };
            }
        }
    }
}
