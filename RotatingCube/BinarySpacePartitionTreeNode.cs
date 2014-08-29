namespace RotatingCube
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BinarySpacePartitionTreeNode
    {
        private List<Triangle> triangles = new List<Triangle>();
        public List<Triangle> Triangles { get { return this.triangles; } }
        private double cx;
        private double cy;
        private double cz;
        private double d;
        private BinarySpacePartitionTreeNode Left { get; set; }
        private BinarySpacePartitionTreeNode Right { get; set; }

        public static BinarySpacePartitionTreeNode Build(IEnumerable<Triangle> triangles)
        {
            if (triangles.Count() == 0)
            {
                return new BinarySpacePartitionTreeNode();
            }

            var triangleArray = triangles.ToArray();

            return BinarySpacePartitionTreeNode.Build(triangleArray);
        }

        private static BinarySpacePartitionTreeNode Build(Triangle[] triangleArray)
        {
            BinarySpacePartitionTreeNode currentNode = new BinarySpacePartitionTreeNode();
            Triangle pivot = triangleArray[0];
            currentNode.Triangles.Add(pivot);

            // The normal of the plane containing the triangle is given by cross product of two of its vectors

            double v1x = pivot.X2 - pivot.X1;
            double v1y = pivot.Y2 - pivot.Y1;
            double v1z = pivot.Z2 - pivot.Z1;

            double v2x = pivot.X3 - pivot.X1;
            double v2y = pivot.Y3 - pivot.Y1;
            double v2z = pivot.Z3 - pivot.Z1;

            currentNode.cx = v1y * v2z - v1z * v2y;
            currentNode.cy = v1z * v2x - v1x * v2z;
            currentNode.cz = v1x * v2y - v1y * v2x;

            // Now, any point X on the same plane must satisfy normal . (x - p) = 0, where p is just any point on the plane
            // take pivot.1 for now. So the plane equation becomes
            // cx (x - p1x) + cy (y - p1y) + cz (x - p1z) = 0 => cx x + cy y + cz z - d = 0 
            currentNode.d = currentNode.cx * pivot.X1 + currentNode.cy * pivot.Y1 + currentNode.cz * pivot.Z1;

            List<Triangle> left = new List<Triangle>();
            List<Triangle> right = new List<Triangle>();
            for (int i = 1; i < triangleArray.Length; i++)
            {
                Triangle test = triangleArray[i];
                double p1 = currentNode.cx * test.X1 + currentNode.cy * test.Y1 + currentNode.cz * test.Z1 - currentNode.d;
                double p2 = currentNode.cx * test.X2 + currentNode.cy * test.Y2 + currentNode.cz * test.Z2 - currentNode.d;
                double p3 = currentNode.cx * test.X3 + currentNode.cy * test.Y3 + currentNode.cz * test.Z3 - currentNode.d;
                if (Math.Abs(p1) < 1e-6 && Math.Abs(p2) < 1e-6 && Math.Abs(p3) < 1e-6)
                {
                    currentNode.Triangles.Add(test);
                }
                else if (p1 >= -1e-6 && p2 >= -1e-6 && p3 >= -1e-6)
                {
                    left.Add(test);
                }
                else if (p1 <= 1e-6 && p2 <= 1e-6 && p3 <= 1e-6)
                {
                    right.Add(test);
                }
                else
                {
                    double direction = 0;
                    int isolatedIndex = 0;
                    if ((p1 + 1e-6) * (p2 + 1e-6) > 0)
                    {
                        direction = p3;
                        isolatedIndex = 3;
                        // p3 is the isolated boy
                        double tx = test.X1;
                        double ty = test.Y1;
                        double tz = test.Z1;
                        test.X1 = test.X3;
                        test.Y1 = test.Y3;
                        test.Z1 = test.Z3;
                        test.X3 = tx;
                        test.Y3 = ty;
                        test.Z3 = tz;
                    }
                    else if ((p2 + 1e-6) * (p3 + 1e-6) > 0)
                    {
                        direction = p1;
                        isolatedIndex = 1;
                        // p1 is the isolated boy
                    }
                    else if ((p1 + 1e-6) * (p3 + 1e-6) > 0)
                    {
                        direction = p2;
                        isolatedIndex = 2;
                        double tx = test.X1;
                        double ty = test.Y1;
                        double tz = test.Z1;
                        test.X1 = test.X2;
                        test.Y1 = test.Y2;
                        test.Z1 = test.Z2;
                        test.X2 = tx;
                        test.Y2 = ty;
                        test.Z2 = tz;
                    }
                    else
                    {
                        // What?
                    }

                    // With the swap - we make sure p1 is on one side

                    double v21x = test.X2 - test.X1;
                    double v21y = test.Y2 - test.Y1;
                    double v21z = test.Z2 - test.Z1;

                    double m21 = (currentNode.cx * test.X1 + currentNode.cy * test.Y1 + currentNode.cz * test.Z1 - currentNode.d) / -(currentNode.cx * v21x + currentNode.cy * v21y + currentNode.cz * v21z);

                    double i21x = test.X1 + m21 * v21x;
                    double i21y = test.Y1 + m21 * v21y;
                    double i21z = test.Z1 + m21 * v21z;

                    double v31x = test.X3 - test.X1;
                    double v31y = test.Y3 - test.Y1;
                    double v31z = test.Z3 - test.Z1;

                    double m31 = (currentNode.cx * test.X1 + currentNode.cy * test.Y1 + currentNode.cz * test.Z1 - currentNode.d) / -(currentNode.cx * v31x + currentNode.cy * v31y + currentNode.cz * v31z);

                    double i31x = test.X1 + m31 * v31x;
                    double i31y = test.Y1 + m31 * v31y;
                    double i31z = test.Z1 + m31 * v31z;

                    Triangle splitted1 = new Triangle { X1 = test.X1, Y1 = test.Y1, Z1 = test.Z1, X2 = i21x, Y2 = i21y, Z2 = i21z, X3 = i31x, Y3 = i31y, Z3 = i31z, Fill = test.Fill, OriginalObject = test.OriginalObject };
                    Triangle splitted2 = new Triangle { X1 = test.X2, Y1 = test.Y2, Z1 = test.Z2, X2 = i21x, Y2 = i21y, Z2 = i21z, X3 = i31x, Y3 = i31y, Z3 = i31z, Fill = test.Fill, OriginalObject = test.OriginalObject };
                    Triangle splitted3 = new Triangle { X1 = test.X2, Y1 = test.Y2, Z1 = test.Z2, X2 = i31x, Y2 = i31y, Z2 = i31z, X3 = test.X3, Y3 = test.Y3, Z3 = test.Z3, Fill = test.Fill, OriginalObject = test.OriginalObject };

                    if (isolatedIndex == 1)
                    {
                        splitted1.Render12 = test.Render12; splitted1.Render23 = false; splitted1.Render13 = test.Render13;
                        splitted2.Render12 = test.Render12; splitted2.Render23 = false; splitted3.Render13 = false;
                        splitted3.Render12 = false; splitted3.Render23 = test.Render13; splitted3.Render13 = test.Render23;
                    }
                    else if (isolatedIndex == 2)
                    {
                        splitted1.Render12 = test.Render12; splitted1.Render23 = false; splitted1.Render13 = test.Render23;
                        splitted2.Render12 = test.Render12; splitted2.Render23 = false; splitted3.Render13 = false;
                        splitted3.Render12 = false; splitted3.Render23 = test.Render23; splitted3.Render13 = test.Render13;
                    }
                    else if (isolatedIndex == 3)
                    {
                        splitted1.Render12 = test.Render23; splitted1.Render23 = false; splitted1.Render13 = test.Render13;
                        splitted2.Render12 = test.Render23; splitted2.Render23 = false; splitted3.Render13 = false;
                        splitted3.Render12 = false; splitted3.Render23 = test.Render13; splitted3.Render13 = test.Render12;
                    }
                    else
                    {
                        // What?
                    }

                    if (direction < 0)
                    {
                        right.Add(splitted1);
                        left.Add(splitted2);
                        left.Add(splitted3);
                    }
                    else
                    {
                        left.Add(splitted1);
                        right.Add(splitted2);
                        right.Add(splitted3);
                    }
                }
            }

            if (left.Count > 0)
            {
                currentNode.Left = Build(left.ToArray());
            }

            if (right.Count > 0)
            {
                currentNode.Right = Build(right.ToArray());
            }

            return currentNode;
        }
        public IEnumerable<Triangle> RenderingOrder(double cameraX, double cameraY, double cameraZ)
        {
            double p = this.cx * cameraX + this.cy * cameraY + this.cz * cameraZ - this.d;
            if (p > -1e-6)
            {
                if (this.Right != null)
                {
                    foreach (Triangle triangle in this.Right.RenderingOrder(cameraX, cameraY, cameraZ))
                    {
                        yield return triangle;
                    }
                }
                foreach (Triangle triangle in this.Triangles)
                {
                    yield return triangle;
                }
                if (this.Left != null)
                {
                    foreach (Triangle triangle in this.Left.RenderingOrder(cameraX, cameraY, cameraZ))
                    {
                        yield return triangle;
                    }
                }
            }
            else
            {
                if (this.Left != null)
                {
                    foreach (Triangle triangle in this.Left.RenderingOrder(cameraX, cameraY, cameraZ))
                    {
                        yield return triangle;
                    }
                }
                foreach (Triangle triangle in this.Triangles)
                {
                    yield return triangle;
                }
                if (this.Right != null)
                {
                    foreach (Triangle triangle in this.Right.RenderingOrder(cameraX, cameraY, cameraZ))
                    {
                        yield return triangle;
                    }
                }
            }
        }
    }
}
