namespace RotatingCube
{
    using System;

    public class XYRotationTransform : IRotationTransform
    {
        protected override void Transform(double ix, double iy, double iz, out double ox, out double oy, out double oz)
        {
            ox = ix * Math.Cos(Angle) - iy * Math.Sin(Angle);
            oy = ix * Math.Sin(Angle) + iy * Math.Cos(Angle);
            oz = iz;
        }
    }
}
