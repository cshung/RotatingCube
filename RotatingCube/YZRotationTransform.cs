namespace RotatingCube
{
    using System;

    public class YZRotationTransform : IRotationTransform
    {
        protected override void Transform(double ix, double iy, double iz, out double ox, out double oy, out double oz)
        {
            ox = ix;
            oy = iy * Math.Cos(Angle) - iz * Math.Sin(Angle);
            oz = iy * Math.Sin(Angle) + iz * Math.Cos(Angle);
        }
    }
}
