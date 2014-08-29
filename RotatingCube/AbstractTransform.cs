namespace RotatingCube
{
    public abstract class AbstractTransform
    {
        protected abstract void Transform(double ix, double iy, double iz, out double ox, out double oy, out double oz);

        public Triangle Transform(Triangle original)
        {
            double tx1, ty1, tz1;
            double tx2, ty2, tz2;
            double tx3, ty3, tz3;
            this.Transform(original.X1, original.Y1, original.Z1, out tx1, out ty1, out tz1);
            this.Transform(original.X2, original.Y2, original.Z2, out tx2, out ty2, out tz2);
            this.Transform(original.X3, original.Y3, original.Z3, out tx3, out ty3, out tz3);
            return new Triangle
            {
                X1 = tx1,
                Y1 = ty1,
                Z1 = tz1,
                X2 = tx2,
                Y2 = ty2,
                Z2 = tz2,
                X3 = tx3,
                Y3 = ty3,
                Z3 = tz3,
                Fill = original.Fill,
                OriginalObject = original.OriginalObject,
                Render12 = original.Render12,
                Render23 = original.Render23,
                Render13 = original.Render13,
            };
        }
    }
}
