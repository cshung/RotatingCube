namespace RotatingCube
{
    using System;

    public class XZRotationTransform : IRotationTransform
    {
        protected override void Transform(double ix, double iy, double iz, out double ox, out double oy, out double oz)
        {
            ox = ix * Math.Cos(Angle) - iz * Math.Sin(Angle);
            oy = iy;
            oz = ix * Math.Sin(Angle) + iz * Math.Cos(Angle);
        }
    }
}
