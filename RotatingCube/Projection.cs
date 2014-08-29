namespace RotatingCube
{
    using System;

    public class Projection
    {
        private double zx;
        private double zy;
        private double zz;
        private double xx;
        private double xy;
        private double xz;
        private double yx;
        private double yy;
        private double yz;
        private double t14;
        private double t24;
        private double t34;

        public Projection(double ex, double ey, double ez)
        {
            // World coordinate center
            double cx = 0, cy = 0, cz = 0;

            // Upward direction
            double ux = 0, uy = 0, uz = 1;

            // Step 1: Compute the -normalize(eye to center) vector - this will be z axis
            double vx = cx - ex;
            double vy = cy - ey;
            double vz = cz - ez;
            double length = Math.Sqrt(vx * vx + vy * vy + vz * vz);
            zx = -vx / length;
            zy = -vy / length;
            zz = -vz / length;

            // i j z
            // x y z
            // X Y Z

            // Step 2: The x axis is computed by normalize(view x up)
            xx = vy * uz - vz * uy;
            xy = vz * ux - vx * uz;
            xz = vx * uy - vy * ux;
            length = Math.Sqrt(xx * xx + xy * xy + xz * xz);
            xx = xx / length;
            xy = xy / length;
            xz = xz / length;

            // Step 3: The y axis can be computed as z x x
            yx = zy * xz - zz * xy;
            yy = zz * xx - zx * xz;
            yz = zx * xy - zy * xx;

            // Step 4: The transformation matrix is given by R' * T(-eye)
            //
            // xx xy xz  0  1 0 0 ex
            // yx yy yz  0  0 1 0 ey
            // zx zy zz  0  0 0 1 ez
            //  0  0  0  1  0 0 0  1
            //  
            //  A 0  I Q   A AQ
            //  0 1  0 1   0  1
            //

            t14 = (xx * ex) + (xy * ey) + (xz * ez);
            t24 = (yx * ex) + (yy * ey) + (yz * ez);
            t34 = (zx * ex) + (zy * ey) + (zz * ez);
        }

        public double SizeFactor { get; set; }

        public void Project(double wx, double wy, double wz, out double vx, out double vy)
        {
            double ex = xx * wx + xy * wy + xz * wz - t14;
            double ey = yx * wx + yy * wy + yz * wz - t24;
            //double ez = zx * wx + zy * wy + zz * wz - t34;
            vx = this.Width / 2 + ex * 3 * SizeFactor;
            vy = this.Height / 2 - ey * 3 * SizeFactor;
        }

        // Give the coordinate of the point on a unit sphere 
        public void Inverse(double ex, double ey, out double wx, out double wy, out double wz)
        {
            // Consider the equations above as ex, ey lies on two surfaces
            // The intersection of the two surfaces will be a line, the direction of the line is given by the cross product because it must be the unique
            // vector that is perpendicular to both normals.

            double ix = xy * yz - xz * yy;
            double iy = xz * yx - xx * yz;
            double iz = xx * yy - xy * yx;

            // Now - we knew any ray pointing to the model has this direction

            // ex + t14 = xx * wx + xy * wy + xz * wz;
            // ey + t24 = yx * wx + yy * wy + yz * wz;
            // ez + t34 = zx * wx + zy * wy + zz * wz;

            // Now we have a good set of equations to solve - note that the matrix is orthogonal - great
            double tx = ex + t14;
            double ty = ey + t24;
            double tz = 0 + t34;

            wx = xx * tx + yx * ty + zx * tz;
            wy = xy * tx + yy * ty + zy * tz;
            wz = xz * tx + yz * ty + zz * tz;

            // Now we know the line equation (p = w + vi) where v is the unknown.
            // We have another constraint that the vector is unit
            // (wx - vix)^2 + (wy - viy)^2 + (wz - viz)^2 = 1
            // wx^2 - 2(wx)(ix)v + (ix)^2v^2 + ... = 1
            // (ix^2 + iy^2 + iz^2)v^2 - 2(wx ix + wy iy + wz iz) v + (wx^2 + wy^2 + wz^2) - 1 = 0
            double a = ix * ix + iy * iy + iz * iz;
            double b = -2 * (wx * ix + wy * iy + wz * iz);
            double c = wx * wx + wy * wy + wz * wz - 1;

            double delta = b * b - 4 * a * c;
            if (delta > 0)
            {
                double v = (-b + Math.Sqrt(delta)) / 2 / a;
                wx = wx - v * ix;
                wy = wy - v * iy;
                wz = wz - v * iz;
            }
            else
            {
                // What?
            }
        }

        public double Width { get; set; }

        public double Height { get; set; }
    }

}
